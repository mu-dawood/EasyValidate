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
    /// <param name="obj">The object being validated.</param>
    /// <param name="parentPath">The parent path for nested validation.</param>
    /// <param name="formatter">The formatter to use for error messages. If null, uses default formatter.</param>
    /// <param name="configureValidator">The validator configurator. If null, uses default configurator.</param>
    public sealed partial class ValidationResult(object obj, string[] parentPath, IFormatter? formatter, IConfigureValidator? configureValidator) : IValidationResult
    {
        /// <summary>
        /// Gets the default formatter instance used for formatting validation messages.
        /// </summary>
        /// <returns>An IFormatter instance that provides basic string formatting capabilities.</returns>
        /// <remarks>
        /// This method returns a singleton instance of the default formatter implementation
        /// that uses standard .NET string formatting.
        /// </remarks>
        /// <example>
        /// <code>
        /// var formatter = ValidationResult.GetDefaultFormatter();
        /// var result = new ValidationResult(obj, [], formatter, null);
        /// </code>
        /// </example>
        /// <docs-member>GetDefaultFormatter()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>IFormatter</docs-return-type>
        public static IFormatter GetDefaultFormatter() => new DefaultFormatter();

        /// <summary>
        /// Gets the default configure validator instance used for configuring validation attributes.
        /// </summary>
        /// <returns>An IConfigureValidator instance that provides basic configuration capabilities.</returns>
        /// <remarks>
        /// This method returns a singleton instance of the default configure validator implementation
        /// that returns validators unchanged.
        /// </remarks>
        /// <example>
        /// <code>
        /// var configureValidator = ValidationResult.GetDefaultConfigureValidator();
        /// var result = new ValidationResult(obj, [], null, configureValidator);
        /// </code>
        /// </example>
        /// <docs-member>GetDefaultConfigureValidator()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>IConfigureValidator</docs-return-type>
        public static IConfigureValidator GetDefaultConfigureValidator() => new DefaultConfigureValidator();

        /// <summary>
        /// Gets the default formatter instance used for formatting validation messages.
        /// </summary>
        /// <returns>An IFormatter instance that provides basic string formatting capabilities.</returns>
        /// <remarks>
        /// This method returns a singleton instance of the default formatter implementation
        /// that uses standard .NET string formatting.
        /// </remarks>
        /// <example>
        /// <code>
        /// var formatter = ValidationResult.GetDefaultFormatter();
        /// var result = new ValidationResult(formatter);
        /// </code>
        /// </example>
        /// <docs-member>GetDefaultFormatter()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>IFormatter</docs-return-type>
        private readonly IFormatter _formatter = formatter ?? GetDefaultFormatter();
        private readonly IConfigureValidator _configureValidator = configureValidator ?? GetDefaultConfigureValidator();
        private readonly string[] _parentPath = parentPath;
        private readonly object _obj = obj;

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
        /// <docs-return-type>IReadOnlyList<ValidationError></docs-return-type>
        private readonly List<ValidationError> _errors = [];
        public IReadOnlyList<ValidationError> Errors => _errors;

        internal void AddError(ValidationError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            _errors.Add(error);
        }
        /// <summary>
        /// Creates a new validation chain for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to create a chain for.</param>
        /// <param name="chain">The optional chain identifier for grouping validations.</param>
        /// <returns>A new ValidationChain instance configured for the specified property.</returns>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        /// <remarks>
        /// This method creates a validation chain that can be used to add and execute validation attributes
        /// for a specific property. The chain supports conditional validation and error collection.
        /// </remarks>
        /// <docs-member>CreateChain()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>ValidationChain</docs-return-type>
        public ValidationChain CreateChain(string propertyName, string chain)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return new ValidationChain(propertyName, this, _obj, _formatter, _configureValidator, chain, _parentPath);
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
            var result = other.Validate(_formatter, _configureValidator, [.. _parentPath, memberName]);
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

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _errors.Select(error => error.FormattedMessage));
        }


        /// <summary>
        /// Default formatter implementation that provides basic string formatting using standard .NET string formatting.
        /// </summary>
        private class DefaultFormatter : IFormatter
        {
            public string Format(string message, params object[] args)
            {
                return string.Format(message, args);
            }

            public string GetFormatedMessage<I, O>(IValidationAttribute<I, O> attribute, object?[] args)
            {
                return string.Format(attribute.ErrorMessage, args);
            }
        }
        /// <summary>
        /// Default configure validator implementation that returns validation attributes unchanged.
        /// </summary>
        private class DefaultConfigureValidator : IConfigureValidator
        {
            public IValidationAttribute<I, O> Configure<I, O>(IValidationAttribute<I, O> validator)
            {
                return validator;
            }
        }
    }

}
