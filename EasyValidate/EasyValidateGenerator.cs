using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable; // for diagnostics
using EasyValidate.Abstraction.Rules;
using System;
using Microsoft.CodeAnalysis;

namespace EasyValidate
{
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        private static readonly IValidationAttributeHandler[] _handlers = new IValidationAttributeHandler[]
        {
            new RequiredHandler(),
            new NotNullHandler(),
            new NotEmptyHandler(),
            new LengthHandler(),
            new MinimumLengthHandler(),
            new MaximumLengthHandler(),
            new GreaterThanHandler(),
            new GreaterThanOrEqualToHandler(),
            new LessThanHandler(),
            new LessThanOrEqualToHandler(),
            new InclusiveBetweenHandler(),
            new MatchesHandler(),
            new EmailAddressHandler(),
            new CreditCardHandler(),
            new PhoneHandler(),
            new UrlHandler(),
            new EqualToHandler(),
            new NotEqualToHandler(),
        };

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
                            .Any(m => m.GetAttributes().Any(attr => _handlers.Any(h => h.CanHandle(attr.AttributeClass?.Name))));
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

                foreach (var cls in list)
                {
                    var src = GenerateForClass(cls, spc, conflictDescriptor);
                    spc.AddSource($"{cls.Name}_Validation.g.cs", SourceText.From(src, Encoding.UTF8));
                }
            });
        }

        private static string GenerateForClass(
            INamedTypeSymbol cls,
            SourceProductionContext spc,
            DiagnosticDescriptor conflictDescriptor)
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
            if (ns != "") sb.AppendLine(ns);
            sb.AppendLine($"    public partial class {cls.Name}");
            sb.AppendLine("    {");
            sb.AppendLine("        public List<string> Validate()");
            sb.AppendLine("        {");
            sb.AppendLine("            var errors = new List<string>();");

            foreach (var prop in cls.GetMembers().OfType<IPropertySymbol>())
            {
                if (prop.DeclaredAccessibility != Accessibility.Public || prop.IsStatic)
                    continue;

                foreach (var attr in prop.GetAttributes())
                {
                    var attributeName = attr.AttributeClass?.Name;
                    if (attributeName is null)
                        continue;
                    var constructorArgs = attr.ConstructorArguments.Select(a => a.Value).ToArray();
                    foreach (var h in _handlers)
                    {
                        if (h.CanHandle(attributeName))
                        {
                            // record helper snippets for this handler
                            foreach (var snippet in h.RequiredHelpers)
                            {
                                // extract method name from first token after return type
                                var firstLine = snippet.Split(new[] { '\n' }, StringSplitOptions.None)[0].Trim();
                                var parts = firstLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                // assume format: private static <type> <name>(...)
                                var mName = parts.Length >= 4 ? parts[3].Split('(')[0] : firstLine;
                                if (helperSnippets.TryGetValue(mName, out var existing))
                                {
                                    if (existing != snippet)
                                        spc.ReportDiagnostic(Diagnostic.Create(conflictDescriptor, Location.None, mName));
                                }
                                else
                                {
                                    helperSnippets[mName] = snippet;
                                }
                            }
                            sb.AppendLine(h.GenerateCheck(prop.Name, constructorArgs));
                            break;
                        }
                    }
                }
            }
            // also handle fields
            foreach (var field in cls.GetMembers().OfType<IFieldSymbol>())
            {
                if (field.DeclaredAccessibility != Accessibility.Public || field.IsStatic)
                    continue;
                foreach (var attr in field.GetAttributes())
                {
                    var attributeName = attr.AttributeClass?.Name;
                    if (attributeName is null)
                        continue;
                    var constructorArgs = attr.ConstructorArguments.Select(a => a.Value).ToArray();
                    foreach (var h in _handlers)
                    {
                        if (h.CanHandle(attributeName))
                        {
                            // record helper snippets
                            foreach (var snippet in h.RequiredHelpers)
                            {
                                var firstLine = snippet.Split(new[] { '\n' }, StringSplitOptions.None)[0].Trim();
                                var parts = firstLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                var mName = parts.Length >= 4 ? parts[3].Split('(')[0] : firstLine;
                                if (helperSnippets.TryGetValue(mName, out var existing) && existing != snippet)
                                    spc.ReportDiagnostic(Diagnostic.Create(conflictDescriptor, Location.None, mName));
                                else
                                    helperSnippets[mName] = snippet;
                            }
                            sb.AppendLine(h.GenerateCheck(field.Name, constructorArgs));
                            break;
                        }
                    }
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
    }
}