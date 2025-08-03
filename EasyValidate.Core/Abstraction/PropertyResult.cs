using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyValidate.Core.Abstraction
{
    public sealed class PropertyResult(ValidationConfig? config, string propertyName) : IPropertyResult
    {
        private readonly string _propertyName = propertyName;
        private readonly ValidationConfig? _config = config;

        public string PropertyName => _propertyName;

        private List<IChainResult>? _results;

        public IReadOnlyCollection<IChainResult> Results => _results?.AsReadOnly() ?? (IReadOnlyCollection<IChainResult>)Array.Empty<IChainResult>();
        private List<IValidationResult>? _nestedResults;
        public IReadOnlyCollection<IValidationResult> NestedResults => _nestedResults?.AsReadOnly() ?? (IReadOnlyCollection<IValidationResult>)Array.Empty<IValidationResult>();

        bool _hasErrors = false;
        bool _hasNestedErrors = false;
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


        public bool HasErrors() => _hasErrors || _hasNestedErrors;
        public bool HasNestedErrors() => _hasNestedErrors;

        public bool HasErrors(string chainName)
        {
            if (_results == null) return false;
            return Chain(chainName)?.HasErrors() ?? false;
        }

        public bool IsValid() => !HasErrors();

        public bool IsValid(string chainName) => !HasErrors(chainName);





        public void AddChainResult(IChainResult result)
        {
            if (result.IsValid()) return;
            _results ??= [];
            _results.Add(result);
            _hasErrors = true;
        }

        public void AddNestedResult<T>(T other) where T : IValidate
        {
            var result = other.Validate(_config);
            if (result.IsValid()) return;
            _nestedResults ??= [];
            _nestedResults.Add(result);
            _hasNestedErrors = true;
            _hasErrors = true;
        }

        public async Task AddNestedResultAsync<T>(T other) where T : IAsyncValidate
        {
            var result = await other.ValidateAsync(_config);
            if (result.IsValid()) return;
            _nestedResults ??= [];
            _nestedResults.Add(result);
            _hasNestedErrors = true;
            _hasErrors = true;
        }

        public void AddNestedResult<T>(IEnumerable<T> collection) where T : IValidate
        {
            foreach (var item in collection)
            {
                AddNestedResult(item);
            }
        }

        public async Task AddNestedResultAsync<T>(IEnumerable<T> collection) where T : IAsyncValidate
        {
            foreach (var item in collection)
            {
                await AddNestedResultAsync(item);
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Results.Select(chain => chain.ToString()));
        }

    }


}
