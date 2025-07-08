using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents the result of a validation operation for any object type.
    /// </summary>
    /// <docs-display-name>Validation Result</docs-display-name>
    /// <docs-icon>ClipboardCheck</docs-icon>
    /// <docs-description>Comprehensive validation result containing all validation errors, organized by property, with formatting and error reporting capabilities. Supports validation chains and transformations.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the ValidationResult class.
    /// </remarks>
    /// <param name="parentPath">The parent path for nested validation.</param>
    /// <param name="formatter">The formatter to use for error messages. If null, uses default formatter.</param>
    /// <param name="configureValidator">The validator configurator. If null, uses default configurator.</param>
    public sealed partial class ValidationResult(IFormatter? formatter, IConfigureValidator? configureValidator, params string[] parentPath) : IValidationResult
    {
        private readonly IFormatter? _formatter = formatter;
        private readonly IConfigureValidator? _configureValidator = configureValidator;
        private readonly string[]? _parentPath = parentPath;

        /// <inheritdoc/>
        public bool HasErrors() => Errors.Any();

        /// <inheritdoc/>
        public bool IsValid() => !HasErrors();

        /// <inheritdoc/>
        public bool HasErrors(params string[] memberPath)
        {
            if (memberPath == null || memberPath.Length == 0)
                throw new ArgumentNullException(nameof(memberPath));

            return Errors.Any(error => error.Path.SequenceEqual(memberPath));
        }

        /// <inheritdoc/>
        public bool HasErrors(string[] memberPath, string chain)
        {
            if (memberPath == null || memberPath.Length == 0)
                throw new ArgumentNullException(nameof(memberPath));

            return Errors.Any(error => error.Chain == chain && error.Path.SequenceEqual(memberPath));
        }

        /// <inheritdoc/>
        public bool IsValid(params string[] memberPath)
        {
            return !HasErrors(memberPath);
        }

        /// <inheritdoc/>
        public bool IsValid(string[] memberPath, string chain)
        {
            return !HasErrors(memberPath, chain);
        }


        /// <summary>
        /// Gets the validation errors as a read-only list.
        /// </summary>
        /// <value>A read-only list containing all validation errors found during the validation process.</value>
        /// <remarks>
        /// This property provides read-only access to all validation errors. Each error contains
        /// information about the validation failure including the property path, error message, and chain information.
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
        /// <docs-return-type>IReadOnlyList of ValidationError</docs-return-type>
        private List<ValidationError>? _errors;
        public IReadOnlyList<ValidationError> Errors => _errors ??= [];

        internal void AddResult<T>(AttributeResult result, string type, string errorCode, T input, string propertyName, string chainName)
        {

            _errors ??= [];
            var error = new ValidationError(errorCode, type, _formatter?.Format(result, input) ?? string.Format(result.MessageTemplate, result.MessageArgs), _parentPath == null ? [propertyName] : [.. _parentPath, propertyName], chainName);
            _errors.Add(error);
        }


        /// <summary>
        /// Merges another ValidationResult into this result, prefixing all member names with the specified member name.
        /// </summary>
        /// <param name="memberName">The prefix to add to all member names from the other result.</param>
        /// <param name="other">The IValidate object to validate and merge into this result.</param>
        /// <remarks>
        /// This method is useful for validating nested objects where validation errors from child objects
        /// need to be included in the parent validation result with proper member path prefixes.
        /// </remarks>
        /// <example>
        /// <code>
        /// var parentResult = new ValidationResult(user, [], formatter, configurator);
        /// parentResult.MergeWith("Address", user.Address);
        /// // Errors will appear as "Address.PropertyName"
        /// </code>
        /// </example>
        /// <docs-member>MergeWith()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>void</docs-return-type>
        public void MergeWith(string memberName, IValidate other)
        {
            var result = other.Validate(_formatter, _configureValidator, _parentPath == null ? [memberName] : [.. _parentPath, memberName]);
            _errors ??= [];
            _errors.AddRange(result.Errors);
        }

        /// <summary>
        /// Merges validation results from a collection into this result, prefixing all member names with the specified member name and index.
        /// </summary>
        /// <param name="memberName">The prefix to add to all member names from the collection items.</param>
        /// <param name="collection">The IEnumerable collection to validate and merge into this result.</param>
        /// <remarks>
        /// This method is useful for validating nested collections where validation errors from child objects
        /// need to be included in the parent validation result with proper member path prefixes including array indices.
        /// </remarks>
        /// <example>
        /// <code>
        /// var parentResult = new ValidationResult(user, [], formatter, configurator);
        /// parentResult.MergeWith("Addresses", user.Addresses);
        /// // Errors will appear as "Addresses.0.PropertyName", "Addresses.1.PropertyName", etc.
        /// </code>
        /// </example>
        /// <docs-member>MergeWith()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>void</docs-return-type>
        public void MergeWith(string memberName, IEnumerable collection)
        {
            int index = 0;
            foreach (var item in collection)
            {
                if (item is IValidate validateItem)
                {
                    MergeWith($"{memberName}.{index}", validateItem);
                }
                else if (item is IEnumerable enumerableItem)
                {
                    MergeWith($"{memberName}.{index}", enumerableItem);
                }
                index++;
            }
        }
        /// <summary>
        /// Creates a new validation chain for the current object.
        /// This allows for more complex validation scenarios where multiple validations can be grouped together.
        /// The chain can be named and associated with a specific property of the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="chainName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ValidationChain CreateChain(object obj, string chainName, string propertyName)
        {
            return new ValidationChain(this, obj, chainName, propertyName);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Errors.Select(error => error.FormattedMessage));
        }

    }

}
