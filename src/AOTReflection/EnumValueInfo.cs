﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apparatus.AOT.Reflection
{
    public class EnumValueInfo<TEnum> : IEnumValueInfo<TEnum>, IEquatable<EnumValueInfo<TEnum>>
        where TEnum : Enum
    {
        public bool Equals(EnumValueInfo<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Name == other.Name &&
                   EqualityComparer<TEnum>.Default.Equals(Value, other.Value) &&
                   Attributes.SequenceEqual(other.Attributes);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((EnumValueInfo<TEnum>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ (Attributes != null ? Attributes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Description?.GetHashCode() ?? 0;

                return hashCode;
            }
        }

        public static bool operator ==(EnumValueInfo<TEnum> left, EnumValueInfo<TEnum> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnumValueInfo<TEnum> left, EnumValueInfo<TEnum> right)
        {
            return !Equals(left, right);
        }

        public EnumValueInfo(string name, string? description, int rawValue, TEnum value, Attribute[] attributes)
        {
            Name = name;
            RawValue = rawValue;
            Attributes = attributes;
            Description = description;
            Value = value;
        }

        public string Name { get; }

        public string? Description { get; }

        public int RawValue { get; }

        public TEnum Value { get; }

        public Attribute[] Attributes { get; }
    }
}