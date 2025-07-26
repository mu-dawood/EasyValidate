using System;
using System.Collections.Generic;

namespace EasyValidate.Core.Abstraction;

/// <summary>
/// Represents the basic contract for validation result operations.
/// </summary>
/// <docs-display-name>Validation Result Interface</docs-display-name>
/// <docs-icon>ClipboardCheck</docs-icon>
/// <docs-description>Base interface for validation results providing essential validation status checking capabilities.</docs-description>
public interface IChainResult
{
        /// <summary>
        /// Gets the property name associated with this validation result.
        /// </summary>
        /// <value>
        /// The property name for which this validation result was generated. Returns an empty string if not applicable.
        /// </value>
        string ChainName { get; }

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
        /// Gets the validation errors as a read-only list.
        /// </summary>
        /// <value>
        /// A read-only list containing all validation errors found during the validation process. Each error contains detailed information about the validation failure including property path, error message, chain information, and formatting details. If there are no errors, the list will be empty.
        /// </value>
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

        /// <summary>
        /// Adds a validation error for a specific attribute, type, error code, and input value to this chain result.
        /// </summary>
        /// <typeparam name="T">The type of the input value being validated.</typeparam>
        /// <param name="result">The <see cref="AttributeResult"/> representing the outcome of the attribute validation.</param>
        /// <param name="type">The <see cref="Type"/> associated with the validation result (usually the validated property or attribute type).</param>
        /// <param name="errorCode">A string representing the error code for the validation failure.</param>
        /// <param name="input">The input value that was validated.</param>
        /// <remarks>
        /// This method is typically called by validation attributes or handlers to record the result of a validation operation, including the error code and the value that was validated. The error will be added to the chain's error list.
        /// </remarks>
        /// <example>
        /// <code>
        /// chainResult.AddResult(attributeResult, typeof(string), "ERR_REQUIRED", "John");
        /// </code>
        /// </example>
        /// <docs-member>AddResult&lt;T&gt;()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>void</docs-return-type>
        void AddResult<T>(AttributeResult result, Type type, string errorCode, T input);
}
