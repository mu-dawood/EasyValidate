using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Extensions;

public static class ValidationResultExtensions
{
    /// <summary>
    /// Gets all validation errors from the hierarchical validation result structure
    /// </summary>
    /// <param name="result">The validation result</param>
    /// <returns>All validation errors flattened from the hierarchical structure</returns>
    public static IEnumerable<ValidationError> GetAllErrors(this IValidationResult result)
    {
        var errors = result.Results
            .SelectMany(property => property.Results)
            .SelectMany(chain => chain.Errors);
        var nestedErrors = result.Results
        .SelectMany(property => property.NestedResults)
        .SelectMany(nested => nested.Results)
        .SelectMany(chain => chain.Results)
        .SelectMany(chain => chain.Errors);
        return errors.Concat(nestedErrors);
    }

    /// <summary>
    /// Gets all validation errors that contain the specified property name in their path
    /// </summary>
    /// <param name="result">The validation result</param>
    /// <param name="propertyName">The property name to search for</param>
    /// <returns>Validation errors that contain the property name</returns>
    public static IEnumerable<ValidationError> GetErrorsForProperty(this IValidationResult result, string propertyName)
    {
        return result.GetAllErrors()
            .Where(error => error.PropertyName?.Contains(propertyName) == true);
    }
}
