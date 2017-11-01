//
// XipSerializationBinder.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
// Copyright 2016 Microsoft. All rights reserved.

using System;
using System.Runtime.Serialization;

using Xamarin.Interactive.Representations.Reflection;

namespace Xamarin.Interactive.Serialization
{
    class XipSerializationBinder : SerializationBinder
    {
        public virtual StreamingContext Context { get; set; }

        public virtual Type BindToType (string typeName)
            => RepresentedType.GetType (typeName);

        public virtual string BindToName (Type serializedType)
            => null;

        public sealed override Type BindToType (string assemblyName, string typeName)
            => BindToType (typeName);

        public sealed override void BindToName (Type serializedType,
            out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = BindToName (serializedType);
        }
    }
}