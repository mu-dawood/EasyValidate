namespace EasyValidate.Generator;

internal class ErrorIds
{
    public const string MustInheritFromAttribute = "EASY001"; // Validation attribute must inherit from System.Attribute
    public const string MustHaveProperAttributeUsage = "EASY002"; // Validation attribute must have proper AttributeUsage
    public const string DivisorCannotBeZero = "EASY003"; // The divisor passed to 'DivisibleByAttribute' cannot be zero
    public const string PowerOfValueMustBeGreaterThanOne = "EASY004"; // The value passed to 'PowerOfAttribute' must be greater than 1
    public const string CollectionElementTypeMismatch = "EASY005"; // The element passed to '{0}' must match the property type
    public const string ConditionalMethodError = "EASY008"; // Conditional method '{0}' {1}
    public const string DuplicateChainName = "EASY009"; // Multiple validation attributes with the same chain name '{0}' found on member '{1}'.
    public const string ChainNeedsReordering = "EASY010"; // Validation chain attributes need reordering
    public const string NeedsNotNullInjection = "EASY011"; // Validation chain needs NotNull attribute injection
    public const string IncompatibleChainTypes = "EASY012"; // Validation chain has incompatible types
    public const string ConditionalMethodInvalidStrategyError = "EASY013"; // Conditional method strategy error
    public const string ConditionalMethodMissingError = "EASY014"; // Conditional method missing error
    public const string ConditionalMethodInvalidNameError = "EASY015"; //Conditional method contains unsupported chars
    public const string ValidateAttributeUsageMissingType = "EASY016"; // Missing required validation type(s) (interface/class)
    public const string ValidationContextPropertyDiagnostic = "EASY017";
    public const string ValidateAttributeUsagePublicMethod = "EASY018"; // Public method with validation chain attributes
    public const string ValidateAttributeMustImplmentGeneric = "EASY019"; // Validation attribute must implement IValidationAttribute<T>
    public const string ConflictingBaseClassInheritance = "EASY020"; // Containing class inherits another class and is required to inherit required base class '{0}'.

}