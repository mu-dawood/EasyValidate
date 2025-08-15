; Unshipped analyzer release
; https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
EASY015 | Usage | Error | Conditional method name contains unsupported characters or does not meet requirements.
EASY016 | Usage | Error | Class is missing required validation interface(s) or type(s) for its validation attributes.
EASY017 | Usage | Error | ValidationContext property rule violation in validation attribute implementation.
EASY018 | Usage | Warning | Public method has validation attributes. This is allowed, but may lead to ambiguity between the original and generated overload. To ensure the generated method is called, pass 'null' or a configuration object as the last parameter.
EASY019 | Usage | Error | Class '{0}' must implement IValidationAttribute<T> or IAsyncValidationAttribute<T> where T is the type of the property it validates you can use also implement IValidationAttribute<TInput,TOutput> for transformations