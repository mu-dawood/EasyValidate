using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Core.Abstraction
{
    public readonly struct ValidationChain
    {
        internal ValidationChain(ValidationResult result, object obj, string chainName, string propertyName)
        {
            _rersult = result;
            _obj = obj;
            _chainName = chainName;
            _propertyName = propertyName;
        }
        private readonly string _chainName;
        internal readonly string ChainName => _chainName;
        private readonly string _propertyName;
        internal readonly string PropertyName => _propertyName;
        private readonly object _obj;
        internal readonly object Obj => _obj;
        private readonly ValidationResult _rersult;

        public readonly bool AddValidtor<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, TInput input, out TOutput output)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (TryInvokeConditional(validator))
                        return ValidateAndReportError(validator, input, out output);
                    output = default!; // Set output to default value if validation is skipped
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (TryInvokeConditional(validator))
                        ValidateAndReportError(validator, input, out output);
                    else
                        output = default!; // Set output to default value if validation is skipped
                    return true; // Skip validation but continue the chain
                case ExecutionStrategy.ValidateAndStop:
                    return ValidateAndReportError(validator, input, out output);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    if (ValidateAndReportError(validator, input, out output))
                        return true; // Validation passed, continue the chain
                    return true; // Validation failed, but continue the chain
                case ExecutionStrategy.SkipErrorAndStop:
                    var result = validator.Validate(_obj, _propertyName, input, out output);
                    if (!result.IsValid)
                        return false;
                    else
                        return true;
                default:
                    output = default!; // Set output to default value if validation is skipped
                    return false; // Skip validation if no valid strategy is defined
            }
        }


        private readonly bool ValidateAndReportError<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator, TInput input, out TOutput output)
        {
            var result = validator.Validate(_obj, _propertyName, input, out output);
            if (!result.IsValid)
            {
                _rersult.AddResult(result, validator.GetType().Name, validator.ErrorCode, input, _propertyName, _chainName);
                output = default!; // Set output to default value if validation fails
                return false; // Return false to indicate validation failure
            }
            else
                return true; // Return true if validation is successful

        }


        private readonly bool TryInvokeConditional<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator)
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




        public readonly bool AddValidtor<TInput>(IValidationAttribute<TInput> validator, TInput input)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (TryInvokeConditional(validator))
                        return ValidateAndReportError(validator, input);
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (TryInvokeConditional(validator))
                        ValidateAndReportError(validator, input);
                    return true; // Skip validation but continue the chain
                case ExecutionStrategy.ValidateAndStop:
                    return ValidateAndReportError(validator, input);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    if (ValidateAndReportError(validator, input))
                        return true; // Validation passed, continue the chain
                    return true; // Validation failed, but continue the chain
                case ExecutionStrategy.SkipErrorAndStop:
                    var result = validator.Validate(_obj, _propertyName, input);
                    if (!result.IsValid)
                        return false;
                    else
                        return true;
                default:
                    return false; // Skip validation if no valid strategy is defined
            }
        }


        private readonly bool ValidateAndReportError<TInput>(IValidationAttribute<TInput> validator, TInput input)
        {
            var result = validator.Validate(_obj, _propertyName, input);
            if (!result.IsValid)
            {
                _rersult.AddResult(result, validator.GetType().Name, validator.ErrorCode, input, _propertyName, _chainName);
                return false; // Return false to indicate validation failure
            }
            else
                return true; // Return true if validation is successful

        }


        private readonly bool TryInvokeConditional<TInput>(IValidationAttribute<TInput> validator)
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


        public readonly bool AddValidtor<TInput>(NotNullAttribute validator, TInput? input, out TInput output) where TInput : class
        {
            if (AddValidtor(validator, input))
            {
                output = input!; // Cast to TOutput
                return true;
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public readonly bool AddValidtor<TInput>(NotNullAttribute validator, TInput? input, out TInput output) where TInput : struct
        {
            if (AddValidtor(validator, input))
            {
                output = input!.Value; // Cast to TOutput
                return true;
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public readonly bool AddValidtor<TInput>(OptionalAttribute validator, TInput? input, out TInput output) where TInput : struct
        {
            if (AddValidtor(validator, input))
            {
                output = input ?? default!; // Cast to TOutput
                return input.HasValue; // Return true if input has value, false otherwise
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public readonly bool AddValidtor<TInput>(OptionalAttribute validator, TInput? input, out TInput output) where TInput : class
        {
            if (AddValidtor(validator, input))
            {
                output = input ?? default!; // Cast to TOutput
                return input != null; // Return true if input is not null, false otherwise
            }
            output = default!;
            return false; // Return false if validation fails
        }

        public readonly bool AddValidtor<TElemnt>(IValidationAttribute<IEnumerable> validator, IEnumerable<TElemnt> input)
        {

            if (AddValidtor<IEnumerable>(validator, input))
            {
                return input != null; // Return true if input is not null, false otherwise
            }
            return false; // Return false if validation fails
        }


    }


}
