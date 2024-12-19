using Apparatus.AOT.Reflection;
using Apparatus.AOT.Reflection.Core.Stores;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class AOTReflectionExtensions
{
    public static IReadOnlyDictionary<KeyOf<TValue>?, IPropertyInfo> GetProperties<TValue>(this TValue value) =>
        AOTReflection.GetProperties<TValue>();

    public static IEnumValueInfo<TEnum> GetEnumValueInfo<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        var data = EnumMetadataStore<TEnum>.Data;
        if (data is null)
        {
            throw new Exception("data is null");
        }

        return data.Value[value];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        return Unsafe.As<TEnum, int>(ref value);
    }
}