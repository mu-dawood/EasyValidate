---
id: localization
title: Localization & Internationalization
sidebar_position: 6
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid } from '@site/src/components/DocsComponents';

<DocsWrapper>

# Localization & Internationalization

EasyValidate is designed from the ground up to support multiple languages and cultures. All error messages use numbered placeholders and the `IFormatter` interface, making localization straightforward and flexible.

## Overview

### Key Features

- **Numbered Placeholders**: All error messages use `{0}`, `{1}`, etc. for easy translation
- **IFormatter Interface**: Flexible message formatting system
- **Resource File Support**: Integration with .NET resource files
- **Culture-Aware Formatting**: Support for culture-specific number and date formatting
- **Extensible Design**: Easy to implement custom localization strategies

### Built-in Error Codes

Each validation attribute has a unique error code that can be mapped to localized messages:

```csharp
// Error codes for common attributes
"NotEmptyValidationError"
"EmailAddressValidationError" 
"RangeValidationError"
"MinLengthValidationError"
"MaxLengthValidationError"
"PastDateValidationError"
// ... and many more
```

## Implementing Localization

### Basic Custom Formatter

Create a simple formatter with hardcoded translations:

```csharp
using System.Collections.Generic;
using EasyValidate.Abstraction;

public class SimpleLocalizedFormatter : IFormatter
{
    private readonly Dictionary<string, Dictionary<string, string>> _messages;
    private readonly string _language;

    public SimpleLocalizedFormatter(string language = "en")
    {
        _language = language;
        _messages = InitializeMessages();
    }

    public string Format(string message, object?[] args)
    {
        // Extract error code from args (first argument is typically the error code)
        var errorCode = args.Length > 0 && args[0] is string code ? code : null;
        
        // Try to find localized message
        if (errorCode != null && 
            _messages.TryGetValue(_language, out var languageMessages) && 
            languageMessages.TryGetValue(errorCode, out var localizedMessage))
        {
            // Use localized message, skip error code in formatting
            return string.Format(localizedMessage, args.Skip(1).ToArray());
        }

        // Fallback to default message
        return string.Format(message, args);
    }

    private Dictionary<string, Dictionary<string, string>> InitializeMessages()
    {
        return new Dictionary<string, Dictionary<string, string>>
        {
            ["en"] = new Dictionary<string, string>
            {
                ["NotEmptyValidationError"] = "The field {0} cannot be empty.",
                ["EmailAddressValidationError"] = "The field {0} must be a valid email address.",
                ["RangeValidationError"] = "The field {0} must be between {1} and {2}.",
                ["MinLengthValidationError"] = "The field {0} must be at least {1} characters long.",
                ["MaxLengthValidationError"] = "The field {0} must not exceed {1} characters.",
                ["PastDateValidationError"] = "The field {0} must be a past date.",
                ["PhoneValidationError"] = "The field {0} must be a valid phone number."
            },
            ["es"] = new Dictionary<string, string>
            {
                ["NotEmptyValidationError"] = "El campo {0} no puede estar vacío.",
                ["EmailAddressValidationError"] = "El campo {0} debe ser una dirección de correo válida.",
                ["RangeValidationError"] = "El campo {0} debe estar entre {1} y {2}.",
                ["MinLengthValidationError"] = "El campo {0} debe tener al menos {1} caracteres.",
                ["MaxLengthValidationError"] = "El campo {0} no debe exceder {1} caracteres.",
                ["PastDateValidationError"] = "El campo {0} debe ser una fecha pasada.",
                ["PhoneValidationError"] = "El campo {0} debe ser un número de teléfono válido."
            },
            ["fr"] = new Dictionary<string, string>
            {
                ["NotEmptyValidationError"] = "Le champ {0} ne peut pas être vide.",
                ["EmailAddressValidationError"] = "Le champ {0} doit être une adresse e-mail valide.",
                ["RangeValidationError"] = "Le champ {0} doit être entre {1} et {2}.",
                ["MinLengthValidationError"] = "Le champ {0} doit avoir au moins {1} caractères.",
                ["MaxLengthValidationError"] = "Le champ {0} ne doit pas dépasser {1} caractères.",
                ["PastDateValidationError"] = "Le champ {0} doit être une date passée.",
                ["PhoneValidationError"] = "Le champ {0} doit être un numéro de téléphone valide."
            },
            ["de"] = new Dictionary<string, string>
            {
                ["NotEmptyValidationError"] = "Das Feld {0} darf nicht leer sein.",
                ["EmailAddressValidationError"] = "Das Feld {0} muss eine gültige E-Mail-Adresse sein.",
                ["RangeValidationError"] = "Das Feld {0} muss zwischen {1} und {2} liegen.",
                ["MinLengthValidationError"] = "Das Feld {0} muss mindestens {1} Zeichen haben.",
                ["MaxLengthValidationError"] = "Das Feld {0} darf nicht mehr als {1} Zeichen haben.",
                ["PastDateValidationError"] = "Das Feld {0} muss ein vergangenes Datum sein.",
                ["PhoneValidationError"] = "Das Feld {0} muss eine gültige Telefonnummer sein."
            }
        };
    }
}
```

