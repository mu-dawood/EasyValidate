using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.EmailAddress;

public partial class EmailAddressModel
 : IValidate
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Optional, EmailAddress]
    public string? OptionalEmail { get; set; }
}

public partial class EmailAddressNestedModel
 : IValidate
{
    [EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;

    public EmailAddressModel? UserEmails { get; set; }
}
