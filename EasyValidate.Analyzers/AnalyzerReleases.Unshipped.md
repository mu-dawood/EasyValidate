; Unshipped analyzer release
; https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md


### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
EASY001 | Design | Error | Validation attribute classes must inherit from System.Attribute to be recognized as valid attributes by the .NET framework
EASY002 | Design | Error | Validation attributes must have [AttributeUsage] specified to define valid targets (Property, Field, etc.) and usage constraints
EASY003 | Usage | Error | DivisibleBy attribute validation - ensures the attribute is used correctly with compatible numeric types and valid divisor values
EASY004 | Usage | Error | PowerOf attribute validation - ensures the attribute is used with compatible numeric types and valid base values for power calculations
EASY005 | Usage | Error | Collection element attribute validation - ensures collection validation attributes are applied to appropriate collection types
EASY006 | Usage | Error | Attribute compatibility validation - detects when validation attributes are used on incompatible property/field types
EASY007 | Usage | Error | Validation attribute compatibility with nullable types - prevents applying validation attributes designed for non-nullable types to nullable properties, which may bypass validation when null
EASY008 | Usage | Error | Conditional method validation - ensures conditional validation methods are properly defined and accessible when referenced by attributes
EASY009 | Usage | Error | Duplicate chain names in validation attributes - ensures chain names are unique within a member to avoid validation ambiguity
EASY010 | Usage | Error | Incompatible validation chain attributes - attributes have incompatible input/output types and cannot be chained together
EASY011 | Usage | Error | Incompatible validation chain attributes - attributes have incompatible input/output type with member type and cannot be work together
EASY012 | Usage | Error | Incompatible validation chain attributes - attributes have fundamental type incompatibilities that cannot be resolved by reordering or NotNull injection
EASY013 | Usage | Error | Conditional method strategy error - ConditionalMethod is set but strategy is not conditional
EASY014 | Usage | Error | Conditional method missing error - Conditional execution strategy is set but ConditionalMethod is missing
EASY999 | Usage | Error | ValidateAttributeUsageAnalyzer
