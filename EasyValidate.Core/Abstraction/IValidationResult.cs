using System;
using System.Collections.Generic;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents the basic contract for validation result operations.
    /// </summary>
    /// <docs-display-name>Validation Result Interface</docs-display-name>
    /// <docs-icon>ClipboardCheck</docs-icon>
    /// <docs-description>Base interface for validation results providing essential validation status checking capabilities.</docs-description>
    public interface IValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether there are validation errors.
        /// </summary>
        /// <returns>True if there are validation errors; otherwise, false.</returns>
        /// <docs-member>HasErrors()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool HasErrors();

        /// <summary>
        /// Checks if the validation result is valid.
        /// </summary>
        /// <returns>True if there are no validation errors; otherwise, false.</returns>
        /// <example>
        /// <code>
        /// if (result.IsValid()) {
        ///     Console.WriteLine("Validation passed!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>IsValid()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool IsValid();

        /// <summary>
        /// Checks if there are validation errors for a specific member path.
        /// </summary>
        /// <param name="memberPath">The path array representing the property hierarchy (e.g., ["Address", "Street"]).</param>
        /// <returns>True if the specified member path has validation errors; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberPath is null or empty.</exception>
        /// <example>
        /// <code>
        /// if (result.HasErrors("Email")) {
        ///     Console.WriteLine("Email validation failed!");
        /// }
        /// // For nested properties:
        /// if (result.HasErrors("Address", "Street")) {
        ///     Console.WriteLine("Address.Street validation failed!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>HasErrors(string[])</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool HasErrors(params string[] memberPath);

        /// <summary>
        /// Checks if there are validation errors for a specific member path within a validation chain.
        /// </summary>
        /// <param name="memberPath">The path array representing the property hierarchy.</param>
        /// <param name="chain">The validation chain identifier to filter by.</param>
        /// <returns>True if the specified member path has validation errors in the given chain; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberPath is null or empty.</exception>
        /// <example>
        /// <code>
        /// if (result.HasErrors(new[] { "Email" }, "primary")) {
        ///     Console.WriteLine("Email validation failed in primary chain!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>HasErrors(string[], string?)</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool HasErrors(string[] memberPath, string chain);

        /// <summary>
        /// Checks if the validation result is valid for a specific member path.
        /// </summary>
        /// <param name="memberPath">The path array representing the property hierarchy (e.g., ["Address", "Street"]).</param>
        /// <returns>True if the specified member path is valid (no errors); otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberPath is null or empty.</exception>
        /// <example>
        /// <code>
        /// if (result.IsValid("Username")) {
        ///     Console.WriteLine("Username is valid!");
        /// }
        /// // For nested properties:
        /// if (result.IsValid("User", "Profile", "Name")) {
        ///     Console.WriteLine("User.Profile.Name is valid!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>IsValid(string[])</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool IsValid(params string[] memberPath);

        /// <summary>
        /// Checks if the validation result is valid for a specific member path within a validation chain.
        /// </summary>
        /// <param name="memberPath">The path array representing the property hierarchy.</param>
        /// <param name="chain">The validation chain identifier to filter by.</param>
        /// <returns>True if the specified member path is valid (no errors) in the given chain; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberPath is null or empty.</exception>
        /// <example>
        /// <code>
        /// if (result.IsValid(new[] { "Email" }, "secondary")) {
        ///     Console.WriteLine("Email is valid in secondary chain!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>IsValid(string[], string?)</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool IsValid(string[] memberPath, string chain);

        /// <summary>
        /// Gets the validation errors as a read-only list.
        /// </summary>
        /// <value>A read-only list containing all validation errors found during the validation process.</value>
        /// <remarks>
        /// This property provides access to all validation errors found during the validation process.
        /// Each error contains detailed information about the validation failure including property path,
        /// error message, chain information, and formatting details. If there are no errors, the list will be empty.
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach (var error in result.Errors) {
        ///     Console.WriteLine($"Property: {string.Join(".", error.Path)}");
        ///     Console.WriteLine($"Error: {error.FormattedMessage}");
        ///     if (error.Chain != null) {
        ///         Console.WriteLine($"Chain: {error.Chain}");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Errors</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>IReadOnlyList&lt;ValidationError&gt;</docs-return-type>
        IReadOnlyList<ValidationError> Errors { get; }
        
        
    }
}
