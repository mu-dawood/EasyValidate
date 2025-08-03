using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Helpers;

public static class FinalizeMembersExtensions
{
    internal static List<MemberInfo> FinalizeMembers(this IEnumerable<ISymbol> members, Dictionary<string, string> instanceNames, AttributeArgumentHandler argumentHandler, INamedTypeSymbol classSymbol)
    {
        List<MemberInfo> memberInfos = [];
        foreach (var member in members)
        {
            MemberType memberType;
            string name;
            ITypeSymbol type;
            if (member is IFieldSymbol fieldSymbol)
            {

                if (fieldSymbol.IsBackingField()) continue; // Skip backing fields
                memberType = MemberType.Field;
                name = fieldSymbol.Name;
                type = fieldSymbol.Type;
            }
            else if (member is IPropertySymbol propertySymbol)
            {
                memberType = MemberType.Property;
                name = propertySymbol.Name;
                type = propertySymbol.Type;
            }
            else if (member is IParameterSymbol parameterSymbol)
            {
                memberType = MemberType.Parameter;
                name = parameterSymbol.Name;
                type = parameterSymbol.Type;
            }
            else
                continue; // Skip unsupported member types


            List<AttributeInfo> attributes = [];

            foreach (var attr in member.GetAttributes())
            {
                if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                {
                    var instnceDeclration = attr.GenerateAttributeInitialization(argumentHandler);

                    var conditionalMethod = attr.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
                    ConditionalMethodInfo? conditionalMethodInfo = null;
                    if (!string.IsNullOrWhiteSpace(conditionalMethod))
                    {
                        /// get conditional method by the class ref
                        var method = classSymbol.GetMembers()
                        .OfType<IMethodSymbol>()
                        .FirstOrDefault((x) => x.Name == conditionalMethod && x.Parameters.Length == 1 && x.Parameters[0].Type.InheritsFrom("EasyValidate.Core.Abstraction.IChainResult"));
                        if (method != null)
                        {
                            var (isAsync, _) = method.IsAsyncMethod();
                            conditionalMethodInfo = new ConditionalMethodInfo(conditionalMethod!, isAsync);
                        }
                    }

                    var chain = attr.GetChainValue();
                    if (instanceNames.ContainsKey(instnceDeclration))
                    {
                        attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceNames[instnceDeclration], instnceDeclration, inputAndOutputTypes));
                    }
                    else if (!instanceNames.Values.Contains(attr.AttributeClass.Name))
                    {
                        instanceNames[instnceDeclration] = attr.AttributeClass.Name;
                        attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, attr.AttributeClass.Name, instnceDeclration, inputAndOutputTypes));
                    }
                    else
                    {
                        // Generate a unique instance name for the attribute
                        var baseName = attr.AttributeClass.Name;
                        var instanceName = baseName;
                        if (instanceNames.Values.Contains(instanceName) && !string.IsNullOrEmpty(chain))
                            instanceName = $"{baseName}_For_{chain}";
                        if (!instanceNames.Values.Contains(instanceName))
                        {
                            instanceNames[instnceDeclration] = instanceName;
                            attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceName, instnceDeclration, inputAndOutputTypes));
                        }
                        else
                        {
                            int suffixIndex = 0;
                            string[] suffixes = [.. Enumerable.Range('A', 26).Select(i => ((char)i).ToString())];
                            while (instanceNames.Values.Contains(instanceName))
                            {
                                string suffix = suffixIndex < suffixes.Length ? suffixes[suffixIndex] : $"_{suffixIndex}";
                                instanceName = $"{instanceName}_{suffix}";
                                suffixIndex++;
                            }
                            instanceNames[instnceDeclration] = instanceName;
                            attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceName, instnceDeclration, inputAndOutputTypes));
                        }
                    }
                }
            }

            // Check if nested validation is required
            NestedConfig? nestedConfig = null;
            if (type.ImplementsIAsyncValidate())
                nestedConfig = new NestedConfig(false, true);
            else if (type.ImplementsIValidate())
                nestedConfig = new NestedConfig(false, false);
            else if (type.IsCollectionOfIAsyncValidate())
                nestedConfig = new NestedConfig(true, true);
            else if (type.IsCollectionOfIValidate())
                nestedConfig = new NestedConfig(true, false);


            var info = new MemberInfo(name, attributes, type, memberType, nestedConfig);

            if (attributes.Count > 0)
                memberInfos.Add(info);
            else if (info.NestedConfig != null)
            {
                var (isGtterForField, fieldName) = classSymbol.IsGetterField(member);
                if (!isGtterForField || !memberInfos.Any(m => m.Name == fieldName))
                    memberInfos.Add(info);
            }
        }

        return memberInfos;
    }


    internal static bool IsBackingField(this ISymbol symbol)
    {
        if (symbol is not IFieldSymbol field)
            return false;

        // Backing fields typically have names like "<PropertyName>k__BackingField"
        return field.Name.StartsWith("<") && field.Name.EndsWith("k__BackingField");
    }
}
