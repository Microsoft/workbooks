//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;

using Xamarin.Interactive.Core;

namespace Xamarin.Interactive.Compilation.Roslyn
{
    struct FilteredDiagnostics : IReadOnlyList<Diagnostic>
    {
        readonly public IReadOnlyList<Diagnostic> diagnostics;

        public int NotShownErrorCount { get; }
        public bool HasWarnings { get; }
        public bool HasErrors { get; }

        public FilteredDiagnostics (IReadOnlyList<Diagnostic> diagnostics, int maxCount)
        {
            NotShownErrorCount = 0;
            HasWarnings = false;
            HasErrors = false;

            if (diagnostics == null || diagnostics.Count == 0) {
                this.diagnostics = EmptyArray<Diagnostic>.Instance;
                return;
            }

            var filteredDiagnostics = new List<Diagnostic> ();

            foreach (var d in diagnostics) {
                HasWarnings |= d.Severity == DiagnosticSeverity.Warning;
                HasErrors |= d.Severity == DiagnosticSeverity.Error;
                if (filteredDiagnostics.Count < maxCount)
                    filteredDiagnostics.Add (d);
            }

            filteredDiagnostics.Sort ((d1, d2)
                => d1.Location.SourceSpan.Start - d2.Location.SourceSpan.Start);

            this.diagnostics = filteredDiagnostics;
            NotShownErrorCount = diagnostics.Count - maxCount;
        }

        public Diagnostic this [int index] => diagnostics [index];
        public int Count => diagnostics.Count;
        public IEnumerator<Diagnostic> GetEnumerator () => diagnostics.GetEnumerator ();
        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
    }
}