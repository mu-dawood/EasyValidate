using System.Collections.Generic;
using System.Linq;
using EasyValidate.Generator.Analyzers;
using EasyValidate.Generator.Analyzers.AttributeAnalyzers;
using EasyValidate.Generator.Analyzers.ChainAnalyzers;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Helpers;

internal class MembersFinalizer(SourceProductionContext context, INamedTypeSymbol containingClass, AttributeArgumentHandler argumentHandler, Dictionary<string, string> instanceNames)
{
    private readonly SourceProductionContext _context = context;
    private readonly INamedTypeSymbol _containingClass = containingClass;

    private readonly AttributeArgumentHandler _argumentHandler = argumentHandler;
    private readonly Dictionary<string, string> _instanceNames = instanceNames;

    private readonly List<AttributeAnalyzer> attributeAnalyzers = [
            new PowerOfAttributeUsage(),
            new DivisibleByAttributeUsage(),
            new CollectionElementAttributeUsage(),
            new ConditionalMethodAttributeUsage()
    ];

    private readonly List<ChainAnalyzer> chainAnalyzers = [
          new ChainIncompatibilityProcessor(),
          new DuplicateAttributeInChainProcessor(),
    ];

    private readonly DiagnosticDescriptor MissingTypeRule = new(
                 ErrorIds.ValidateAttributeUsageMissingType,
                 "Missing Validation Type Error",
                 "Class '{0}' is missing required type(s): [{1}]",
                 "Usage",
                 DiagnosticSeverity.Error,
                 true
    );

    private readonly DiagnosticDescriptor ConflictingBaseRule = new(
                 ErrorIds.ConflictingBaseClassInheritance,
                 "Conflicting Base Class Inheritance",
                 "Class '{0}' inherits {1} and is required to inherit required base class '{2}'",
                 "Usage",
                 DiagnosticSeverity.Error,
                 true
    );

