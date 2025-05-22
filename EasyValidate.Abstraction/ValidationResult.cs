using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Abstraction
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public sealed class ValidationResult
    {
        public static IFormatter GetDefaultFormatter() => new DefaultFormatter();
        private readonly IFormatter _formatter;
        public ValidationResult(IFormatter formatter)
        {
            _formatter = formatter;
        }
        /// <summary>
        /// Gets or sets a value indicating whether there are validation errors.
        /// </summary>
        public bool HasErrors() => Errors.Values.Any(errorList => errorList.Count > 0);

        /// <summary>
        /// Checks if the validation result is valid.
        /// </summary>
        public bool IsValid() => !HasErrors();

        /// <summary>
        /// Checks if there are validation errors for a specific member.
        /// </summary>
        /// <param name="memberName">The name of the member to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when memberName is null or empty.</exception>
        public bool HasErrors(string memberName)
        {
            if (string.IsNullOrEmpty(memberName))
                throw new ArgumentNullException(nameof(memberName));

            return Errors.ContainsKey(memberName) && Errors[memberName].Count > 0;
        }

        /// <summary>
        /// Checks if the validation result is valid for a specific member.
        /// </summary>
        /// <param name="memberName">The name of the member to check.</param>
        /// <returns>True if the member is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberName is null or empty.</exception>
        public bool IsValid(string memberName)
        {
            if (string.IsNullOrEmpty(memberName))
                throw new ArgumentNullException(nameof(memberName));

            return !HasErrors(memberName);
        }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        private readonly Dictionary<string, List<ValidationError>> _errors = new Dictionary<string, List<ValidationError>>();
        public IReadOnlyDictionary<string, IReadOnlyList<ValidationError>> Errors => _errors.ToDictionary(
            kvp => kvp.Key,
            kvp => (IReadOnlyList<ValidationError>)kvp.Value.AsReadOnly());


        public void TryAddError<T>(string memberName, T validator, Func<T, AttributeResult> action) where T : ValidationAttributeBase
        {
            var attributeResult = action(validator);
            if (!attributeResult.IsValid)
            {
                if (!_errors.ContainsKey(memberName))
                    _errors[memberName] = new List<ValidationError>();
                _errors[memberName].Add(new ValidationError
                {
                    ErrorCode = validator.ErrorCode,
                    Message = attributeResult.Message,
                    Args = attributeResult.MessageArgs,
                    AttributeName = validator.GetType().Name,
                    FormattedMessage = _formatter.Format(attributeResult.Message, attributeResult.MessageArgs)
                });
            }
        }

        public ValidationResult Merge(string memberName, ValidationResult other)
        {
            foreach (var kvp in other.Errors)
            {
                var key = $"{memberName}.{kvp.Key}";
                if (!_errors.ContainsKey(key))
                    _errors[key] = new List<ValidationError>();
                _errors[key].AddRange(kvp.Value);
            }
            return this;
        }


        /// <summary>
        /// Default implementation of the IFormatter interface.
        /// </summary>
        private class DefaultFormatter : IFormatter
        {
            public string Format(string message, object[] args)
            {
                return string.Format(message, args);
            }
        }
    }
}
