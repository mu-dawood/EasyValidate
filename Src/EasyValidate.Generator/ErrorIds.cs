namespace EasyValidate.Generator;

/// <summary>
/// Centralized error code definitions for EasyValidate diagnostics.
/// </summary>
internal static class ErrorIds
{
    // Attribute usage and inheritance
    public const string AttributeMustInheritFromSystemAttribute = "EASY001"; // Validation attribute must inherit from System.Attribute
    public const string AttributeMustHaveProperUsage = "EASY002"; // Validation attribute must have proper AttributeUsage
    public const string ValidateAttributeMustImplementGeneric = "EASY003"; // Validation attribute must implement IValidationAttribute<T>

    // Numeric validation
    public const string DivisibleByAttributeDivisorCannotBeZero = "EASY004"; // The divisor passed to 'DivisibleByAttribute' cannot be zero
    public const string PowerOfAttributeValueMustBeGreaterThanOne = "EASY005"; // The value passed to 'PowerOfAttribute' must be greater than 1

    // Collection validation
    public const string CollectionElementTypeMismatch = "EASY006"; // The element passed to '{0}' must match the property type

    // Chain validation
    public const string DuplicateChainName = "EASY007"; // Multiple validation attributes with the same chain name '{0}' found on member '{1}'.
    public const string ChainNeedsReordering = "EASY008"; // Validation chain attributes need reordering
    public const string NeedsNotNullInjection = "EASY009"; // Validation chain needs NotNull attribute injection
    public const string IncompatibleChainTypes = "EASY010"; // Validation chain has incompatible types

    // Conditional method validation
    public const string ConditionalMethodIsMissing = "EASY011"; // Conditional method is missing on the containing class
    public const string ConditionalMethodInvalidParameterLength = "EASY012"; // Conditional method must accept exactly one parameter of type IValidationResult
    public const string ConditionalMethodFirstParameterTypeMismatch = "EASY013"; // Conditional method first parameter must be of type IChainResult
    public const string ConditionalMethodReturnTypeMismatch = "EASY014"; // Conditional method must return bool or ValueTask<bool>
    public const string InvalidConditionalMethodName = "EASY015"; // Conditional method name is not a valid C# method name

    // Validation context and type
    public const string ValidateAttributeUsageMissingType = "EASY016"; // Missing required validation type(s) (interface/class)
    public const string ValidationContextPropertyDiagnostic = "EASY017"; // ValidationContext property diagnostic

    // Method and generic validation
    public const string PublicMethodCanCauseConfusion = "EASY018"; // Public method with validation attributes can cause confusion in validation processing

    public const string ConflictingBaseClassInheritance = "EASY019"; // Containing class inherits another class and is required to inherit required base class '{0}'.

}