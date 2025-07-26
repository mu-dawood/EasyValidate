using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Core.Abstraction
{
    public sealed class ValidationResult : IValidationResult
    {

        private List<IPropertyResult>? _results;
        private bool _hasErrors = false;

        public IReadOnlyList<IPropertyResult> Results
        {
            get
            {
                _results ??= [];
                return _results.AsReadOnly();
            }
        }

        public bool HasErrors() => _hasErrors;

        public bool HasErrors(string propertyName) => _results != null && _results.Any((a) => a.PropertyName == propertyName && a.HasErrors());

        public bool IsValid() => !HasErrors();

        public bool IsValid(string propertyName) => !HasErrors(propertyName);

        public IPropertyResult? Property(string propertyName) => _results?.FirstOrDefault((f) => f.PropertyName == propertyName);

        public void AddPropertyResult(IPropertyResult result)
        {
            _results ??= [];
            if (!_hasErrors)
                _hasErrors = result.HasErrors();
            _results.Add(result);
        }
    }


}
