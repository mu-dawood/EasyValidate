namespace EasyValidate.Abstractions;

/// <summary>
/// Specifies the access modifier for generated validation methods.
/// </summary>
public enum AccessModifier
{
    /// <summary>
    /// The generated method will be public.
    /// </summary>
    Public,

    /// <summary>
    /// The generated method will be internal.
    /// </summary>
    Internal,

    /// <summary>
    /// The generated method will be protected.
    /// </summary>
    Protected,

    /// <summary>
    /// The generated method will be private.
    /// </summary>
    Private,

    /// <summary>
    /// The generated method will be protected internal.
    /// </summary>
    ProtectedInternal,

    /// <summary>
    /// The generated method will be private protected.
    /// </summary>
    PrivateProtected
}