    internal List<Member> Finalize(IEnumerable<ISymbol> members, Compilation compilation)
    {

        List<Member> Members = [];
        List<string> missingTypes = [];
        foreach (var member in members)
        {
            MemberType memberType;
            string name;
            ITypeSymbol type;
            if (member is IFieldSymbol fieldSymbol)
            {

                if (IsBackingField(fieldSymbol)) continue; // Skip backing fields
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

            var analyzerContext = new AnalyserContext(_context, compilation, _containingClass, member);
            Dictionary<string, IReadOnlyCollection<AttributeInfo>> chains = [];

            var grouped = member.GetAttributes().GroupBy(attr => attr.GetChainValue())
                .OrderBy(g => string.IsNullOrEmpty(g.Key) ? 1 : 0) // Ensure empty chain comes first
                .ThenBy(g => g.Key);

            foreach (var group in grouped)
            {
                if (!group.Any()) continue; // Skip empty groups
                var chain = group.Key;
                List<AttributeInfo> attributes = [];
                var validGroup = true;
                foreach (var attr in group)
                {
                    if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                    {
                        var instnceDeclration = attr.GenerateAttributeInitialization(_argumentHandler);

                        var conditionalMethod = attr.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
                        ConditionalMethodInfo? conditionalMethodInfo = null;
                        if (!string.IsNullOrWhiteSpace(conditionalMethod))
                        {
                            // get conditional method by the class ref
                            var method = _containingClass.GetMembers()
                            .OfType<IMethodSymbol>()
                            .FirstOrDefault((x) => x.Name == conditionalMethod && x.Parameters.Length == 1 && x.Parameters[0].Type.InheritsFrom("EasyValidate.Abstractions.IChainResult"));
                            if (method != null)
                            {
                                var (isAsync, _) = method.IsAsyncMethod();
                                conditionalMethodInfo = new ConditionalMethodInfo(conditionalMethod!, isAsync);
                            }
                        }

                        var location = attr.ApplicationSyntaxReference?.GetSyntax()?.GetLocation()
                                                       ?? member.Locations.FirstOrDefault()
                                                       ?? Location.None;

                        var instanceName = string.Empty;
                        var baseName = attr.AttributeClass.Name;
                        if (_instanceNames.ContainsKey(instnceDeclration))
                            instanceName = _instanceNames[instnceDeclration];
                        else
                        {
                            baseName = string.IsNullOrEmpty(chain) ? baseName : $"{baseName}_For_{chain}";
                            int suffixIndex = 0;
                            string[] suffixes = [.. Enumerable.Range('A', 26).Select(i => ((char)i).ToString())];
                            while (_instanceNames.Values.Contains(baseName))
                            {
                                string suffix = suffixIndex < suffixes.Length ? suffixes[suffixIndex] : $"_{suffixIndex}";
                                baseName = $"{baseName}_{suffix}";
                                suffixIndex++;
                            }
                            _instanceNames[instnceDeclration] = baseName;
                            instanceName = baseName;
                        }
                        var info = new AttributeInfo(attr, location, chain, conditionalMethodInfo, instanceName, instnceDeclration, inputAndOutputTypes);
                        foreach (var analyzer in attributeAnalyzers)
                        {
                            if (!analyzer.Analyze(analyzerContext, info))
                                validGroup = false;
                        }
                        attributes.Add(info);
                        var attributeClass = attr.AttributeClass;
                        if (attributeClass != null)
                        {
                            var props = attributeClass.GetMembers()
                                                      .OfType<IPropertySymbol>()
                                                      .Where(p => p.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                                                      .ToList();
                            foreach (var prop in props)
                            {
                                // check if the property type is interface
                                var isInterface = prop.Type.TypeKind == TypeKind.Interface;

                                if (isInterface && !_containingClass.ImplementsInterface(prop.Type.GetFullName()))
                                    missingTypes.Add($"interface:{prop.Type.ToNonNullable().ToDisplayString()}");
                                else if (!isInterface && !_containingClass.InheritsFrom(prop.Type.GetFullName()))
                                {
                                    var baseType = GetDirectBaseClass(_containingClass);
                                    if (baseType != null)
                                    {
                                        // if it has base type, then it is required to inherit the required type
                                        var diagnostic = Diagnostic.Create(
                                            ConflictingBaseRule,
                                            info.Location,
                                            _containingClass.Name,
                                            baseType.ToNonNullable().ToDisplayString(),
                                            prop.Type.ToNonNullable().ToDisplayString());
                                        _context.ReportDiagnostic(diagnostic);
                                    }
                                    else
                                        // if not, add the missing type
                                        missingTypes.Add($"class:{prop.Type.ToNonNullable().ToDisplayString()}");
                                }
                            }

                        }
                    }

                }

                if (attributes.Count == 0 || !validGroup)
                    continue;

                foreach (var chainAnalyzer in chainAnalyzers)
                {
                    var (isValid, order) = chainAnalyzer.Analyze(analyzerContext, chain, attributes);
                    if (!isValid)
                        validGroup = false;
                }
                if (!validGroup)
                    continue;
                chains[chain] = attributes.AsReadOnly();
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



            if (chains.Count > 0)
            {
                var info = new Member(name, chains, type, memberType, nestedConfig);
                Members.Add(info);
            }
            else if (nestedConfig != null)
            {
                var (isGtterForField, fieldName) = _containingClass.IsGetterField(member);
                if (!isGtterForField || !Members.Any(m => m.Name == fieldName))
                {
                    var info = new Member(name, chains, type, memberType, nestedConfig);
                    Members.Add(info);
                }
            }
        }


        if (missingTypes.Count > 0)
        {
            var diagnostic = Diagnostic.Create(
                MissingTypeRule,
              _containingClass.Locations.FirstOrDefault() ?? Location.None,
                _containingClass.Name,
                string.Join(", ", missingTypes));
            _context.ReportDiagnostic(diagnostic);
        }
        return Members;
    }

    private static INamedTypeSymbol? GetDirectBaseClass(INamedTypeSymbol typeSymbol)
    {
        var baseType = typeSymbol.BaseType;

        if (baseType == null)
            return null;

        // Filter out implicit bases
        if (baseType.SpecialType == SpecialType.System_Object ||
            baseType.SpecialType == SpecialType.System_ValueType ||
            baseType.SpecialType == SpecialType.System_Enum)
        {
            return null;
        }

        return baseType;
    }
    internal bool IsBackingField(ISymbol symbol)
    {
        if (symbol is not IFieldSymbol field)
            return false;

        // Backing fields typically have names like "<PropertyName>k__BackingField"
        return field.Name.StartsWith("<") && field.Name.EndsWith("k__BackingField");
    }
}
