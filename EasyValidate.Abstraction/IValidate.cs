
namespace EasyValidate.Abstraction
{
    /// <summary>
    /// Interface for validation.
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// Validates the object and returns a ValidationResult.
        /// </summary>
        /// <param name="formatter">The formatter to use for formatting the validation result.</param>
        /// <returns>A ValidationResult containing the validation errors.</returns>
        ValidationResult Validate(IFormatter formatter);

        /// <summary>
        /// Validates the object and returns a ValidationResult using the default formatter.
        /// </summary>
        /// <returns>A ValidationResult containing the validation errors.</returns>
        /// <remarks>
        /// This method uses the default formatter for formatting the validation result.
        /// </remarks>
        ValidationResult Validate();

    }
}