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
    public partial class UniqueEmailAddressAsyncAttribute : Attribute, IAsyncValidationAttribute<string>
    {
        public static readonly Lazy<UniqueEmailAddressAsyncAttribute> Instance = new(() => new UniqueEmailAddressAsyncAttribute());

        /// <inheritdoc/>
        public string ErrorCode { get; set; } = "UniqueEmailAddressValidationError";

        /// <inheritdoc/>
        public string? ConditionalMethod { get; set; }

        /// <inheritdoc/>
        public ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        public string Chain { get; set; } = string.Empty;


        public Task<AttributeResult> ValidateAsync(IServiceProvider serviceProvider, string propertyName, string value)
        {
            throw new NotImplementedException();
        }
    }
    
}