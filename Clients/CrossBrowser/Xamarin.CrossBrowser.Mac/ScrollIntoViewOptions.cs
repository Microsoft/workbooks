//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Foundation;
using JavaScriptCore;

namespace Xamarin.CrossBrowser
{
    [Protocol]
    interface IScrollIntoViewOptions : IJSExport
    {
        [Export ("behavior")]
        string Behavior { get; }

        [Export ("block")]
        string Block { get; }
    }

    public sealed class ScrollIntoViewOptions : NSObject, IScrollIntoViewOptions
    {
        public ScrollIntoViewBehavior Behavior { get; set; }

        string IScrollIntoViewOptions.Behavior {
            get { return Behavior.ToString ().ToLowerInvariant (); }
        }

        public ScrollIntoViewBlock Block { get; set; }

        string IScrollIntoViewOptions.Block {
            get { return Block.ToString ().ToLowerInvariant (); }
        }
    }
}