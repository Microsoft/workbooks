//
// Author:
//   Sandy Armstrong <sandy@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Xamarin.Interactive.Protocol;
using Xamarin.Interactive.Representations.Reflection;

namespace Xamarin.Interactive.Core
{
    [Serializable]
    sealed class AbortEvaluationRequest : IXipRequestMessage<Agent>
    {
        public Guid MessageId { get; } = Guid.NewGuid ();

        public int ExecutionSessionId { get; }

        public AbortEvaluationRequest (int executionSessionId)
            => ExecutionSessionId = executionSessionId;

        public void Handle (Agent agent, Action<object> responseWriter)
        {
            try {
                var thread = agent
                    .GetEvaluationContext (ExecutionSessionId)
                    .CurrentRunThread;
                if (thread != null)
                    thread.Abort ();
                responseWriter (true);
            } catch (Exception e) {
                responseWriter (new XipErrorMessage {
                    Exception = ExceptionNode.Create (e)
                });
            }
        }
    }
}