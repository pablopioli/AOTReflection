using Apparatus.AOT.Reflection.Core.Stores;
using System;
using System.Collections.Generic;

namespace Apparatus.AOT.Reflection
{
    public static class AOTReflection
    {
        public static IReadOnlyDictionary<KeyOf<TValue>?, IPropertyInfo> GetProperties<TValue>()
        {
            var data = MetadataStore<TValue>.Data ?? throw new InvalidOperationException(
                    $"Type '{typeof(TValue).FullName}' is not registered. Use 'Apparatus.AOT.Reflection.GenericHelper.Bootstrap' or extension 'GetProperties' to bootstrap it.");

            return data;
        }
    }
}