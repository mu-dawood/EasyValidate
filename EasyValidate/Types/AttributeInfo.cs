using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class AttributeInfo(AttributeData attribute, ConditionalMethodInfo? conditionalMethod, string instanceName, string instanceDeclration, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    public AttributeData Attribute => attribute;

    public string InstanceName => instanceName;
    public string InstanceDeclration => instanceDeclration;

    public ConditionalMethodInfo? ConditionalMethod => conditionalMethod;

    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes => inputAndOutputTypes;
}
