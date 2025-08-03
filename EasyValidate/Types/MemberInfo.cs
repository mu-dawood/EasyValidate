using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class MemberInfo(string name, List<AttributeInfo> attributes, ITypeSymbol type, MemberType memberType, NestedConfig? nestedConfig)
{
    public string Name { get; } = name;
    public NestedConfig? NestedConfig { get; } = nestedConfig;
    public ITypeSymbol Type { get; } = type;
    public MemberType MemberType { get; } = memberType;
    public IReadOnlyList<AttributeInfo> Attributes { get; } = attributes;
}


internal enum MemberType
{
    Property,
    Field,
    Parameter
}