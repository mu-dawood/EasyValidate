namespace EasyValidate.Abstractions;

/// <summary>
/// Provides configuration for validation, including management of service provider and formatter instances.
/// </summary>
/// <docs-display-name>Validation Configuration</docs-display-name>
/// <docs-description>Manages dependencies and configuration for validation operations.</docs-description>
/// <remarks>
/// Initializes a new instance of <see cref="ValidationConfig"/> with optional service provider and formatter.
/// </remarks>
/// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for validation. If null, no provider will be set.</param>
/// <param name="formatter">The <see cref="IFormatter"/> to use for validation errors and messages. If null, no formatter will be set.</param>
public class ValidationConfig(IServiceProvider? serviceProvider = null, IFormatter? formatter = null)
{
    private IServiceProvider? _provider = serviceProvider;
    private IFormatter? _formatter = formatter;

    /// <summary>
    /// Gets the current service provider used for validation, or null if not set.
    /// </summary>
    public IServiceProvider? ServiceProvider => _provider;
    /// <summary>
    /// Gets the current formatter used for validation errors and messages, or null if not set.
    /// </summary>
    public IFormatter? Formatter => _formatter;

    /// <summary>
    /// Sets the service provider to be used for validation.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> instance to use.</param>
    public void SetServiceProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Sets the formatter to be used for formatting validation errors and messages.
    /// </summary>
    /// <param name="formatter">The <see cref="IFormatter"/> instance to use.</param>
    public void SetFormatter(IFormatter formatter)
    {
        _formatter = formatter;
    }

}
