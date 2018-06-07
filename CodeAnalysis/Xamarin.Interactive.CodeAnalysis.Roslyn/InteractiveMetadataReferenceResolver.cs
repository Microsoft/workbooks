//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Xamarin.Interactive.CodeAnalysis.Resolving;

using AssemblyIdentity = Microsoft.CodeAnalysis.AssemblyIdentity;

namespace Xamarin.Interactive.CodeAnalysis.Roslyn
{
    sealed class InteractiveMetadataReferenceResolver : MetadataReferenceResolver
    {
        readonly DependencyResolver dependencyResolver;

        ImmutableDictionary<string, PortableExecutableReference> resolvedReferences
            = ImmutableDictionary<string, PortableExecutableReference>.Empty;

        public InteractiveMetadataReferenceResolver (DependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException (nameof (dependencyResolver));

            this.dependencyResolver = dependencyResolver;
        }

        PortableExecutableReference GetPEReference (ResolvedAssembly resolvedAssembly)
        {
            if (resolvedAssembly == null)
                return null;

            PortableExecutableReference resolvedReference;

            if (!resolvedReferences.TryGetValue (resolvedAssembly.AssemblyName.Name,
                out resolvedReference))
                resolvedReferences = resolvedReferences.Add (
                    resolvedAssembly.AssemblyName.Name,
                    resolvedReference = MetadataReference.CreateFromFile (
                        resolvedAssembly.Path));

            return resolvedReference;
        }

        public override bool ResolveMissingAssemblies => true;

        public override PortableExecutableReference ResolveMissingAssembly (
            MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            if (resolvedReferences.TryGetValue (referenceIdentity.Name, out var resolvedReference))
                return resolvedReference;

            resolvedReference = GetPEReference (dependencyResolver.ResolveWithoutReferences (
                new AssemblyName (referenceIdentity.ToString ())));
            resolvedReferences = resolvedReferences.Add (referenceIdentity.Name, resolvedReference);
            return resolvedReference;
        }

        public override ImmutableArray<PortableExecutableReference> ResolveReference (
            string reference, string baseFilePath, MetadataReferenceProperties properties)
        {
            const string nugetRefScheme = "nugetref:";
            if (reference.StartsWith (nugetRefScheme, StringComparison.OrdinalIgnoreCase)) {
                reference = reference.Substring (nugetRefScheme.Length);
                return ImmutableArray<PortableExecutableReference>.Empty.Add (
                    PortableExecutableReference.CreateFromFile (reference, properties));
            }

            // Attempt to resolve by file name (will be combined with the
            // DependencyResolver.BaseFilePath and checked as a file),
            // then by assembly name.
            var resolvedAssembly = dependencyResolver.ResolveWithoutReferences (reference);

            if (resolvedAssembly == null)
                resolvedAssembly = dependencyResolver.ResolveWithoutReferences (
                    new AssemblyName (reference));

            var result = ImmutableArray<PortableExecutableReference>.Empty;
            var resolvedReference = GetPEReference (resolvedAssembly);

            if (resolvedReference == null)
                return result;

            return result.Add (resolvedReference);
        }

        public override bool Equals (object other) => ((object)this).Equals (other);
        public override int GetHashCode () => ((object)this).GetHashCode ();
    }
}