### Resource File-Based Formatter

For production applications, use .NET resource files:

#### 1. Create Resource Files

Create resource files for each language:

- `ValidationMessages.resx` (default/English)
- `ValidationMessages.es.resx` (Spanish)
- `ValidationMessages.fr.resx` (French)
- `ValidationMessages.de.resx` (German)

#### 2. Add Error Messages to Resource Files

In `ValidationMessages.resx`:
```
NotEmptyValidationError = The field {0} cannot be empty.
EmailAddressValidationError = The field {0} must be a valid email address.
RangeValidationError = The field {0} must be between {1} and {2}.
MinLengthValidationError = The field {0} must be at least {1} characters long.
```

#### 3. Implement Resource-Based Formatter

```csharp
using System.Globalization;
using System.Resources;
using EasyValidate.Abstraction;

public class ResourceBasedFormatter : IFormatter
{
    private readonly ResourceManager _resourceManager;
    private readonly CultureInfo _culture;

    public ResourceBasedFormatter(ResourceManager resourceManager, CultureInfo? culture = null)
    {
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        _culture = culture ?? CultureInfo.CurrentCulture;
    }

    public string Format(string message, object?[] args)
    {
        // Try to get localized message from resources
        if (args.Length > 0 && args[0] is string errorCode)
        {
            try
            {
                var localizedMessage = _resourceManager.GetString(errorCode, _culture);
                if (!string.IsNullOrEmpty(localizedMessage))
                {
                    // Format with culture-specific formatting
                    return string.Format(_culture, localizedMessage, args.Skip(1).ToArray());
                }
            }
            catch (MissingManifestResourceException)
            {
                // Resource not found, fall back to default
            }
        }

        // Fallback to default message with culture-specific formatting
        return string.Format(_culture, message, args);
    }
}
```

### Database-Based Formatter

For dynamic localization, store translations in a database:

```csharp
public class DatabaseFormatter : IFormatter
{
    private readonly ILocalizationService _localizationService;
    private readonly string _language;
    private readonly CultureInfo _culture;

    public DatabaseFormatter(ILocalizationService localizationService, string language)
    {
        _localizationService = localizationService;
        _language = language;
        _culture = CultureInfo.GetCultureInfo(language);
    }

    public string Format(string message, object?[] args)
    {
        if (args.Length > 0 && args[0] is string errorCode)
        {
            var localizedMessage = _localizationService.GetTranslation(errorCode, _language);
            if (!string.IsNullOrEmpty(localizedMessage))
            {
                return string.Format(_culture, localizedMessage, args.Skip(1).ToArray());
            }
        }

        return string.Format(_culture, message, args);
    }
}

public interface ILocalizationService
{
    string? GetTranslation(string key, string language);
}
```

## Advanced Localization Features

### Culture-Aware Formatting

The formatter handles culture-specific formatting for numbers, dates, and currencies:

```csharp
public class CultureAwareFormatter : IFormatter
{
    private readonly CultureInfo _culture;

    public CultureAwareFormatter(CultureInfo culture)
    {
        _culture = culture;
    }

    public string Format(string message, object?[] args)
    {
        // Format arguments according to culture
        var formattedArgs = args.Select(FormatArgument).ToArray();
        return string.Format(_culture, message, formattedArgs);
    }

    private object FormatArgument(object? arg)
    {
        return arg switch
        {
            DateTime date => date.ToString("d", _culture), // Short date pattern
            decimal number => number.ToString("N", _culture), // Number with culture-specific separators
            double number => number.ToString("N", _culture),
            float number => number.ToString("N", _culture),
            _ => arg
        };
    }
}
```

### Property Name Localization

Localize property names displayed in error messages:

```csharp
public class PropertyNameLocalizedFormatter : IFormatter
{
    private readonly ResourceManager _propertyNameResources;
    private readonly ResourceManager _messageResources;
    private readonly CultureInfo _culture;

    public PropertyNameLocalizedFormatter(
        ResourceManager propertyNameResources,
        ResourceManager messageResources,
        CultureInfo culture)
    {
        _propertyNameResources = propertyNameResources;
        _messageResources = messageResources;
        _culture = culture;
    }

    public string Format(string message, object?[] args)
    {
        if (args.Length == 0) return message;

        var errorCode = args[0] as string;
        var propertyName = args.Length > 1 ? args[1] as string : null;

        // Localize property name
        if (!string.IsNullOrEmpty(propertyName))
        {
            var localizedPropertyName = _propertyNameResources.GetString(propertyName, _culture);
            if (!string.IsNullOrEmpty(localizedPropertyName))
            {
                args[1] = localizedPropertyName;
            }
        }

        // Get localized message
        if (!string.IsNullOrEmpty(errorCode))
        {
            var localizedMessage = _messageResources.GetString(errorCode, _culture);
            if (!string.IsNullOrEmpty(localizedMessage))
            {
                return string.Format(_culture, localizedMessage, args.Skip(1).ToArray());
            }
        }

        return string.Format(_culture, message, args);
    }
}
```

