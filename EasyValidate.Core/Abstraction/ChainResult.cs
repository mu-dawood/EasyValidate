using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Core.Abstraction
{
    public sealed class ChainResult(IFormatter? formatter, string chainName, string propertyName) : IChainResult
    {
        private readonly IFormatter? _formatter = formatter;
        private readonly string _chainName = chainName;
        private readonly string _propertyName = propertyName;
        public string PropertyName => _propertyName;
        public string ChainName => _chainName;

        private List<ValidationError>? _errors;

        public IReadOnlyCollection<ValidationError> Errors => _errors?.AsReadOnly() ?? (IReadOnlyCollection<ValidationError>)[];

        public void AddResult<TValidator, TInput>(AttributeResult result, TValidator validator, TInput input) where TValidator : IValidationAttribute
        {
            _errors ??= [];
            if (_formatter == null)
            {
                var formattedMessage = string.Format(result.MessageTemplate, result.MessageArgs ?? []);
                var error = new ValidationError(validator.ErrorCode, validator.GetType().Name, formattedMessage, _propertyName, _chainName);
                _errors.Add(error);
            }
            else
            {
                var formattedMessage = _formatter.Format(result.MessageTemplate, result.MessageArgs ?? []);
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
