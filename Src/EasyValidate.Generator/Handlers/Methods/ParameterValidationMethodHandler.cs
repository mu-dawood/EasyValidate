using System.Collections.Generic;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Helpers;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers.Methods
{
    /// <summary>
    /// Generates individual private validation methods for each property.
    /// </summary>
    internal class ParameterValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            // Process members
            foreach (var method in @params.Target.Methods)
            {
                List<string> awaitableMembersList = [];
                foreach (var member in method.Parmters)
                {
                    if (GeneratePropertyValidationMethod(sb, method, member))
                    {
                        awaitableMembersList.Add(member.Name);
                    }
                }
                method.SetAwaitableMembers(awaitableMembersList);
            }
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private bool GeneratePropertyValidationMethod(StringBuilder sb, MethodTarget method, Member member)
        {


            var methodName = $"Validate@{member.Name}@for@{method.Symbol.Name}".ToPascalCase();
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
                    "" => $"Default@Validate@{member.Name}@for{method.Symbol.Name}".ToPascalCase(),
                    _ => $"Validate@{member.Name}@for@{method.Symbol.Name}@{group.Key}".ToPascalCase()
                };

                if (GeneratePropertyChainMethod(chainMethodsBuilder, method.Symbol.IsStatic, member, chainMethod, group.Key, infos))
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
            var staticMethod = method.Symbol.IsStatic ? "static " : string.Empty;

            if (awaitable)
                sb.AppendLine($"        private {staticMethod}async ValueTask<IPropertyResult> {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            else
                sb.AppendLine($"        private {staticMethod}IPropertyResult {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            sb.Append(propertyBuilder);
            sb.Append(chainMethodsBuilder);
            return awaitable;

        }



        private bool GeneratePropertyChainMethod(StringBuilder sb, bool isStatic, Member member, string methodName, string chain, IReadOnlyCollection<AttributeInfo> infos)
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
            var staticMethod = isStatic ? "static " : string.Empty;

            sb.AppendLine($"        public {staticMethod}{returnType} {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = new ChainResult(config?.Formatter, {passedChainValue}, nameof({member.Name}));");
            sb.Append(propsBuilder);
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            return awaitable;
        }



        private string GetChainValue(AttributeInfo attr) => attr.Chain;
    }
}
