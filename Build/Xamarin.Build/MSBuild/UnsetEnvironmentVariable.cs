﻿//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Xamarin.MSBuild
{
    public sealed class UnsetEnvironmentVariable : Task
    {
        [Required]
        public string Name { get; set; }

        public override bool Execute ()
        {
            Environment.SetEnvironmentVariable (Name, null);
            return true;
        }
    }
}