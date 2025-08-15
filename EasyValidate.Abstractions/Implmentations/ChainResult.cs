namespace EasyValidate.Abstractions
{
    /// <inheritdoc cref="IChainResult"/>
    public sealed class ChainResult(IFormatter? formatter, string chainName, string propertyName) : IChainResult
    {
        private readonly IFormatter? _formatter = formatter;
        private readonly string _chainName = chainName;
        private readonly string _propertyName = propertyName;
        internal string PropertyName => _propertyName;

        /// <inheritdoc/>
        public string ChainName => _chainName;

        private List<ValidationError>? _errors;

        /// <inheritdoc/>
        public IReadOnlyCollection<ValidationError> Errors => _errors?.AsReadOnly() ?? (IReadOnlyCollection<ValidationError>)[];
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool HasErrors()
        {
            return _errors != null && _errors.Count > 0;
        }
        /// <inheritdoc/>
        public bool IsValid() => !HasErrors();

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, Errors.Select(error => error.FormattedMessage));
        }
    }

}