## Usage Examples

### Basic Usage

```csharp
public class ValidationService
{
    public ValidationResult ValidateWithLocalization(IValidate model, string language = "en")
    {
        var formatter = new SimpleLocalizedFormatter(language);
        return model.Validate(formatter);
    }
}

// Usage
var user = new User { Email = "invalid", Age = 15 };
var result = validationService.ValidateWithLocalization(user, "es");

foreach (var error in result.Errors)
{
    Console.WriteLine($"{error.Key}:");
    foreach (var validationError in error.Value)
    {
        Console.WriteLine($"  • {validationError.FormattedMessage}");
    }
}

// Output in Spanish:
// Email:
//   • El campo Email debe ser una dirección de correo válida.
// Age:
//   • El campo Age debe estar entre 18 y 99.
```

### ASP.NET Core Integration

```csharp
public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get language from Accept-Language header or query parameter
        var language = GetLanguageFromRequest(context);
        var culture = CultureInfo.GetCultureInfo(language);
        
        // Set current culture
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }

    private string GetLanguageFromRequest(HttpContext context)
    {
        // Check query parameter first
        if (context.Request.Query.TryGetValue("lang", out var langParam))
        {
            return langParam.ToString();
        }

        // Check Accept-Language header
        var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var firstLanguage = acceptLanguage.Split(',').FirstOrDefault()?.Split(';').FirstOrDefault();
            if (!string.IsNullOrEmpty(firstLanguage))
            {
                return firstLanguage.Trim();
            }
        }

        return "en"; // Default to English
    }
}

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        // Create formatter based on current culture
        var formatter = new ResourceBasedFormatter(
            ValidationMessages.ResourceManager, 
            CultureInfo.CurrentCulture);

        var result = user.Validate(formatter);
        
        if (!result.IsValid())
        {
            var errors = result.Errors.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(e => e.FormattedMessage).ToArray()
            );
            
            return BadRequest(new { Errors = errors });
        }

        return Ok();
    }
}
```

### Dependency Injection Setup

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register localization services
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        
        // Register custom formatter
        services.AddScoped<IFormatter>(provider =>
        {
            var culture = CultureInfo.CurrentCulture;
            return new ResourceBasedFormatter(ValidationMessages.ResourceManager, culture);
        });

        // Configure supported cultures
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es"),
                new CultureInfo("fr"),
                new CultureInfo("de")
            };

            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Add localization middleware
        app.UseRequestLocalization();
        
        // Other middleware...
    }
}
```

## Best Practices

1. **Use Resource Files**: For production applications, use .NET resource files
2. **Culture-Aware Formatting**: Always format numbers, dates, and currencies according to culture
3. **Fallback Strategy**: Always provide fallback to default messages
4. **Property Name Localization**: Consider localizing property names too
5. **Testing**: Test with multiple cultures and edge cases
6. **Performance**: Cache translated messages when possible
7. **Consistency**: Use the same localization strategy throughout your application

## Common Error Codes Reference

Here's a comprehensive list of built-in error codes you can localize:

### String Validation
- `NotEmptyValidationError`
- `MinLengthValidationError`
- `MaxLengthValidationError`
- `ExactLengthValidationError`
- `StartsWithValidationError`
- `EndsWithValidationError`
- `ContainsValidationError`
- `NotContainsValidationError`
- `MatchesValidationError`
- `AlphaValidationError`
- `AlphaNumericValidationError`
- `LowercaseValidationError`
- `UppercaseValidationError`
- `EmailAddressValidationError`
- `PhoneValidationError`
- `UrlValidationError`
- `CreditCardValidationError`
- `GuidValidationError`
- `AsciiValidationError`
- `HexValidationError`
- `IpAddressValidationError`

### Numeric Validation
- `PositiveValidationError`
- `NegativeValidationError`
- `NonZeroValidationError`
- `EvenNumberValidationError`
- `OddNumberValidationError`
- `RangeValidationError`
- `GreaterThanValidationError`
- `GreaterThanOrEqualToValidationError`
- `LessThanValidationError`
- `LessThanOrEqualToValidationError`
- `MultipleOfValidationError`
- `PrimeValidationError`
- `MaxDigitsValidationError`
- `MinDigitsValidationError`

### Date Validation
- `PastDateValidationError`
- `FutureDateValidationError`
- `TodayValidationError`
- `NotInFutureValidationError`
- `NotInPastValidationError`
- `WeekendValidationError`
- `WeekdayValidationError`
- `MonthValidationError`
- `YearValidationError`
- `DateRangeValidationError`
- `DayOfWeekValidationError`

### Collection Validation
- `NotEmptyValidationError` (collections)
- `MinLengthValidationError` (collections)
- `MaxLengthValidationError` (collections)
- `LengthValidationError` (collections)
- `ContainsValidationError` (collections)
- `NotContainValidationError` (collections)
- `UniqueValidationError`
- `SingleValidationError`
- `SingleOrNoneValidationError`

---

**Next:** Learn about [Contributing](contributing.md) to the EasyValidate project.

</DocsWrapper>
