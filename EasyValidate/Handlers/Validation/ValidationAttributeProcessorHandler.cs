using EasyValidate;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers.Validation
{
    /// <summary>
    /// Coordinates validation processing for properties by delegating to specialized handlers.
    /// </summary>
    internal class ValidationAttributeProcessorHandler(Compilation compilation)
    {
        private readonly NestedValidationHandler _nestedHandler = new NestedValidationHandler();
        private readonly Compilation _compilation = compilation;

        /// <summary>
        /// Determines if a property requires a validation method to be generated.
        /// </summary>
        public bool ShouldGenerateValidationMethod(IPropertySymbol member)
        {
            // Check if field has validation attributes
            var hasValidationAttributes = member.GetAttributes()
                .Any(attr => attr.AttributeClass?.IsValidationAttribute() == true);

            if (hasValidationAttributes) return true;

            // Check if field requires nested validation
            return RequiresNestedValidation(member.Type);
        }

        /// <summary>
        /// Determines if a field requires a validation method to be generated.
        /// </summary>
        public bool ShouldGenerateValidationMethod(IFieldSymbol member)
        {
            // Exclude compiler-generated backing fields
            if (member.IsImplicitlyDeclared || member.Name.Contains("k__BackingField"))
                return false;
            // Check if field has validation attributes
            var hasValidationAttributes = member.GetAttributes()
                .Any(attr => attr.AttributeClass?.IsValidationAttribute() == true);

            if (hasValidationAttributes) return true;

            // Check if field requires nested validation
            return RequiresNestedValidation(member.Type);
        }

        /// <summary>
        /// Determines if a type requires nested validation.
        /// </summary>
        private bool RequiresNestedValidation(ITypeSymbol memberType)
        {
            if (IsCollection(memberType)) return true;
            // Get all properties and fields from the type
            var properties = memberType.GetMembers().OfType<IPropertySymbol>();
            // Check if any properties have validation attributes
            var hasPropertiesWithValidation = properties.Any(property =>
                property.GetAttributes().Any(attr =>
                    attr.AttributeClass?.IsValidationAttribute() == true));

            if (hasPropertiesWithValidation)
            {
                return true;
            }

            var fields = memberType.GetMembers().OfType<IFieldSymbol>();
            // Check if any fields have validation attributes
            var hasFieldsWithValidation = fields.Any(field =>
                field.GetAttributes().Any(attr =>
                    attr.AttributeClass?.IsValidationAttribute() == true));

            return hasFieldsWithValidation;

        }

        /// <summary>
        /// Determines if a type is a collection type.
        /// </summary>
        private bool IsCollection(ITypeSymbol type)
        {
            // Exclude string type (even though it implements IEnumerable<char>)
            if (type.SpecialType == SpecialType.System_String)
                return false;

            // Check for arrays first
            if (type is IArrayTypeSymbol)
                return true;

            // Check if it's a collection type (implements IEnumerable)
            if (type is INamedTypeSymbol namedType)
            {
                // Check if the collection type implements IEnumerable (generic or non-generic)
                bool isEnumerable = namedType.AllInterfaces.Any(i =>
                {
                    var interfaceName = i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return interfaceName.StartsWith("global::System.Collections.Generic.IEnumerable") ||
                           interfaceName == "global::System.Collections.IEnumerable";
                });

                return isEnumerable;
            }

            return false;
        }

        /// <summary>
        /// Processes all validation for a property including attributes and nested validation.
        /// </summary>
        public void ProcessPropertyValidation(StringBuilder sb, IPropertySymbol member)
        {
            DebuggerUtil.Log($"Processing member: {member.Name}");
            DebuggerUtil.Log($"Member type: {member.Type.ToDisplayString()}");

            List<AttributeInfo> attributes = [];
            foreach (var attr in member.GetAttributes())
            {
                if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                {
                    attributes.Add(new AttributeInfo(attr, inputAndOutputTypes));
                }
            }

            // Process validation attributes using the new CreateChain pattern
            if (attributes.Any())
            {
                ProcessValidationAttributesWithChain(sb, member.Name, member.Type, attributes);
            }

            // Process nested validation (collections and objects)
            _nestedHandler.ProcessNestedValidation(sb, member);
        }

        /// <summary>
        /// Processes all validation for a field including attributes and nested validation.
        /// </summary>
        public void ProcessFieldValidation(StringBuilder sb, IFieldSymbol member)
        {
            DebuggerUtil.Log($"Processing member: {member.Name}");
            DebuggerUtil.Log($"Member type: {member.Type.ToDisplayString()}");

            // Get all validation attributes for this field
            List<AttributeInfo> attributes = [];
            foreach (var attr in member.GetAttributes())
            {
                if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                {
                    attributes.Add(new AttributeInfo(attr, inputAndOutputTypes));
                }
            }

            // Process validation attributes using the new CreateChain pattern
            if (attributes.Any())
            {
                ProcessValidationAttributesWithChain(sb, member.Name, member.Type, attributes);
            }

            // Process nested validation (collections and objects)
            _nestedHandler.ProcessNestedValidationForField(sb, member);
        }

        /// <summary>
        /// Processes validation attributes for a member using the new CreateChain pattern.
        /// </summary>
        private void ProcessValidationAttributesWithChain(StringBuilder sb, string memberName, ITypeSymbol memberType, List<AttributeInfo> validationAttributes)
        {
            var argumentHandler = new AttributeArgumentHandler();
            string indent = "            ";

            // Group attributes by Chain property
            var groupedAttributes = validationAttributes.GroupBy(GetChainValue);
            foreach (var chainGroup in groupedAttributes)
            {
                var chainName = chainGroup.Key;
                var attributes = chainGroup.ToList();

                if (attributes.Count > 0)
                {
                    sb.AppendLine($"{indent}// Chain: {chainName}");

                    // Create chain using result.CreateChain(nameof(PropertyName), "chainName")
                    var chainVariable = $"{chainName}Chain".Replace(" ", "_").ToLowerInvariant();
                    var passedChainValue = chainName switch
                    {
                        null => "string.Empty",
                        "" => "string.Empty",
                        _ => $"\"{chainName}\""
                    };
                    sb.AppendLine($"{indent}var {chainVariable} = result.CreateChain(nameof({memberName}), {passedChainValue});");

                    // Generate validation chain with flow control
                    string currentInputVariable = memberName;
                    var attributeList = attributes.ToList();
                    var currentType = memberType;
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        var attr = attributeList[i].Attribute;
                        var (canAccept, resolvedType) = attributeList[i].InputAndOutputTypes.CanAccept(_compilation, currentType);
                        if (!canAccept)
                        {
                            sb.AppendLine($"{indent}// Proplem with chain for {attr.AttributeClass?.Name} for {currentInputVariable} as it cannot accept type {currentType.ToDisplayString()}");
                            sb.AppendLine($"{indent}throw new InvalidOperationException($\"Attribute {attr.AttributeClass?.Name} cannot accept type {currentType.ToDisplayString()} for {currentInputVariable}.\");");
                            continue;
                        }
                        var attributeInitialization = GenerateAttributeInitialization(attr, argumentHandler);

                        // Generate variable name for output (based on attribute type)
                        var attributeName = GetAttributeVariableName(attr);
                        var outputVariable = $"{chainName}_{attributeName}_Output".ToLower();

                        // Generate the AddValidtor call with flow control
                        if (attr.AttributeClass.IsNotNullAttribute())
                            sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor({attributeInitialization}, {currentInputVariable}, out var {outputVariable}))");
                        else if (attr.AttributeClass.IsGeneralAttribute())
                            sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor<{currentType.GetFullName()}>({attributeInitialization}, {currentInputVariable}, out var {outputVariable}))");
                        else
                            sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor<{resolvedType!.InputType.GetFullName()}, {resolvedType!.OutputType.GetFullName()}>({attributeInitialization}, {currentInputVariable}, out var {outputVariable}))");
                        sb.AppendLine($"{indent}    return;");

                        // Update input variable for next iteration
                        currentInputVariable = outputVariable;
                        currentType = resolvedType!.ResolveOutPutType(currentType);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the Chain property value from a validation attribute.
        /// </summary>
        private string GetChainValue(AttributeInfo attr)
        {
            var chainProperty = attr.Attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Chain");
            return chainProperty.Value.Value?.ToString() ?? "";
        }

        /// <summary>
        /// Simplifies type names by removing global:: prefix and using short forms for common types.
        /// </summary>
        private string SimplifyTypeName(string typeName)
        {
            // Remove global:: prefix
            if (typeName.StartsWith("global::"))
            {
                typeName = typeName.Substring(8);
            }

            // Map common system types to their C# keywords
            return typeName switch
            {
                "System.String" => "string",
                "System.Int32" => "int",
                "System.Int64" => "long",
                "System.Double" => "double",
                "System.Decimal" => "decimal",
                "System.Single" => "float",
                "System.Boolean" => "bool",
                "System.DateTime" => "DateTime",
                "System.Object" => "object",
                "System.Byte" => "byte",
                "System.SByte" => "sbyte",
                "System.Int16" => "short",
                "System.UInt16" => "ushort",
                "System.UInt32" => "uint",
                "System.UInt64" => "ulong",
                "System.Collections.IEnumerable" => "IEnumerable",
                _ => typeName
            };
        }

        /// <summary>
        /// Generates the attribute initialization code with constructor and named arguments.
        /// </summary>
        private string GenerateAttributeInitialization(AttributeData attr, AttributeArgumentHandler argumentHandler)
        {
            var attributeClass = attr.AttributeClass!;
            var constructorArguments = argumentHandler.FormatConstructorArguments(attr);

            // Create attribute instance with constructor arguments - use short class name since we have using directive
            var shortClassName = attributeClass.Name;

            // Handle generic attributes by adding type parameters
            if (attributeClass.IsGenericType && attributeClass.TypeArguments.Length > 0)
            {
                var typeArguments = attributeClass.TypeArguments
                    .Select(typeArg => SimplifyTypeName(typeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
                    .ToArray();
                shortClassName = $"{shortClassName}<{string.Join(", ", typeArguments)}>";
            }

            var attributeCreation = $"new {shortClassName}({string.Join(", ", constructorArguments)})";

            // If there are named arguments (property assignments), use object initialization syntax
            if (attr.NamedArguments.Any())
            {
                // Build object initializer with named arguments
                var namedArguments = attr.NamedArguments
                    .Select(namedArg => $"{namedArg.Key} = {argumentHandler.FormatArgument(namedArg.Value)}")
                    .ToList();

                return $"{attributeCreation} {{ {string.Join(", ", namedArguments)} }}";
            }
            else
            {
                return attributeCreation;
            }
        }

        /// <summary>
        /// Gets a variable name based on the attribute type for generating output variables.
        /// </summary>
        private string GetAttributeVariableName(AttributeData attr)
        {
            var attributeClass = attr.AttributeClass!;
            var className = attributeClass.Name;

            // Remove "Attribute" suffix if present
            if (className.EndsWith("Attribute"))
            {
                className = className.Substring(0, className.Length - 9);
            }

            // Convert to camelCase
            return char.ToLowerInvariant(className[0]) + className.Substring(1);
        }
    }
}



public class AttributeInfo(AttributeData attribute, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    public AttributeData Attribute { get; } = attribute;

    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes { get; } = inputAndOutputTypes;
}