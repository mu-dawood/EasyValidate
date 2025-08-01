using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class AttributeInfo(AttributeData attribute, string chain, ConditionalMethodInfo? conditionalMethod, string instancedName, string instanceDeclration, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    public AttributeData Attribute => attribute;

    public string Chain { get; } = chain;
    public string InstanceMethod { get; } = $"Get_{instancedName}".ToPascalCase();
    public string InstanceVariable { get; } = instancedName.ToSakeCase();
    public string InstanceDeclration { get; } = instanceDeclration;

    public ConditionalMethodInfo? ConditionalMethod => conditionalMethod;

    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes => inputAndOutputTypes;
}
