﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Xamarin.Interactive.CodeAnalysis;
using Xamarin.Interactive.CodeAnalysis.Evaluating;
using Xamarin.Interactive.CodeAnalysis.Models;
using Xamarin.Interactive.Remote;
using Xamarin.Interactive.Representations.Reflection;

namespace Xamarin.Interactive.Serialization
{
    sealed class
    #if EXTERNAL_INTERACTIVE_JSON_SERIALIZER_SETTINGS
    ExternalInteractiveJsonSerializerSettings
    #else
    InteractiveJsonSerializerSettings
    #endif
        : JsonSerializerSettings
    {

        #if !EXTERNAL_INTERACTIVE_JSON_SERIALIZER_SETTINGS
        public static InteractiveJsonSerializerSettings SharedInstance { get; }
            = new InteractiveJsonSerializerSettings ();
        #endif

        sealed class InteractiveJsonContractResolver : DefaultContractResolver
        {
            public InteractiveJsonContractResolver ()
            {
                NamingStrategy = new CamelCaseNamingStrategy {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = true
                };
            }

            static CustomAttributeData GetCustomAttributeNamed (MemberInfo member, string name)
                => member.CustomAttributes.FirstOrDefault (a => a.AttributeType.Name == name);

            static ConstructorInfo GetAttributeConstructor (Type objectType)
                => objectType
                    .GetConstructors (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where (c => GetCustomAttributeNamed (c, "JsonConstructorAttribute") != null)
                    .SingleOrDefault ();

            #if EXTERNAL_INTERACTIVE_JSON_SERIALIZER_SETTINGS

            protected override JsonObjectContract CreateObjectContract (Type objectType)
            {
                var contract = base.CreateObjectContract (objectType);

                if (contract.OverrideCreator == null && !objectType.IsAbstract && !objectType.IsInterface) {
                    var ctor = GetAttributeConstructor (objectType);
                    if (ctor != null) {
                        contract.OverrideCreator = args => ctor.Invoke (args);
                        contract.CreatorParameters.Clear ();
                        foreach (var parameter in CreateConstructorParameters (ctor, contract.Properties))
                            contract.CreatorParameters.Add (parameter);
                    }
                }

                return contract;
            }

            protected override JsonProperty CreateProperty (
                MemberInfo member,
                MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty (member, memberSerialization);
                if (property.PropertyType.IsEnum)
                    property.DefaultValueHandling = DefaultValueHandling.Include;
                return property;
            }

            #endif
        }

        sealed class InteractiveJsonBinder : ISerializationBinder
        {
            public void BindToName (Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;

                if (typeof (InspectView).IsAssignableFrom (serializedType))
                    typeName = typeof (InspectView).ToSerializableName ();
                else
                    typeName = serializedType.FullName;
            }

            public Type BindToType (string assemblyName, string typeName)
                => RepresentedType.GetType (typeName);
        }

        public
        #if EXTERNAL_INTERACTIVE_JSON_SERIALIZER_SETTINGS
        ExternalInteractiveJsonSerializerSettings
        #else
        InteractiveJsonSerializerSettings
        #endif
        ()
        {
            ContractResolver = new InteractiveJsonContractResolver ();
            SerializationBinder = new InteractiveJsonBinder ();

            // Non-default NJS Converters
            Converters.Add (new MonacoAwareStringEnumConverter ());
            Converters.Add (new IsoDateTimeConverter ());
            Converters.Add (new VersionConverter ());

            // Custom Converters
            Converters.Add (new CodeCellIdConverter ());
            Converters.Add (new EvaluationContextIdConverter ());
            Converters.Add (new SdkIdConverter ());
            Converters.Add (new FrameworkNameConverter ());
            Converters.Add (new IRepresentedTypeConverter ());

            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            DefaultValueHandling = DefaultValueHandling.Ignore;
            TypeNameHandling = TypeNameHandling.Objects;
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        }

        public JsonSerializer CreateSerializer ()
            => JsonSerializer.Create (this);

        #region Converters

        sealed class MonacoAwareStringEnumConverter : StringEnumConverter
        {
            public override bool CanConvert (Type objectType)
                => base.CanConvert (objectType) &&
                    objectType.GetCustomAttribute<MonacoSerializableAttribute> () == null;
        }

        sealed class CodeCellIdConverter : StringConverter<CodeCellId>
        {
            protected override CodeCellId GetValue (string value) => (CodeCellId)value;
            protected override string GetString (CodeCellId value) => (string)value;
        }

        sealed class EvaluationContextIdConverter : StringConverter<EvaluationContextId>
        {
            protected override EvaluationContextId GetValue (string value) => (EvaluationContextId)value;
            protected override string GetString (EvaluationContextId value) => (string)value;
        }

        sealed class SdkIdConverter : StringConverter<SdkId>
        {
            protected override SdkId GetValue (string value) => (SdkId)value;
            protected override string GetString (SdkId value) => (string)value;
        }
        sealed class FrameworkNameConverter : StringConverter<FrameworkName>
        {
            protected override FrameworkName GetValue (string value) => new FrameworkName (value);
            protected override string GetString (FrameworkName value) => value.FullName;
        }

        sealed class IRepresentedTypeConverter : StringConverter<IRepresentedType>
        {
            protected override IRepresentedType GetValue (string value) => RepresentedType.Lookup (value);
            protected override string GetString (IRepresentedType value) => value.Name;
        }

        abstract class StringConverter<T> : JsonConverter<T>
        {
            protected abstract T GetValue (string value);
            protected abstract string GetString (T value);

            public sealed override T ReadJson (
                JsonReader reader,
                Type objectType,
                T existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
                => GetValue ((string)reader.Value);

            public sealed override void WriteJson (
                JsonWriter writer,
                T value,
                JsonSerializer serializer)
                => writer.WriteValue (GetString (value));
        }

        #endregion
    }
}