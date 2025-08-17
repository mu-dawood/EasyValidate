namespace EasyValidate.Abstractions
{
    /// <inheritdoc cref="IValidationResult"/>
    public class ValidationResult : IValidationResult
    {
        internal ValidationResult(List<IPropertyResult>? results)
        {
            _results = results;
        }
        /// <summary>
        /// Creates a new instance of <see cref="ValidationResult"/> with no results.
        /// </summary>
        /// <returns></returns>
        public static ValidationResult Create() => new(null);

        private List<IPropertyResult>? _results;
        /// <inheritdoc/>
        public IReadOnlyCollection<IPropertyResult> Results => _results?.AsReadOnly() ?? (IReadOnlyCollection<IPropertyResult>)Array.Empty<IPropertyResult>();

        bool _hasErrors = false;
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public bool HasErrors() => _hasErrors;
        /// <inheritdoc/>
        public bool HasErrors(string propertyName)
        {
            if (_results == null) return false;
            return Property(propertyName)?.HasErrors() ?? false;
        }
        /// <inheritdoc/>
        public bool IsValid() => !HasErrors();
        /// <inheritdoc/>
        public bool IsValid(string propertyName) => !HasErrors(propertyName);
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void AddPropertyResult(IPropertyResult result)
        {
            if (result.IsValid()) return;
            _results ??= [];
            _results.Add(result);
            _hasErrors = true;
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            if (_results == null || _results.Count == 0) return "No validation results.";
            return string.Join(Environment.NewLine, _results.Select(prop => prop.ToString()));
        }
        /// <inheritdoc/>
        public async Task AddPropertyResultAsync(ValueTask<IPropertyResult> result)
        {
            _results ??= [];
            _results.Add(await result);
        }
        /// <inheritdoc/>
        public IValidationResult<T> WithResult<T>(T? result)
        {
            return new ValidationResult<T>(_results, result);
        }
    }

    /// <inheritdoc cref="IValidationResult{T}"/>

    public class ValidationResult<T> : ValidationResult, IValidationResult<T>
    {
        /// <inheritdoc/>
        public T? Result { get; private set; }

        internal ValidationResult(List<IPropertyResult>? results, T? result) : base(results)
        {
            Result = result;
        }

    }


}
