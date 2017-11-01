//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Interactive.Client.AgentProcesses;
using Xamarin.Interactive.Core;
using Xamarin.Interactive.Messages;

using Foundation;

[assembly: AgentProcessManager.Registration (
    "mac-xamarinmac-modern",
    typeof (AgentProcessManager<MacAgentProcess>))]

[assembly: AgentProcessManager.Registration (
    "mac-xamarinmac-full",
    typeof (AgentProcessManager<MacAgentProcess>))]

namespace Xamarin.Interactive.Client.AgentProcesses
{
    sealed class MacAgentProcess : IAgentProcess
    {
        NSTask workbookAppProcess;
        NSObject terminatedObserver;

        public WorkbookAppInstallation WorkbookApp { get; }

        public event EventHandler UnexpectedlyTerminated;

        public MacAgentProcess (WorkbookAppInstallation workbookApp)
            => WorkbookApp = workbookApp ?? throw new ArgumentNullException (nameof (workbookApp));

        public Task StartAgentProcessAsync (
            IdentifyAgentRequest identifyAgentRequest,
            IMessageService messageService,
            CancellationToken cancellationToken)
            => MainThread.SendAsync (() => {
                using (NativeExceptionHandler.Trap ())
                    StartAgentProcessOnMainThread (identifyAgentRequest);
            });

        void StartAgentProcessOnMainThread (IdentifyAgentRequest identifyAgentRequest)
        {
            MainThread.Ensure ();

            var bundle = NSBundle.FromPath (WorkbookApp.AppPath);

            workbookAppProcess = new NSTask {
                LaunchPath = bundle.ExecutablePath,
                Arguments = identifyAgentRequest.ToCommandLineArguments ()
            };

            terminatedObserver = workbookAppProcess.NotifyTerminated (
                NSNotificationCenter.DefaultCenter,
                task => {
                    terminatedObserver?.Dispose ();
                    terminatedObserver = null;

                    UnexpectedlyTerminated?.Invoke (this, EventArgs.Empty);
                });

            workbookAppProcess.Launch ();
        }

        public Task TerminateAgentProcessAsync ()
        {
            using (NativeExceptionHandler.Trap ()) {
                if (terminatedObserver != null) {
                    NSNotificationCenter.DefaultCenter.RemoveObserver (terminatedObserver);
                    terminatedObserver.Dispose ();
                    terminatedObserver = null;
                }

                if (workbookAppProcess != null && workbookAppProcess.IsRunning) {
                    workbookAppProcess.Terminate ();
                    workbookAppProcess.WaitUntilExit ();
                }

                workbookAppProcess?.Dispose ();
                workbookAppProcess = null;

                return Task.CompletedTask;
            }
        }
    }
}