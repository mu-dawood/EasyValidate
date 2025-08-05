using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyValidate.Core.Abstraction
{
    public class ValidationResult : IValidationResult
    {
        internal ValidationResult(List<IPropertyResult>? results)
        {
            _results = results;
        }

        public static ValidationResult Create() => new(null);

        private List<IPropertyResult>? _results;

        public IReadOnlyCollection<IPropertyResult> Results => _results?.AsReadOnly() ?? (IReadOnlyCollection<IPropertyResult>)Array.Empty<IPropertyResult>();

        bool _hasErrors = false;
        public int ErrorsCount
        {
            get
            {
                if (_results == null) return 0;
                var count = 0;
                foreach (var prop in _results)
                    foreach (var chain in prop.Results)
                        count += chain.Errors.Count;
                return count;
            }
        }

        public bool HasErrors() => _hasErrors;

        public bool HasErrors(string propertyName)
        {
            if (_results == null) return false;
            return Property(propertyName)?.HasErrors() ?? false;
        }

        public bool IsValid() => !HasErrors();

        public bool IsValid(string propertyName) => !HasErrors(propertyName);

        public IPropertyResult? Property(string propertyName)
        {
            if (_results == null) return null;
            foreach (var item in _results)
            {
                if (item.PropertyName == propertyName)
                    return item;
            }
            return null;
        }

        public void AddPropertyResult(IPropertyResult result)
        {
            if (result.IsValid()) return;
            _results ??= [];
            _results.Add(result);
            _hasErrors = true;
        }
        public override string ToString()
        {
            if (_results == null || _results.Count == 0) return "No validation results.";
            return string.Join(Environment.NewLine, _results.Select(prop => prop.ToString()));
        }

        public async Task AddPropertyResultAsync(ValueTask<IPropertyResult> result)
        {
            _results ??= [];
            _results.Add(await result);
        }

        public IValidationResult<T> WithResult<T>(T? result)
        {
            return new ValidationResult<T>(_results, result);
        }
    }

    public class ValidationResult<T> : ValidationResult, IValidationResult<T>
    {

        public T? Result { get; private set; }

        internal ValidationResult(List<IPropertyResult>? results, T? result) : base(results)
        {
            Result = result;
        }

    }


}
