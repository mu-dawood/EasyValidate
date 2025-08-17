namespace EasyValidate.Abstractions
{
    /// <inheritdoc cref="IPropertyResult"/>

    public sealed class PropertyResult(ValidationConfig? config, string propertyName) : IPropertyResult
    {
        private readonly string _propertyName = propertyName;
        private readonly ValidationConfig? _config = config;

        /// <inheritdoc/>
        public string PropertyName => _propertyName;

        private List<IChainResult>? _results;
        /// <inheritdoc/>

        public IReadOnlyCollection<IChainResult> Results => _results?.AsReadOnly() ?? (IReadOnlyCollection<IChainResult>)Array.Empty<IChainResult>();
        private List<IValidationResult>? _nestedResults;
        /// <inheritdoc/>
        public IReadOnlyCollection<IValidationResult> NestedResults => _nestedResults?.AsReadOnly() ?? (IReadOnlyCollection<IValidationResult>)Array.Empty<IValidationResult>();

        bool _hasErrors = false;
        bool _hasNestedErrors = false;
        /// <inheritdoc/>
        public IChainResult? Chain(string chainName)
        {
            if (_results == null) return null;
            foreach (var item in _results)
            {
                if (item.ChainName == chainName)
                    return item;
            }
            return null;
        }

        /// <inheritdoc/>
        public bool HasErrors() => _hasErrors || _hasNestedErrors;
        /// <inheritdoc/>
        public bool HasNestedErrors() => _hasNestedErrors;
        /// <inheritdoc/>
        public bool HasErrors(string chainName)
        {
            if (_results == null) return false;
            return Chain(chainName)?.HasErrors() ?? false;
        }
        /// <inheritdoc/>
        public bool IsValid() => !HasErrors();
        /// <inheritdoc/>
        public bool IsValid(string chainName) => !HasErrors(chainName);




        /// <inheritdoc/>
        public void AddChainResult(IChainResult result)
        {
            if (result.IsValid()) return;
            _results ??= [];
            _results.Add(result);
            _hasErrors = true;
        }

        /// <inheritdoc/>
        public void AddNestedResult<T>(T other) where T : IValidate
        {
            var result = other.Validate(_config);
            if (result.IsValid()) return;
            _nestedResults ??= [];
            _nestedResults.Add(result);
            _hasNestedErrors = true;
            _hasErrors = true;
        }
        /// <inheritdoc/>
        public async Task AddNestedResultAsync<T>(T other) where T : IAsyncValidate
        {
            var result = await other.ValidateAsync(_config);
            if (result.IsValid()) return;
            _nestedResults ??= [];
            _nestedResults.Add(result);
            _hasNestedErrors = true;
            _hasErrors = true;
        }
        /// <inheritdoc/>
        public void AddNestedResult<T>(IEnumerable<T> collection) where T : IValidate
        {
            foreach (var item in collection)
            {
                AddNestedResult(item);
            }
        }
        /// <inheritdoc/>
        public async Task AddNestedResultAsync<T>(IEnumerable<T> collection) where T : IAsyncValidate
        {
            foreach (var item in collection)
            {
                await AddNestedResultAsync(item);
            }
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, Results.Select(chain => chain.ToString()));
        }

    }


}
