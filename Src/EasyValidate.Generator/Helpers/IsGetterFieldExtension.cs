using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Generator.Helpers;

internal static class IsGetterFieldExtension
{
    internal static (bool found, string? fieldName) IsGetterField(this INamedTypeSymbol classSymbol, ISymbol member)
    {
        if (member is IPropertySymbol prop)
        {
            var syntaxRef = prop.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxRef != null)
            {
                var propDecl = syntaxRef.GetSyntax() as PropertyDeclarationSyntax;
                // Expression-bodied property: public string Prop => _field;
                if (propDecl?.ExpressionBody?.Expression is IdentifierNameSyntax idName)
                {
                    if (classSymbol.GetMembers().OfType<IFieldSymbol>().Any(f => f.Name == idName.Identifier.Text))
                    {
                        return (true, idName.Identifier.Text);
                    }
                }
                // Block-bodied property: public string Prop { get { return _field; } }
                else if (propDecl?.AccessorList != null)
                {
                    var getter = propDecl.AccessorList.Accessors.FirstOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration));
                    if (getter?.Body?.Statements.Count == 1 &&
                        getter.Body.Statements[0] is ReturnStatementSyntax ret &&
                        ret.Expression is IdentifierNameSyntax idName2 &&
                        classSymbol.GetMembers().OfType<IFieldSymbol>().Any(f => f.Name == idName2.Identifier.Text))
                    {
                        return (true, idName2.Identifier.Text);
                    }
                }
            }
        }
        return (false, null);
    }
}
