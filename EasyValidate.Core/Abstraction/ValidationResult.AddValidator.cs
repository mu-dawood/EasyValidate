using EasyValidate.Core.Attributes;

namespace EasyValidate.Core.Abstraction;


public sealed partial class ValidationResult
{

    /// <summary>
    /// Adds a validator to the chain and executes validation with conditional logic support.
    /// </summary>
    /// <typeparam name="TInput">The input type for this validation step.</typeparam>
    /// <typeparam name="TOutput">The output type after this validation step.</typeparam>
    /// <param name="validator">The validation attribute to execute.</param>
    /// <param name="propertyName"></param>
    /// <param name="chainName"></param>
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
    public bool AddValidtor<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, string chainName, string propertyName, TInput input, out TOutput output)
    {

        var _validator = _configureValidator.Configure(validator);
        switch (_validator.Strategy)
        {
            case ExecutionStrategy.ConditionalAndStopChain:
                if (TryInvokeConditional(_validator))
                    return ValidateAndReportError(_validator, chainName, propertyName, input, out output);
                output = default!; // Set output to default value if validation is skipped
                return false; // Skip validation if conditional method returns false
            case ExecutionStrategy.ConditionalAndContinue:
                if (TryInvokeConditional(_validator))
                    ValidateAndReportError(_validator, chainName, propertyName, input, out output);
                else
                    output = default!; // Set output to default value if validation is skipped
                return true; // Skip validation but continue the chain
            case ExecutionStrategy.ValidateAndStop:
                return ValidateAndReportError(_validator, chainName, propertyName, input, out output);
            case ExecutionStrategy.ValidateErrorAndContinue:
                if (ValidateAndReportError(_validator, chainName, propertyName, input, out output))
                    return true; // Validation passed, continue the chain
                return true; // Validation failed, but continue the chain
            case ExecutionStrategy.SkipErrorAndStop:
                var result = validator.Validate(_obj, propertyName, input, out output);
                if (!result.IsValid)
                    return false;
                else
                    return true;
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
            var shouldValidate = conditionalMethod.Invoke(_obj, [this]);
            if (shouldValidate is bool boolResult && !boolResult)
                return false; // Skip validation if the condition is false
        }
        return true; // Proceed with validation if no condition or condition is true
    }


    private bool ValidateAndReportError<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, string chainName, string propertyName, TInput input, out TOutput output)
    {
        var result = validator.Validate(_obj, propertyName, input, out output);
        if (!result.IsValid)
        {
            AddError(new ValidationError(validator.ErrorCode, validator.GetType().Name, _formatter.Format(result, input), [.. _parentPath, propertyName], chainName));
            output = default!; // Set output to default value if validation fails
            return false; // Return false to indicate validation failure
        }
        else
            return true; // Return true if validation is successful

    }

    public bool AddValidtor<TInput>(NotNullAttribute validator, string chainName, string propertyName, TInput? input, out TInput output) where TInput : struct
    {
        if (AddValidtor<object?, object?>(validator, chainName, propertyName, input, out var output1))
        {
            output = (TInput)output1!; // Cast to TOutput
            return true;
        }
        output = default!;
        return false; // Return false if validation fails
    }

    public bool AddValidtor<TInput>(NotNullAttribute validator, string chainName, string propertyName, TInput? input, out TInput output) where TInput : class
    {
        if (AddValidtor<object?, object?>(validator, chainName, propertyName, input, out var output1))
        {
            output = (TInput)output1!; // Cast to TOutput
            return true;
        }
        output = default!;
        return false; // Return false if validation fails
    }

    public bool AddValidtor<TInput>(GeneralValidationAttributeBase validator, string chainName, string propertyName, TInput input, out TInput output)
    {
        if (AddValidtor<object?, object?>(validator, chainName, propertyName, input, out var output1))
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


}