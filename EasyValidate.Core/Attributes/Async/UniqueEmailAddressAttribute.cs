using System;
using System.Threading.Tasks;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid email address.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [EmailAddress]
    ///     public string Email { get; set; } // Valid: "user@example.com", "test.email@domain.org", Invalid: "invalid-email", "user@"
    ///     
    ///     [EmailAddress]
    ///     public string ContactEmail { get; set; } // Valid: "john.doe@company.com", Invalid: "john.doe", "@company.com"
    /// }
    /// </code>
    /// </example>





    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public partial class UniqueEmailAddressAsyncAttribute() : Attribute, IAsyncValidationAttribute<string>
    {
        public string ErrorCode { get; set; } = "UniqueEmailAddressValidationError";
        public string? ConditionalMethod { get; set; }
        public string Chain { get; set; } = string.Empty;
        public IServiceProvider? ServiceProvider { get; set; }

        public Task<AttributeResult> ValidateAsync(string propertyName, string value)
        {
            throw new NotImplementedException();
        }
    }


}