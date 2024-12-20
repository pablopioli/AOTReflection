﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Apparatus.AOT.Reflection.SourceGenerator
{
    public static class Utils
    {
        public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol symbol)
        {
            if (symbol.BaseType != null)
            {
                foreach (var member in symbol.BaseType.GetAllMembers())
                {
                    yield return member;
                }
            }

            foreach (var member in symbol.GetMembers())
            {
                yield return member;
            }
        }

        public static IEnumerable<TSymbol> GetPublicMembers<TSymbol>(this IEnumerable<ISymbol> members)
            where TSymbol : ISymbol
        {
            return members
                .OfType<TSymbol>()
                .Where(o => o.DeclaredAccessibility.HasFlag(Accessibility.Public));
        }

        public static string GenerateAttributes(this ImmutableArray<AttributeData> attributes)
        {
            return attributes
                .Select(o =>
                {
                    var parameters = o?.AttributeConstructor?.Parameters
                        .Select((parameter, i) =>
                            new KeyValuePair<string, TypedConstant>(parameter.Name, o.ConstructorArguments[i]))
                        .Select(Convert).ToArray();

                    return
                        $@"new {o?.AttributeClass?.ToGlobalName()}({parameters?.Join()}),";
                })
                .JoinWithNewLine();
        }

        public static string Convert(KeyValuePair<string, TypedConstant> pair)
        {
            if (pair.Value.Kind == TypedConstantKind.Array && !pair.Value.IsNull)
            {
                return $@"new {pair.Value.Type?.ToString()} {pair.Value.ToCSharpString()}";
            }

            return $@"{pair.Value.ToCSharpString()}";
        }

        public static string ToGlobalName(this ISymbol symbol)
        {
            return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        public static string ToSafeGlobalName(this ISymbol symbol)
        {
            return symbol.ToGlobalName().Replace(".", "").Replace("global::", "");
        }

        public static string ToFileName(this ISymbol symbol)
        {
            return symbol.ToGlobalName().Replace("global::", "") + "Extensions.g";
        }

        public static string Join(this IEnumerable<string> values, string separator = ", ")
        {
            return string.Join(separator, values);
        }

        public static string JoinWithNewLine(this IEnumerable<string> values, string separator = "")
        {
            return string.Join($"{separator}\n\r", values);
        }
    }
}