//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Xamarin.Interactive.Representations
{
    [Serializable]
    public enum ImageFormat
    {
        Unknown,
        Png,
        Jpeg,
        Gif,
        Rgba32,
        Rgb24,
        Bgra32,
        Bgr24,
        Uri,
        Svg
    }
}