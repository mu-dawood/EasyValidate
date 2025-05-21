using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable; // for diagnostics
using EasyValidate.Abstraction.Rules;
using System;
using Microsoft.CodeAnalysis;
using System.Reflection;
using EasyValidate.Abstraction;

namespace EasyValidate
{
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        // Handlers are instantiated at runtime by matching attribute name to handler class

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
                Debugger.Launch();
#endif
            // 1) pick up all classes where any property or field uses our validation attributes
            var candidates = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds &&
                        (cds.Members.OfType<PropertyDeclarationSyntax>().Any(p => p.AttributeLists.Count > 0)
                         || cds.Members.OfType<FieldDeclarationSyntax>().Any(f => f.AttributeLists.Count > 0)),
                    transform: (ctx, ct) =>
                    {
                        var decl = (ClassDeclarationSyntax)ctx.Node;
                        var sym = ctx.SemanticModel.GetDeclaredSymbol(decl) as INamedTypeSymbol;
                        if (sym == null) return null;
                        // confirm at least one property/field has a matching rule attribute
                        bool hasRule = sym.GetMembers()
                            .OfType<IPropertySymbol>().Cast<ISymbol>()
                            .Concat(sym.GetMembers().OfType<IFieldSymbol>())
                            .Any(m => m.GetAttributes().Any(attr =>
                            {
                                var baseType = attr.AttributeClass?.BaseType;
                                return baseType != null && baseType.ToDisplayString() == "EasyValidate.Abstraction.Attributes.ValidationAttributeBase";
                            }));
                        return hasRule ? sym : null;
                    })
                .Where(s => s != null);

            // 2) collect and emit one file per class
            context.RegisterSourceOutput(candidates.Collect(), (spc, list) =>
            {
                // diagnostic for helper conflicts
                var conflictDescriptor = new DiagnosticDescriptor(
                    "EVG002",
                    "Helper conflict",
                    "Helper method '{0}' has conflicting implementations.",
                    "EasyValidateGenerator",
                    DiagnosticSeverity.Warning,
                    isEnabledByDefault: true);

                // diagnostic for invalid attribute target types
                var invalidTargetDescriptor = new DiagnosticDescriptor(
                    "EVG003", 
                    "Invalid attribute target", 
                    "Attribute '{0}' is not compatible with type '{1}'", 
                    "EasyValidateGenerator", 
                    DiagnosticSeverity.Error, 
                    isEnabledByDefault: true);

                foreach (var cls in list)
                {
                    var src = GenerateForClass(cls, spc, conflictDescriptor, invalidTargetDescriptor);
                    spc.AddSource($"{cls.Name}_Validation.g.cs", SourceText.From(src, Encoding.UTF8));
                }
            });
        }

        private static string GenerateForClass(
            INamedTypeSymbol cls,
            SourceProductionContext spc,
            DiagnosticDescriptor conflictDescriptor,
            DiagnosticDescriptor invalidTargetDescriptor)
        {
            // collect unique helper method code by method name
            var helperSnippets = new Dictionary<string, string>();

            var ns = cls.ContainingNamespace.IsGlobalNamespace
                ? ""
                : $"namespace {cls.ContainingNamespace.ToDisplayString()}{{";

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text.RegularExpressions;");
            sb.AppendLine("using EasyValidate.Abstraction;");
            if (ns != "") sb.AppendLine(ns);
            sb.AppendLine($"    public partial class {cls.Name}");
            sb.AppendLine("    {");
            sb.AppendLine("        public List<ValidationError> Validate()");
            sb.AppendLine("        {");
            sb.AppendLine("            var errors = new List<ValidationError>();");

            // Ensure unique variable names for each attribute applied to a property or field.
            int errorCounter = 0;

            // process properties
            foreach (var prop in cls.GetMembers().OfType<IPropertySymbol>())
            {
                if (prop.DeclaredAccessibility != Accessibility.Public || prop.IsStatic)
                    continue;

                foreach (var attr in prop.GetAttributes())
                {
                    var attributeName = attr.AttributeClass?.Name;
                    if (attributeName is null || !AttributeHandlers.ContainsKey(attributeName))
                        continue;

                    if (!ValidateTarget(
                        attributeName,
                        prop.Type,
                        prop.Locations.FirstOrDefault() ?? Location.None,
                        spc,
                        invalidTargetDescriptor))
                        continue;

                    var errorVarName = $"{prop.Name}Errors{errorCounter++}";
                    sb.AppendLine($"            var {errorVarName} = new ValidationError");
                    sb.AppendLine("            {");
                    sb.AppendLine($"                PropertyName = \"{prop.Name}\"");
                    sb.AppendLine("            };");
                    sb.AppendLine(AttributeHandlers[attributeName](prop.Name, attr.ConstructorArguments.Select(a => a.Value).ToArray()).Replace("errors.Add", $"{errorVarName}.ErrorMessages.Add"));
                    sb.AppendLine($"            if ({errorVarName}.ErrorMessages.Count > 0)");
                    sb.AppendLine($"                errors.Add({errorVarName});");
                }
            }

            // handle fields similarly
            foreach (var field in cls.GetMembers().OfType<IFieldSymbol>())
            {
                if (field.DeclaredAccessibility != Accessibility.Public || field.IsStatic)
                    continue;

                foreach (var attr in field.GetAttributes())
                {
                    var attributeName = attr.AttributeClass?.Name;
                    if (attributeName is null || !AttributeHandlers.ContainsKey(attributeName))
                        continue;

                    if (!ValidateTarget(
                        attributeName,
                        field.Type,
                        field.Locations.FirstOrDefault() ?? Location.None,
                        spc,
                        invalidTargetDescriptor))
                        continue;

                    var errorVarName = $"{field.Name}Errors{errorCounter++}";
                    sb.AppendLine($"            var {errorVarName} = new ValidationError");
                    sb.AppendLine("            {");
                    sb.AppendLine($"                PropertyName = \"{field.Name}\"");
                    sb.AppendLine("            };");
                    sb.AppendLine(AttributeHandlers[attributeName](field.Name, attr.ConstructorArguments.Select(a => a.Value).ToArray()).Replace("errors.Add", $"{errorVarName}.ErrorMessages.Add"));
                    sb.AppendLine($"            if ({errorVarName}.ErrorMessages.Count > 0)");
                    sb.AppendLine($"                errors.Add({errorVarName});");
                }
            }

            sb.AppendLine("            return errors;");
            sb.AppendLine("        }");

            // append unique helper snippets
            foreach (var snippet in helperSnippets.Values)
                sb.AppendLine(snippet);

            sb.AppendLine("    }");
            if (ns != "") sb.AppendLine("}");

            return sb.ToString();
        }

        // helper: validate attribute-target compatibility and report diagnostics
        private static bool ValidateTarget(
            string attributeName,
            ITypeSymbol targetType,
            Location location,
            SourceProductionContext spc,
            DiagnosticDescriptor invalidTargetDescriptor)
        {
            if (attributeName is null)
                return false;

            var numericAttrs = new[] { "GreaterThanAttribute", "LessThanAttribute", "GreaterThanOrEqualToAttribute", "LessThanOrEqualToAttribute", "InclusiveBetweenAttribute" };
            var stringAttrs = new[] { "NotEmptyAttribute", "LengthAttribute", "MinimumLengthAttribute", "MaximumLengthAttribute", "MatchesAttribute", "EmailAddressAttribute", "PhoneAttribute", "UrlAttribute", "CreditCardAttribute" };

            if (numericAttrs.Contains(attributeName) && !IsNumericType(targetType))
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    invalidTargetDescriptor,
                    location,
                    attributeName,
                    targetType.ToDisplayString()));
                return false;
            }
            if (stringAttrs.Contains(attributeName) && targetType.SpecialType != SpecialType.System_String)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    invalidTargetDescriptor,
                    location,
                    attributeName,
                    targetType.ToDisplayString()));
                return false;
            }
            return true;
        }

        private static bool IsNumericType(ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Byte:
                case SpecialType.System_SByte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_Decimal:
                    return true;
                default:
                    return false;
            }
        }

        // Removed dynamic handler instantiation and replaced it with a predefined mapping of attribute names to handler logic.
        private static readonly Dictionary<string, Func<string, object[], string>> AttributeHandlers = new()
        {
            { "RequiredAttribute", (propertyName, args) => $"if (string.IsNullOrWhiteSpace({propertyName})) errors.Add(\"{propertyName} is required.\");" },
            { "NotNullAttribute", (propertyName, args) => $"if ({propertyName} == null) errors.Add(\"{propertyName} cannot be null.\");" },
            { "EmailAddressAttribute", (propertyName, args) => $"if (!Regex.IsMatch({propertyName}, \"^[^@\\\\s]+@[^@\\\\s]+\\\\.[^@\\\\s]+$\")) errors.Add(\"{propertyName} is not a valid email address.\");" }
            // Add more handlers as needed
        };
    }
}