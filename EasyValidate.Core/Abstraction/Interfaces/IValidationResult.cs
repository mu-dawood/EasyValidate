using System;
using System.Collections.Generic;

namespace EasyValidate.Core.Abstraction;

/// <summary>
/// Represents the basic contract for validation result operations.
/// </summary>
/// <docs-display-name>Validation Result Interface</docs-display-name>
/// <docs-icon>ClipboardCheck</docs-icon>
/// <docs-description>Base interface for validation results providing essential validation status checking capabilities.</docs-description>
public interface IValidationResult
{

        /// <summary>
        /// Determines whether this validation result contains any validation errors.
        /// </summary>
        /// <returns>
        /// <c>true</c> if there are validation errors; otherwise, <c>false</c>.
        /// </returns>
        /// <docs-member>HasErrors()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        /// <summary>
        /// Determines whether this validation result contains any validation errors.
        /// </summary>
        /// <returns>
        /// <c>true</c> if there are validation errors; otherwise, <c>false</c>.
        /// </returns>
        /// <docs-member>HasErrors()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool HasErrors();

        /// <summary>
        /// Gets the total number of validation errors contained in this result.
        /// </summary>
        /// <value>
        /// The number of validation errors found across all properties.
        /// </value>
        /// <example>
        /// <code>
        /// int errorCount = result.ErrorsCount;
        /// if (errorCount > 0) {
        ///     Console.WriteLine($"Total errors: {errorCount}");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ErrorsCount</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>int</docs-return-type>
        int ErrorsCount { get; }

        /// <summary>
        /// Checks if the validation result is valid (i.e., contains no errors).
        /// </summary>
        /// <returns>
        /// <c>true</c> if there are no validation errors; otherwise, <c>false</c>.
        /// </returns>
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
        /// Checks if the specified property is valid (i.e., contains no errors).
        /// </summary>
        /// <param name="propertyName">The name of the property to check for validity.</param>
        /// <returns>
        /// <c>true</c> if the specified property has no validation errors; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// if (result.IsValid("Email")) {
        ///     Console.WriteLine("Email is valid!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>IsValid(string)</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        bool IsValid(string propertyName);

        /// <summary>
        /// Gets the validation result for a specific property by name.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve the validation result for.</param>
        /// <returns>
        /// The <see cref="IPropertyResult"/> for the specified property, or <c>null</c> if not found.
        /// </returns>
        /// <example>
        /// <code>
        /// var emailResult = result.Property("Email");
        /// if (emailResult != null &amp;&amp; emailResult.HasErrors()) {
        ///     Console.WriteLine("Email validation failed!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Property(string)</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>IPropertyResult?</docs-return-type>
        IPropertyResult? Property(string propertyName);


        /// <summary>
        /// Gets all property validation results as a read-only list.
        /// </summary>
        /// <value>
        /// A read-only list containing the validation results for each property processed during validation. Each <see cref="IPropertyResult"/> provides details about the property's validation status, errors, and related information. If no properties were validated, the list will be empty.
        /// </value>
        /// <example>
        /// <code>
        /// foreach (var propertyResult in result.PropertyResults) {
        ///     Console.WriteLine($"Property: {string.Join(".", propertyResult.Path)}");
        ///     Console.WriteLine($"Is Valid: {!propertyResult.HasErrors()}");
        ///     if (propertyResult.HasErrors()) {
        ///         Console.WriteLine($"Error: {propertyResult.FormattedMessage}");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <docs-member>PropertyResults</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>IReadOnlyList&lt;IPropertyResult&gt;</docs-return-type>
        IReadOnlyList<IPropertyResult> Results { get; }

        /// <summary>
        /// Adds a property validation result to this validation result, allowing aggregation of results for multiple properties.
        /// </summary>
        /// <param name="result">The <see cref="IPropertyResult"/> to add.</param>
        /// <remarks>
        /// This method is useful for scenarios where validation results for multiple properties need to be aggregated into a single validation result instance.
        /// </remarks>
        /// <example>
        /// <code>
        /// validationResult.AddPropertyResult(propertyResult);
        /// </code>
        /// </example>
        /// <docs-member>AddPropertyResult()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>void</docs-return-type>
        void AddPropertyResult(IPropertyResult result);

}


