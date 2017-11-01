﻿//
// AgentConnection.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2016 Microsoft. All rights reserved.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Interactive.Core;
using Xamarin.Interactive.Messages;

namespace Xamarin.Interactive.Client
{
	interface IAgentConnection
	{
		AgentType Type { get; }
		AgentIdentity Identity { get; }
		AgentClient Api { get; }
		ImmutableArray<string> AssemblySearchPaths { get; }
		AgentFeatures Features { get; }
		bool IsConnected { get; }
		bool IncludePeImage { get; }
	}

	sealed class AgentConnection : IAgentConnection, IDisposable
	{
		readonly IAgentTicket ticket;

		public AgentType Type { get; }
		public AgentIdentity Identity { get; }
		public AgentClient Api { get; }
		public ImmutableArray<string> AssemblySearchPaths { get; }
		public AgentFeatures Features { get; }

		public bool IncludePeImage => (!InteractiveInstallation.Default.IsMac && Type == AgentType.iOS) ||
				Type == AgentType.Android;
		public bool IsConnected => ticket != null && !ticket.IsDisposed && Api != null;

		public AgentConnection (AgentType agentType)
		{
			Type = agentType;
			AssemblySearchPaths = ImmutableArray<string>.Empty;
		}

		AgentConnection (
			IAgentTicket ticket,
			AgentType type,
			AgentIdentity identity,
			AgentClient apiClient,
			ImmutableArray<string> assemblySearchPaths,
			AgentFeatures features)
		{
			this.ticket = ticket;

			Type = type;
			Identity = identity;
			Api = apiClient;
			AssemblySearchPaths = assemblySearchPaths;
			Features = features;
		}

		void IDisposable.Dispose ()
		{
			ticket?.Dispose ();
		}

		public AgentConnection WithAgentType (AgentType agentType)
		{
			if (agentType == Type)
				return this;

			return new AgentConnection (
				ticket,
				agentType,
				Identity,
				Api,
				AssemblySearchPaths,
				Features);
		}

		public async Task<AgentConnection> ConnectAsync (
			WorkbookAppInstallation workbookApp,
			ClientSessionUri clientSessionUri,
			IMessageService messageService,
			Action<object> agentMessageHandler,
			Action disconnectedHandler,
			CancellationToken cancellationToken)
		{
			if (agentMessageHandler == null)
				throw new ArgumentNullException (nameof (agentMessageHandler));

			if (disconnectedHandler == null)
				throw new ArgumentNullException (nameof (disconnectedHandler));

			IAgentTicket ticket;

			if (clientSessionUri.SessionKind == ClientSessionKind.Workbook && Type != AgentType.Unknown)
				ticket = await workbookApp.RequestAgentTicketAsync (
					clientSessionUri,
					messageService,
					disconnectedHandler,
					cancellationToken);
			else
				ticket = new UncachedAgentTicket (
					clientSessionUri,
					messageService,
					disconnectedHandler);

			var assemblySearchPaths = ticket.AssemblySearchPaths;

			var identity = await ticket.GetAgentIdentityAsync (cancellationToken);
			if (identity == null)
				throw new Exception ("IAgentTicket.GetAgentIdentityAsync did not return an identity");

			var apiClient = await ticket.GetClientAsync (cancellationToken);
			if (apiClient == null)
				throw new Exception ("IAgentTicket.GetClientAsync did not return a client");

			apiClient.SessionCancellationToken = cancellationToken;
			apiClient.AgentMessageHandler = agentMessageHandler;

			return new AgentConnection (
				ticket,
				identity.AgentType,
				identity,
				apiClient,
				assemblySearchPaths == null
					? ImmutableArray<string>.Empty
					: assemblySearchPaths.ToImmutableArray (),
				await apiClient.GetAgentFeaturesAsync (cancellationToken));
		}

		public async Task<AgentConnection> RefreshFeaturesAsync ()
		{
			if (!IsConnected)
				throw new InvalidOperationException ("Not connected to agent");

			var features = await Api.GetAgentFeaturesAsync ();

			if (features == Features || (Features != null && Features.Equals (features)))
				return this;

			return new AgentConnection (
				ticket,
				Type,
				Identity,
				Api,
				AssemblySearchPaths,
				features);
		}

		public AgentConnection TerminateConnection ()
		{
			if (IsConnected)
				((IDisposable)this).Dispose ();

			return new AgentConnection (Type);
		}
	}
}