using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyValidate.Core.Abstraction
{

    public static class ValidationAttributeExtensions
    {
        

        /// <summary>
        /// Synchronously validates an input against a validation attribute with custom output.
        /// </summary>
        public static (bool isValid, TOutput? output) IsValid<TInput, TOutput>(this IValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ValidateAndStop:
                    return validator.Validate(input, chain);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    return (true, validator.Validate(input, chain).output);
                case ExecutionStrategy.SkipErrorAndStop:
                default:
                    var result = validator.Validate(chain.Provider, chain.PropertyName, input);
                    if (!result.IsValid)
                        return (false, result.Output);
                    else
                        return (true, result.Output);
            }
        }

        public static (bool isValid, TOutput? output) IsValid<TInput, TOutput>(this IValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input, Func<ChainResult, bool> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (conditionalMethod.Invoke(chain))
                        return validator.Validate(input, chain);
                    return (false, default);
                case ExecutionStrategy.ConditionalAndContinue:
                    if (conditionalMethod.Invoke(chain))
                        return (true, validator.Validate(input, chain).output);
                    return (true, input is TOutput ? (TOutput)(object)input : default);
                default:
                    return IsValid(validator, chain, input);
            }
        }

        public static async Task<(bool isValid, TOutput? output)> IsValid<TInput, TOutput>(this IValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input, Func<ChainResult, ValueTask<bool>> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (await conditionalMethod.Invoke(chain))
                        return validator.Validate(input, chain);
                    return (false, default);
                case ExecutionStrategy.ConditionalAndContinue:
                    if (await conditionalMethod.Invoke(chain))
                        return (true, validator.Validate(input, chain).output);
                    return (true, input is TOutput ? (TOutput)(object)input : default);
                default:
                    return IsValid(validator, chain, input);
            }
        }

        /// <summary>
        /// Asynchronously validates an input against an async validation attribute with custom output.
        /// </summary>
        public static async Task<(bool isValid, TOutput? output)> IsValid<TInput, TOutput>(this IAsyncValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ValidateAndStop:
                    return await ValidateAsync(validator, input, chain);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    return (true, (await ValidateAsync(validator, input, chain)).output);
                case ExecutionStrategy.SkipErrorAndStop:
                default:
                    var result = await validator.ValidateAsync(chain.Provider, chain.PropertyName, input);
                    if (!result.IsValid)
                        return (false, result.Output);
                    else
                        return (true, result.Output);
            }
        }

        public static async Task<(bool isValid, TOutput? output)> IsValid<TInput, TOutput>(this IAsyncValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input, Func<ChainResult, ValueTask<bool>> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (await conditionalMethod.Invoke(chain))
                        return await ValidateAsync(validator, input, chain);
                    return (false, default);
                case ExecutionStrategy.ConditionalAndContinue:
                    if (await conditionalMethod.Invoke(chain))
                        return (true, (await ValidateAsync(validator, input, chain)).output);
                    return (true, input is TOutput ? (TOutput)(object)input : default);
                default:
                    return await ValidateAsync(validator, input, chain);
            }
        }


        public static async Task<(bool isValid, TOutput? output)> IsValid<TInput, TOutput>(this IAsyncValidationAttribute<TInput, TOutput> validator, ChainResult chain, TInput input, Func<ChainResult, bool> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (conditionalMethod.Invoke(chain))
                        return await ValidateAsync(validator, input, chain);
                    return (false, default);
                case ExecutionStrategy.ConditionalAndContinue:
                    if (conditionalMethod.Invoke(chain))
                        return (true, (await ValidateAsync(validator, input, chain)).output);
                    return (true, input is TOutput ? (TOutput)(object)input : default);
                default:
                    return await ValidateAsync(validator, input, chain);
            }
        }

        // Private helpers for TInput, TOutput
        private static (bool isValid, TOutput? output) Validate<TInput, TOutput>(this IValidationAttribute<TInput, TOutput> validator, TInput input, ChainResult validationResult)
        {
            var result = validator.Validate(validationResult.Provider, validationResult.PropertyName, input);
            if (!result.IsValid)
            {
                validationResult.AddResult(result, validator.GetType(), validator.ErrorCode, input);
                return (false, default);
            }
            else
                return (true, result.Output);
        }

        private static async Task<(bool isValid, TOutput? output)> ValidateAsync<TInput, TOutput>(this IAsyncValidationAttribute<TInput, TOutput> validator, TInput input, ChainResult validationResult)
        {
            var result = await validator.ValidateAsync(validationResult.Provider, validationResult.PropertyName, input);
            if (!result.IsValid)
            {
                validationResult.AddResult(result, validator.GetType(), validator.ErrorCode, input);
                return (false, default);
            }
            else
                return (true, result.Output);
        }


        /// <summary>
        /// Synchronously validates an input against a validation attribute.
        /// </summary>
        public static bool IsValid<TInput>(this IValidationAttribute<TInput> validator, ChainResult chain, TInput input)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ValidateAndStop:
                    return validator.Validate(input, chain);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    if (validator.Validate(input, chain))
                        return true; // Validation passed, continue the chain
                    return true; // Validation failed, but continue the chain
                case ExecutionStrategy.SkipErrorAndStop:
                default:
                    var result = validator.Validate(chain.Provider, chain.PropertyName, input);
                    if (!result.IsValid)
                        return false;
                    else
                        return true;
            }
        }

        public static bool IsValid<TInput>(this IValidationAttribute<TInput> validator, ChainResult chain, TInput input, Func<ChainResult, bool> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (conditionalMethod.Invoke(chain))
                        return validator.Validate(input, chain);
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (conditionalMethod.Invoke(chain))
                        validator.Validate(input, chain);
                    return true; // Skip validation but continue the chain
                default:
                    return IsValid(validator, chain, input); // Fallback to default validation
            }
        }

        public static async Task<bool> IsValid<TInput>(this IValidationAttribute<TInput> validator, ChainResult chain, TInput input, Func<ChainResult, ValueTask<bool>> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (await conditionalMethod.Invoke(chain))
                        return validator.Validate(input, chain);
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (await conditionalMethod.Invoke(chain))
                        validator.Validate(input, chain);
                    return true; // Skip validation but continue the chain
                default:
                    return IsValid(validator, chain, input); // Fallback to default validation
            }
        }

        /// <summary>
        /// Asynchronously validates an input against a validation attribute.
        /// </summary>
        public static async Task<bool> IsValid<TInput>(this IAsyncValidationAttribute<TInput> validator, ChainResult chain, TInput input)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ValidateAndStop:
                    return await validator.ValidateAsync(input, chain);
                case ExecutionStrategy.ValidateErrorAndContinue:
                    if (await validator.ValidateAsync(input, chain))
                        return true; // Validation passed, continue the chain
                    return true; // Validation failed, but continue the chain
                case ExecutionStrategy.SkipErrorAndStop:
                default:
                    var result = await validator.ValidateAsync(chain.Provider, chain.PropertyName, input);
                    if (!result.IsValid)
                        return false;
                    else
                        return true;
            }
        }
        public static async Task<bool> IsValid<TInput>(this IAsyncValidationAttribute<TInput> validator, ChainResult chain, TInput input, Func<ChainResult, bool> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (conditionalMethod.Invoke(chain))
                        return await validator.ValidateAsync(input, chain);
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (conditionalMethod.Invoke(chain))
                        await validator.ValidateAsync(input, chain);
                    return true; // Skip validation but continue the chain
                default:
                    return await IsValid(validator, chain, input); // Fallback to default validation
            }
        }

        public static async Task<bool> IsValid<TInput>(this IAsyncValidationAttribute<TInput> validator, ChainResult chain, TInput input, Func<ChainResult, ValueTask<bool>> conditionalMethod)
        {
            switch (validator.Strategy)
            {
                case ExecutionStrategy.ConditionalAndStopChain:
                    if (await conditionalMethod.Invoke(chain))
                        return await validator.ValidateAsync(input, chain);
                    return false; // Skip validation if conditional method returns false
                case ExecutionStrategy.ConditionalAndContinue:
                    if (await conditionalMethod.Invoke(chain))
                        await validator.ValidateAsync(input, chain);
                    return true; // Skip validation but continue the chain

                default:
                    return await IsValid(validator, chain, input); // Fallback to default validation
            }
        }

        private static bool Validate<TInput>(this IValidationAttribute<TInput> validator, TInput input, ChainResult validationResult)
        {
            var result = validator.Validate(validationResult.Provider, validationResult.PropertyName, input);
            if (!result.IsValid)
            {
                validationResult.AddResult(result, validator.GetType(), validator.ErrorCode, input);
                return false; // Return false to indicate validation failure
            }
            else
                return true; // Return true if validation is successful
        }

        private static async Task<bool> ValidateAsync<TInput>(this IAsyncValidationAttribute<TInput> validator, TInput input, ChainResult validationResult)
        {
            var result = await validator.ValidateAsync(validationResult.Provider, validationResult.PropertyName, input);
            if (!result.IsValid)
            {
                validationResult.AddResult(result, validator.GetType(), validator.ErrorCode, input);
                return false; // Return false to indicate validation failure
            }
            else
                return true; // Return true if validation is successful
        }


    }


}
