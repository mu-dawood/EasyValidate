using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Core.Abstraction
{
    public sealed class ChainResult(IServiceProvider provider, string chainName, string propertyName) : IChainResult
    {
        private readonly IServiceProvider _provider = provider;
        private readonly string _chainName = chainName;
        private readonly string _propertyName = propertyName;
        public IServiceProvider Provider => _provider;
        public string PropertyName => _propertyName;
        public string ChainName => _chainName;

        private List<ValidationError>? _errors;
       
        public IReadOnlyCollection<ValidationError> Errors => _errors?.AsReadOnly() ?? (IReadOnlyCollection<ValidationError>)Array.Empty<ValidationError>();

        public void AddResult<TValidator, TInput>(AttributeResult result, TValidator validator, TInput input) where TValidator : IValidationAttribute
        {
            _errors ??= [];
            var formatter = _provider?.GetService(typeof(IFormatter));
            if (formatter is not IFormatter _formatter)
            {
                var formattedMessage = string.Format(result.MessageTemplate, result.MessageArgs);
                var error = new ValidationError(validator.ErrorCode, validator.GetType().Name, formattedMessage, _propertyName, _chainName);
                _errors.Add(error);
            }
            else
            {
                var formattedMessage = _formatter.Format(result, input);
                var error = new ValidationError(validator.ErrorCode, validator.GetType().Name, formattedMessage, _propertyName, _chainName);
                _errors.Add(error);
            }

        }


        public bool HasErrors()
        {
            return _errors != null && _errors.Count > 0;
        }

        public bool IsValid() => !HasErrors();

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Errors.Select(error => error.FormattedMessage));
        }
    }

}
