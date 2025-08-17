using System.Collections.Immutable;
using EasyValidate.Generator.Analyzers;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Types;

internal class AttributeInfo(AttributeData attribute, Location location, string chain, ConditionalMethodInfo? conditionalMethod, string instancedName, string instanceDeclration, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    internal AttributeData Attribute => attribute;
    internal Location Location { get; } = location;
    internal string Chain { get; } = chain;
    internal string InstanceMethod { get; } = $"Get_{instancedName}".ToPascalCase();
    internal string InstanceVariable { get; } = instancedName.ToCSharpVariableName();
    internal string InstanceDeclration { get; } = instanceDeclration;

    internal ConditionalMethodInfo? ConditionalMethod => conditionalMethod;

    internal ImmutableArray<InputAndOutputTypes> InputAndOutputTypes => inputAndOutputTypes;
   internal string FullName => attribute.AttributeClass?.GetFullName() ?? string.Empty;
    internal string Name => attribute.AttributeClass?.Name?? string.Empty;

    internal bool NeedServiceProvider()
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


    internal (bool areCompatible, InputAndOutputTypes?) CanAccept(AnalyserContext context, ITypeSymbol type)
    {
        if (attribute.AttributeClass.IsOptionalOrNotNullAttribute()) return (true, new(type, type, false, true));
        return InputAndOutputTypes.CanAccept(context.Compilation, type);
    }
}



