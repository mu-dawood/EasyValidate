using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.EmailAddress;

public partial class EmailAddressModel
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Optional, EmailAddress]
    public string? OptionalEmail { get; set; }
}

public partial class EmailAddressNestedModel
{
    [EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;

    public EmailAddressModel? UserEmails { get; set; }
}
