; Shipped analyzer releases
; https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
EASY001 | Attribute usage and inheritance | Error | Validation attribute must inherit from `System.Attribute`
EASY002 | Attribute usage and inheritance | Error | Validation attribute must have proper `AttributeUsage`
EASY003 | Attribute usage and inheritance | Error | Validation attribute must implement `IValidationAttribute<T>`
EASY004 | Numeric validation | Error | The divisor passed to `DivisibleByAttribute` cannot be zero
EASY005 | Numeric validation | Error | The value passed to `PowerOfAttribute` must be greater than 1
EASY006 | Collection validation | Error | The element passed to `{0}` must match the property type
EASY007 | Chain validation | Error | Multiple validation attributes with the same chain name `{0}` found on member `{1}`
EASY008 | Chain validation | Error | Validation chain attributes need reordering
EASY009 | Chain validation | Error | Validation chain needs `NotNull` attribute injection
EASY010 | Chain validation | Error | Validation chain has incompatible types
EASY011 | Conditional method validation | Error | Conditional method is missing on the containing class
EASY012 | Conditional method validation | Error | Conditional method must accept exactly one parameter of type `IValidationResult`
EASY013 | Conditional method validation | Error | Conditional method first parameter must be of type `IChainResult`
EASY014 | Conditional method validation | Error | Conditional method must return `bool` or `ValueTask<bool>`
EASY015 | Conditional method validation | Error | Conditional method name is not a valid C# method name
EASY016 | Validation context and type | Error | Missing required validation type(s) (interface/class)
EASY017 | Validation context and type | Error | `ValidationContext` property diagnostic
EASY018 | Usage | Warning | EasyValidateGenerator
EASY019 | Method and generic validation | Error | Containing class inherits another class and is required to inherit required base class `{0}`