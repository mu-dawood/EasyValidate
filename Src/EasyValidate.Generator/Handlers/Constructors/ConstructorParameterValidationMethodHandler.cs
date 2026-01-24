using System.Collections.Generic;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Helpers;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers.Constructors
{
    /// <summary>
    /// Generates individual private validation methods for each constructor parameter.
    /// </summary>
    internal class ConstructorParameterValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            
            // Process constructors - only generate validation methods for parameters that need validation
            foreach (var constructor in @params.Target.Constructors)
            {
                List<string> awaitableMembersList = [];
                foreach (var member in constructor.ValidatedParameters)
                {
                    if (GenerateParameterValidationMethod(sb, constructor, member))
                    {
                        awaitableMembersList.Add(member.Name);
                    }
                }
                constructor.SetAwaitableMembers(awaitableMembersList);
            }
            
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }

        /// <summary>
        /// Generates a private validation method for a specific constructor parameter.
        /// </summary>
        private bool GenerateParameterValidationMethod(StringBuilder sb, ConstructorTarget constructor, Member member)
        {
            var targetMethodName = constructor.MethodName;
            var methodName = $"Validate@{member.Name}@for@{targetMethodName}".ToPascalCase();
            StringBuilder propertyBuilder = new();
            StringBuilder chainMethodsBuilder = new();
            var awaitable = false;
            
            propertyBuilder.AppendLine("        {");
            propertyBuilder.AppendLine($"            var property_result = new PropertyResult(config, nameof({member.Name}));");
            
            foreach (var group in member.Attributes)
            {
                var infos = group.Value;
                var chainMethod = group.Key switch
                {
                    "" => $"Default@Validate@{member.Name}@for@{targetMethodName}".ToPascalCase(),
                    _ => $"Validate@{member.Name}@for@{targetMethodName}@{group.Key}".ToPascalCase()
                };

                if (GenerateParameterChainMethod(chainMethodsBuilder, member, chainMethod, group.Key, infos))
                {
                    chainMethod = "await " + chainMethod;
                    awaitable = true;
                }
                propertyBuilder.AppendLine($"            property_result.AddChainResult({chainMethod}({member.Name}, config));");
            }
            
            if (member.NestedConfig != null)
            {
                if (member.NestedConfig.IsAsync)
                {
                    awaitable = true;
                    propertyBuilder.AppendLine($"            if ({member.Name} != null) await property_result.AddNestedResultAsync({member.Name});");
                }
                else
                    propertyBuilder.AppendLine($"            if ({member.Name} != null) property_result.AddNestedResult({member.Name});");
            }
            
            propertyBuilder.AppendLine("            return property_result;");
            propertyBuilder.AppendLine("        }");
            propertyBuilder.AppendLine();

            // Constructor validation methods are always static since they're used in static Create method
            if (awaitable)
                sb.AppendLine($"        private static async ValueTask<IPropertyResult> {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            else
                sb.AppendLine($"        private static IPropertyResult {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            
            sb.Append(propertyBuilder);
            sb.Append(chainMethodsBuilder);
            return awaitable;
        }

        private bool GenerateParameterChainMethod(StringBuilder sb, Member member, string methodName, string chain, IReadOnlyCollection<AttributeInfo> infos)
        {
            var passedChainValue = chain switch
            {
                null => "string.Empty",
                "" => "string.Empty",
                _ => $"\"{chain}\""
            };
            
            var propsBuilder = new StringBuilder();
            var awaitable = _processor.ProcessPropertyValidation(propsBuilder, member, infos);
            var returnType = awaitable ? "async ValueTask<IChainResult>" : "IChainResult";

            // Constructor chain methods are always static
            sb.AppendLine($"        public static {returnType} {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = new ChainResult(config?.Formatter, {passedChainValue}, nameof({member.Name}));");
            sb.Append(propsBuilder);
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            return awaitable;
        }
    }
}
