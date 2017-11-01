//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using CoreGraphics;

namespace Xamarin.Interactive.Client.Mac.CoordinateMappers
{
    abstract class AgentCoordinateMapper : IDisposable
    {
        public abstract bool TryGetLocalCoordinate (CGPoint hostCoordinate, out CGPoint localCoordinate);

        public void Dispose ()
        {
            Dispose (true);
        }

        protected virtual void Dispose (bool disposing)
        {
        }
    }
}