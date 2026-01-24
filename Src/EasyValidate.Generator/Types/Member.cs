using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Types;

internal class Member(string name, IReadOnlyDictionary<string, IReadOnlyCollection<AttributeInfo>> attributes, ITypeSymbol type, MemberType memberType, NestedConfig? nestedConfig)
{
    internal string Name { get; } = name;
    internal NestedConfig? NestedConfig { get; } = nestedConfig;
    internal ITypeSymbol Type { get; } = type;
    internal MemberType MemberType { get; } = memberType;
    internal IReadOnlyDictionary<string, IReadOnlyCollection<AttributeInfo>> Attributes { get; } = attributes;
    
    /// <summary>
    /// Indicates whether this member requires validation (has validation attributes or nested validation config).
    /// </summary>
    internal bool NeedsValidation => Attributes.Count > 0 || NestedConfig != null;
}


internal enum MemberType
{
    Property,
    Field,
    Parameter
}