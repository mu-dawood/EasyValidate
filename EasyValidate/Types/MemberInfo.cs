using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class MemberInfo(string name, List<AttributeInfo> attributes, ITypeSymbol type, bool isProperty, NestedConfig? nestedConfig)
{
    public string Name { get; } = name;
    public NestedConfig? NestedConfig { get; } = nestedConfig;
    public ITypeSymbol Type { get; } = type;
    public bool IsProperty { get; } = isProperty;
    public IReadOnlyList<AttributeInfo> Attributes { get; } = attributes;
}
