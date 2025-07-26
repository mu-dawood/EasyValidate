using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyValidate.Core.Abstraction
{
    public sealed class PropertyResult(IServiceProvider provider, string propertyName) : IPropertyResult
    {
        private readonly IServiceProvider _provider = provider;
        private readonly string _propertyName = propertyName;
        private bool _hasErrors = false;
        private bool _hasNestedErrors = false;

        public string PropertyName => _propertyName;

        private List<IChainResult>? _results;

        public IReadOnlyList<IChainResult> Results
        {
            get
            {
                _results ??= [];
                return _results.AsReadOnly();
            }
        }
        private List<IValidationResult>? _nestedResults;
        public IReadOnlyList<IValidationResult> NestedResults
        {
            get
            {
                _nestedResults ??= [];
                return _nestedResults.AsReadOnly();
            }
        }

        public IChainResult? Chain(string chainName) => _results?.FirstOrDefault((f) => f.ChainName == chainName);


        public bool HasErrors() => _hasErrors || _hasNestedErrors;
        public bool HasNestedErrors() => _hasNestedErrors;


        public bool HasErrors(string chainName) => _results != null && _results.Any((a) => a.ChainName == chainName && a.HasErrors());

        public bool IsValid() => !HasErrors();

        public bool IsValid(string chainName) => !HasErrors(chainName);

        public void AddNestedResult(IValidate other)
        {
            var result = other.Validate(_provider);
            _nestedResults ??= [];
            if (!_hasNestedErrors)
                _hasNestedErrors = result.HasErrors();
            _nestedResults.Add(result);
        }
        public async Task AddNestedResultAsync(IAsyncValidate other)
        {
            var result = await other.ValidateAsync(_provider);
            _nestedResults ??= [];
            if (!_hasNestedErrors)
                _hasNestedErrors = result.HasErrors();
            _nestedResults.Add(result);
        }

        public void AddNestedResult(IEnumerable collection)
        {
            foreach (var item in collection)
            {
                if (item is IValidate validateItem)
                {
                    AddNestedResult(validateItem);
                }
                if (item is IAsyncValidate validateItemAsync)
                {
                    AddNestedResultAsync(validateItemAsync).GetAwaiter().GetResult();
                }
                else if (item is IEnumerable enumerableItem)
                {
                    AddNestedResult(enumerableItem);
                }
            }
        }

        public void AddChainResult(IChainResult result)
        {
            _results ??= [];
            if (!_hasErrors)
                _hasErrors = result.HasErrors();
            _results.Add(result);
        }


    }


}
