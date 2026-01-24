namespace EasyValidate.Abstractions;

/// <summary>
/// Specifies options for the generated validation method.
/// Apply this attribute to a method or constructor to customize the generated validation method.
/// </summary>
/// <example>
/// <code>
/// public partial class User : IGenerate
/// {
///     // Generates: internal static IValidationResult&lt;User&gt; CreateUser(...)
///     [GeneratedMethod(MethodName = "CreateUser", AccessModifier = AccessModifier.Internal)]
///     private User([NotNull] string name, int age) { }
///     
///     // Generates: public IValidationResult ValidateAndUpdate(...)
///     [GeneratedMethod(MethodName = "ValidateAndUpdate")]
///     private void Update([NotNull] string name, int age) { ... }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
public class GeneratedMethodAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the generated validation method.
    /// For constructors, defaults to "Create" if not specified.
    /// For methods, defaults to the original method name if not specified.
    /// </summary>
    public string? MethodName { get; set; }

    /// <summary>
    /// Gets or sets the access modifier for the generated validation method.
    /// If not specified, defaults to <see cref="AccessModifier.Public"/>.
    /// </summary>
    public AccessModifier AccessModifier { get; set; } = AccessModifier.Public;
}
