using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    public class ValidateAttributeUsageHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attr in member.GetAttributes())
                {
                    var attributeType = attr.AttributeClass;
                    if (attributeType == null)
                        continue;

                    // Add validation logic for attributes here

                    // Example: Report diagnostic if attribute usage is invalid
                    var diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            id: "EV001",
                            title: "Invalid Attribute Usage",
                            messageFormat: "{0} is not valid for {1}.",
                            category: "Usage",
                            DiagnosticSeverity.Error,
                            isEnabledByDefault: true
                        ),
                        member.Locations.FirstOrDefault()
                    );

                    context.ReportDiagnostic(diagnostic);
                }
            }

            // Call the next handler in the chain
            base.Handle(classSymbol, context, sb);
        }
    }
}
