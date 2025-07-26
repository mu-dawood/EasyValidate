using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Core.Abstraction
{
    public sealed class ChainResult(IServiceProvider provider, string propertyName, string chainName) : IChainResult
    {
        private readonly IServiceProvider _provider = provider;
        private readonly string _chainName = chainName;
        private readonly string _propertyName = propertyName;
        private bool _hasErrors = false;
        public IServiceProvider Provider => _provider;
        public string PropertyName => _propertyName;
        public string ChainName => _chainName;

        private List<ValidationError>? _errors;
        public IReadOnlyList<ValidationError> Errors
        {
            get
            {
                _errors ??= [];
                return _errors.AsReadOnly();
            }
        }

        public void AddResult<T>(AttributeResult result, Type type, string errorCode, T input)
        {
            _errors ??= [];
            _hasErrors = true;
            var formatter = _provider?.GetService(typeof(IFormatter));
            if (formatter is not IFormatter _formatter)
            {
                var formattedMessage = string.Format(result.MessageTemplate, result.MessageArgs);
                var error = new ValidationError(errorCode, type.Name, formattedMessage, _propertyName, _chainName);
                _errors.Add(error);
            }
            else
            {
                var formattedMessage = _formatter.Format(result, input);
                var error = new ValidationError(errorCode, type.Name, formattedMessage, _propertyName, _chainName);
                _errors.Add(error);
            }

        }


        public bool HasErrors() => _hasErrors;

        public bool IsValid() => !HasErrors();

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Errors.Select(error => error.FormattedMessage));
        }
    }

}
