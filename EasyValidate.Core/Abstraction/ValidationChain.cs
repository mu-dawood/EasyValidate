using EasyValidate.Core.Attributes;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents a chain of validation attributes that can transform values through multiple validation steps.
    /// </summary>
    /// <typeparam name="TInput">The input type for this validation step.</typeparam>
    /// <typeparam name="TOutput">The output type after this validation step.</typeparam>
    /// <docs-display-name>Validation Chain</docs-display-name>
    /// <docs-icon>Link</docs-icon>
    /// <docs-description>A chainable validation system that allows multiple validation attributes to be linked together, with each step potentially transforming the value for the next step. Supports conditional validation and error collection.</docs-description>
    public sealed class ValidationChain
    {
        internal ValidationChain(string propertyName, ValidationResult result, object obj, IFormatter formatter, IConfigureValidator configureValidator, string chain, string[] parentPath)
        {
            _propertyName = propertyName;
            _result = result;
            _obj = obj;
            _formatter = formatter;
            _configureValidator = configureValidator;
            _chain = chain;
            _parentPath = parentPath;
        }
        private readonly ValidationResult _result;
        private readonly string _propertyName;
        private readonly object _obj;
        private readonly IFormatter _formatter;
        private readonly IConfigureValidator _configureValidator;
        private readonly string? _chain;
        private readonly string[] _parentPath;


        /// <summary>
        /// Adds a validator to the chain and executes validation with conditional logic support.
        /// </summary>
        /// <typeparam name="TInput">The input type for this validation step.</typeparam>
        /// <typeparam name="TOutput">The output type after this validation step.</typeparam>
        /// <param name="validator">The validation attribute to execute.</param>
        /// <param name="input">The input value to validate.</param>
        /// <param name="output">The output value after validation (set to default if validation fails or is skipped).</param>
        /// <returns>True if validation passes; false if validation fails or is skipped due to conditional logic.</returns>
        /// <remarks>
        /// This method executes validation with support for conditional validation. If the validator has a ConditionalMethod
        /// and that method returns false, validation is skipped. If validation fails, an error is added to the errors collection.
        /// </remarks>
        /// <docs-member>AddValidator()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>bool</docs-return-type>
        public bool AddValidtor<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, TInput input, out TOutput output)
        {

            var _validator = _configureValidator.Configure(validator);
            switch (_validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (TryInvokeConditional(_validator))
                        return ValidateAndReportError(_validator, input, out output);
                    output = default!; // Set output to default value if validation is skipped
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (TryInvokeConditional(_validator))
                        ValidateAndReportError(_validator, input, out output);
                    else
                        output = default!; // Set output to default value if validation is skipped
                    return true; // Skip validation but continue the chain
                case ExecutionStrategy.ValidateAndStop:
                    return ValidateAndReportError(_validator, input, out output);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    if (ValidateAndReportError(_validator, input, out output))
                        return true; // Validation passed, continue the chain
                    return true; // Validation failed, but continue the chain
                case ExecutionStrategy.SkipErrorAndStop:
                    var result = validator.Validate(_obj, _propertyName, input);
                    if (!result.IsValid)
                    {
                        output = default!;
                        return false;
                    }
                    else
                    {
                        output = result.TransformedValue!;
                        return true;
                    }
                default:
                    output = default!; // Set output to default value if validation is skipped
                    return false; // Skip validation if no valid strategy is defined
            }
        }

        private bool TryInvokeConditional<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator)
        {
            if (string.IsNullOrEmpty(validator.ConditionalMethod))
                return true; // No conditional method, validation always runs

            var conditionalMethod = _obj.GetType().GetMethod(validator.ConditionalMethod,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            if (conditionalMethod != null)
            {
                var shouldValidate = conditionalMethod.Invoke(_obj, [_result]);
                if (shouldValidate is bool boolResult && !boolResult)
                    return false; // Skip validation if the condition is false
            }
            return true; // Proceed with validation if no condition or condition is true
        }


        private bool ValidateAndReportError<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, TInput input, out TOutput output)
        {
            var result = validator.Validate(_obj, _propertyName, input);
            if (!result.IsValid)
            {
                _result.AddError(GetValidationError(validator, result, _formatter));
                output = default!; // Set output to default value if validation fails
                return false; // Return false to indicate validation failure
            }
            else
            {
                output = result.TransformedValue!;
                return true; // Return true if validation is successful
            }
        }

        public bool AddValidtor<TInput>(NotNullAttribute validator, TInput? input, out TInput output) where TInput : struct
        {
            if (AddValidtor<object?, object?>(validator, input, out var output1))
            {
                output = (TInput)output1!; // Cast to TOutput
                return true;
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public bool AddValidtor<TInput>(NotNullAttribute validator, TInput? input, out TInput output) where TInput : class
        {
            if (AddValidtor<object?, object?>(validator, input, out var output1))
            {
                output = (TInput)output1!; // Cast to TOutput
                return true;
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public bool AddValidtor<TInput>(GeneralValidationAttributeBase validator, TInput input, out TInput output)
        {
            if (AddValidtor<object?, object?>(validator, input, out var output1))
            {
                if (output1 is null)
                    output = default!;
                else
                    output = (TInput)output1; // Cast to TOutput
                return true;
            }
            output = default!;
            return false; // Return false if validation fails
        }
        /// <summary>
        /// Creates a ValidationError instance from a failed validation result.
        /// </summary>
        /// <param name="result">The failed validation result containing error information.</param>
        /// <param name="formatter">The formatter to use for creating the formatted error message.</param>
        /// <returns>A ValidationError instance with all necessary error details.</returns>
        /// <remarks>
        /// This method constructs a comprehensive ValidationError that includes the error code,
        /// raw message, message arguments, attribute name, and formatted message.
        /// </remarks>
        /// <docs-member>GetValidationError()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>ValidationError</docs-return-type>
        private ValidationError GetValidationError<TInput, TOutput>(IValidationAttribute<TInput, TOutput> _validator, AttributeResult<TOutput> result, IFormatter formatter)
        {
            return new ValidationError(_validator.ErrorCode, _validator.ErrorMessage, result.MessageArgs, _validator.GetType().Name, formatter.GetFormatedMessage(_validator, result.MessageArgs), [.. _parentPath, _propertyName], _chain ?? string.Empty);
        }

    }


}
