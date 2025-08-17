namespace EasyValidate.Abstractions;
/// <summary>
/// Marks a property as a validation context, indicating that the property will be injected with a model instance.
/// <para>
/// The type of the property can be any user-defined type. The validation framework will enforce that the injected model
/// either implements, inherits, or directly matches the specified property type. This ensures type safety and allows
/// flexible model composition for validation scenarios.
/// </para>
/// <para>
/// Usage:
/// <code>
/// [ValidationContext]
/// public ICustomValidationModel MyModel { get; set; }
/// </code>
/// The framework will inject an instance of a model that implements or inherits <c>ICustomValidationModel</c>, or use the type directly if it matches.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidationContextAttribute : Attribute
{

}
