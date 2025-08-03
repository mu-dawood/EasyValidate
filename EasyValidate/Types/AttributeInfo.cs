using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class AttributeInfo(AttributeData attribute, string chain, ConditionalMethodInfo? conditionalMethod, string instancedName, string instanceDeclration, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    public AttributeData Attribute => attribute;

    public string Chain { get; } = chain;
    public string InstanceMethod { get; } = $"Get_{instancedName}".ToPascalCase();
    public string InstanceVariable { get; } = instancedName.ToCSharpVariableName();
    public string InstanceDeclration { get; } = instanceDeclration;

    public ConditionalMethodInfo? ConditionalMethod => conditionalMethod;

    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes => inputAndOutputTypes;


    internal  bool NeedServiceProvider()
    {
        if (Attribute.AttributeClass == null)
            return false;
        foreach (var a in Attribute.AttributeClass.GetMembers())
        {
            if (a is IPropertySymbol prop && prop.Type.GetFullName() == "global::System.IServiceProvider")
            {
                return true;
            }
        }
        return false;
    }
}
