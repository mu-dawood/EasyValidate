---
id: extending
title: Extending EasyValidate
sidebar_position: 5
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid } from '@site/src/components/DocsComponents';

<DocsWrapper>

# Extending EasyValidate

<DocSection 
  title="Extend EasyValidate to Fit Your Needs" 
  subtitle="EasyValidate is designed to be extensible. You can create custom validation attributes by inheriting from the appropriate base classes, implement custom formatters for localization, and even create specialized validation logic."
  icon="🔧"
  background="gradient"
>

<FeatureGrid>
  <FeatureCard
    icon="🏗️"
    title="Custom Attributes"
    description="Create specialized validation logic for your domain"
    color="primary"
  />
  <FeatureCard
    icon="🌐"
    title="Custom Formatters"
    description="Implement localization and custom error messages"
    color="secondary"
  />
  <FeatureCard
    icon="⚡"
    title="Source Generation"
    description="Leverage compile-time code generation"
    color="accent"
  />
</FeatureGrid>

</DocSection>

## Creating Custom Validation Attributes

### Base Classes Overview

EasyValidate provides specialized base classes for different data types:

- **`StringValidationAttributeBase`** - For string validation
- **`NumericValidationAttributeBase`** - For numeric validation (supports all numeric types)
- **`DateValidationAttributeBase`** - For DateTime validation
- **`CollectionValidationAttributeBase<T>`** - For collection validation
- **`ValidationAttributeBase`** - For general object validation

### String Validation Attributes

Create custom string validators by inheriting from `StringValidationAttributeBase`:

```csharp
using System;
using EasyValidate.Abstraction;
using EasyValidate.Attributes;

/// <summary>
/// Validates that a string contains only valid SQL identifier characters.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SqlIdentifierAttribute : StringValidationAttributeBase
{
    public override string ErrorCode => "SqlIdentifierValidationError";

    public override AttributeResult Validate(string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} cannot be empty.",
                MessageArgs = [propertyName]
            };
        }

        if (!IsValidSqlIdentifier(value!))
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must be a valid SQL identifier.",
                MessageArgs = [propertyName]
            };
        }

        return new AttributeResult { IsValid = true };
    }

    private static bool IsValidSqlIdentifier(string value)
    {
        if (value.Length == 0) return false;
        
        // First character must be letter or underscore
        if (!char.IsLetter(value[0]) && value[0] != '_') return false;
        
        // Remaining characters must be letters, digits, or underscores
        for (int i = 1; i < value.Length; i++)
        {
            char c = value[i];
            if (!char.IsLetterOrDigit(c) && c != '_') return false;
        }
        
        return true;
    }
}
```

### Numeric Validation Attributes

Create custom numeric validators by inheriting from `NumericValidationAttributeBase`:

```csharp
using System;
using EasyValidate.Abstraction;
using EasyValidate.Attributes;

/// <summary>
/// Validates that a number is a perfect square.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PerfectSquareAttribute : NumericValidationAttributeBase
{
    public override string ErrorCode => "PerfectSquareValidationError";

    public override AttributeResult ValidateNumber(string propertyName, decimal value)
    {
        if (value < 0)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must be a non-negative number to be a perfect square.",
                MessageArgs = [propertyName]
            };
        }

        double sqrt = Math.Sqrt((double)value);
        bool isPerfectSquare = Math.Abs(sqrt - Math.Floor(sqrt)) < double.Epsilon;

        if (!isPerfectSquare)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must be a perfect square.",
                MessageArgs = [propertyName]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

### Date Validation Attributes

Create custom date validators by inheriting from `DateValidationAttributeBase`:

```csharp
using System;
using EasyValidate.Abstraction;
using EasyValidate.Attributes;

/// <summary>
/// Validates that a date falls within business hours.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class BusinessHoursAttribute : DateValidationAttributeBase
{
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }

    public BusinessHoursAttribute(string startTime = "09:00", string endTime = "17:00")
    {
        StartTime = TimeSpan.Parse(startTime);
        EndTime = TimeSpan.Parse(endTime);
    }

    public override string ErrorCode => "BusinessHoursValidationError";

    public override AttributeResult Validate(string propertyName, DateTime value)
    {
        var timeOfDay = value.TimeOfDay;
        
        if (timeOfDay < StartTime || timeOfDay > EndTime)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must be within business hours ({1} - {2}).",
                MessageArgs = [propertyName, StartTime.ToString(@"hh\:mm"), EndTime.ToString(@"hh\:mm")]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

### Collection Validation Attributes

Create custom collection validators by inheriting from `CollectionValidationAttributeBase<T>`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;
using EasyValidate.Attributes;

/// <summary>
/// Validates that a collection contains no duplicate values based on a key selector.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NoDuplicatesByAttribute<T, TKey> : CollectionValidationAttributeBase<T>
{
    private readonly Func<T, TKey> _keySelector;

    public NoDuplicatesByAttribute(Func<T, TKey> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }

    public override string ErrorCode => "NoDuplicatesByValidationError";

    protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
    {
        var items = value.ToList();
        var keys = items.Select(_keySelector).ToList();
        var uniqueKeys = keys.Distinct().ToList();

        if (keys.Count != uniqueKeys.Count)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must not contain duplicate items.",
                MessageArgs = [propertyName]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

### General Object Validation

For complex validation that doesn't fit other categories, inherit from `ValidationAttributeBase`:

```csharp
using System;
using EasyValidate.Abstraction;
using EasyValidate.Attributes;

/// <summary>
/// Validates that an object is not null.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class RequiredObjectAttribute : ValidationAttributeBase
{
    public override string ErrorCode => "RequiredObjectValidationError";

    public AttributeResult Validate(string propertyName, object? value)
    {
        if (value == null)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} is required.",
                MessageArgs = [propertyName]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

## Creating Custom Formatters

### Basic Custom Formatter

Implement `IFormatter` to customize error message formatting:

```csharp
using System.Collections.Generic;
using EasyValidate.Abstraction;

public class LocalizedFormatter : IFormatter
{
    private readonly Dictionary<string, string> _errorMessages;
    private readonly string _language;

    public LocalizedFormatter(string language = "en")
    {
        _language = language;
        _errorMessages = LoadErrorMessages(language);
    }

    public string Format(string message, object?[] args)
    {
        // Try to find localized message first
        if (args.Length > 0 && args[0] is string errorCode)
        {
            var key = $"{_language}.{errorCode}";
            if (_errorMessages.TryGetValue(key, out var localizedMessage))
            {
                return string.Format(localizedMessage, args.Skip(1).ToArray());
            }
        }

        // Fallback to default formatting
        return string.Format(message, args);
    }

    private Dictionary<string, string> LoadErrorMessages(string language)
    {
        // Load from resource files, database, or configuration
        return language switch
        {
            "es" => new Dictionary<string, string>
            {
                { "es.NotEmptyValidationError", "El campo {0} no puede estar vacío." },
                { "es.EmailAddressValidationError", "El campo {0} debe ser una dirección de correo válida." },
                { "es.RangeValidationError", "El campo {0} debe estar entre {1} y {2}." }
            },
            "fr" => new Dictionary<string, string>
            {
                { "fr.NotEmptyValidationError", "Le champ {0} ne peut pas être vide." },
                { "fr.EmailAddressValidationError", "Le champ {0} doit être une adresse e-mail valide." },
                { "fr.RangeValidationError", "Le champ {0} doit être entre {1} et {2}." }
            },
            _ => new Dictionary<string, string>() // Default/English
        };
    }
}
```

### Resource-Based Formatter

For larger applications, consider using resource files:

```csharp
using System.Globalization;
using System.Resources;
using EasyValidate.Abstraction;

public class ResourceFormatter : IFormatter
{
    private readonly ResourceManager _resourceManager;
    private readonly CultureInfo _culture;

    public ResourceFormatter(ResourceManager resourceManager, CultureInfo? culture = null)
    {
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        _culture = culture ?? CultureInfo.CurrentCulture;
    }

    public string Format(string message, object?[] args)
    {
        if (args.Length > 0 && args[0] is string errorCode)
        {
            var localizedMessage = _resourceManager.GetString(errorCode, _culture);
            if (!string.IsNullOrEmpty(localizedMessage))
            {
                return string.Format(_culture, localizedMessage, args.Skip(1).ToArray());
            }
        }

        return string.Format(_culture, message, args);
    }
}
```

## Advanced Patterns

### Parameterized Attributes

Create attributes that accept configuration parameters:

```csharp
/// <summary>
/// Validates string length with configurable minimum and maximum.
/// </summary>
public class FlexibleLengthAttribute : StringValidationAttributeBase
{
    public int? MinLength { get; }
    public int? MaxLength { get; }
    public bool AllowNull { get; }

    public FlexibleLengthAttribute(int? minLength = null, int? maxLength = null, bool allowNull = false)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        AllowNull = allowNull;
    }

    public override string ErrorCode => "FlexibleLengthValidationError";

    public override AttributeResult Validate(string propertyName, string? value)
    {
        if (value == null)
        {
            if (AllowNull)
            {
                return new AttributeResult { IsValid = true };
            }
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} cannot be null.",
                MessageArgs = [propertyName]
            };
        }

        var length = value.Length;

        if (MinLength.HasValue && length < MinLength.Value)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must have at least {1} characters.",
                MessageArgs = [propertyName, MinLength.Value]
            };
        }

        if (MaxLength.HasValue && length > MaxLength.Value)
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must have at most {1} characters.",
                MessageArgs = [propertyName, MaxLength.Value]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

### Composite Validation

Create attributes that combine multiple validation rules:

```csharp
/// <summary>
/// Validates password strength with multiple criteria.
/// </summary>
public class PasswordStrengthAttribute : StringValidationAttributeBase
{
    public int MinLength { get; }
    public bool RequireUppercase { get; }
    public bool RequireLowercase { get; }
    public bool RequireDigits { get; }
    public bool RequireSpecialChars { get; }

    public PasswordStrengthAttribute(
        int minLength = 8,
        bool requireUppercase = true,
        bool requireLowercase = true,
        bool requireDigits = true,
        bool requireSpecialChars = true)
    {
        MinLength = minLength;
        RequireUppercase = requireUppercase;
        RequireLowercase = requireLowercase;
        RequireDigits = requireDigits;
        RequireSpecialChars = requireSpecialChars;
    }

    public override string ErrorCode => "PasswordStrengthValidationError";

    public override AttributeResult Validate(string propertyName, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} is required.",
                MessageArgs = [propertyName]
            };
        }

        var errors = new List<string>();

        if (value.Length < MinLength)
            errors.Add($"at least {MinLength} characters");

        if (RequireUppercase && !value.Any(char.IsUpper))
            errors.Add("at least one uppercase letter");

        if (RequireLowercase && !value.Any(char.IsLower))
            errors.Add("at least one lowercase letter");

        if (RequireDigits && !value.Any(char.IsDigit))
            errors.Add("at least one digit");

        if (RequireSpecialChars && !value.Any(c => !char.IsLetterOrDigit(c)))
            errors.Add("at least one special character");

        if (errors.Any())
        {
            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must contain {1}.",
                MessageArgs = [propertyName, string.Join(", ", errors)]
            };
        }

        return new AttributeResult { IsValid = true };
    }
}
```

## Usage Examples

### Using Custom Attributes

```csharp
public class DatabaseEntity
{
    [SqlIdentifier]
    public string TableName { get; set; }

    [PerfectSquare]
    public int SquareValue { get; set; }

    [BusinessHours("08:00", "18:00")]
    public DateTime CreatedAt { get; set; }

    [FlexibleLength(minLength: 5, maxLength: 50, allowNull: false)]
    public string Description { get; set; }

    [PasswordStrength(minLength: 12, requireSpecialChars: true)]
    public string Password { get; set; }
}
```

### Using Custom Formatters

```csharp
public class ValidationService
{
    public ValidationResult ValidateWithLocalization(object model, string language = "en")
    {
        var formatter = new LocalizedFormatter(language);
        
        if (model is IValidate validatable)
        {
            return validatable.Validate(formatter);
        }

        throw new ArgumentException("Model must implement IValidate", nameof(model));
    }
}

// Usage
var user = new User { Email = "invalid-email", Age = 15 };
var result = validationService.ValidateWithLocalization(user, "es");
// Error messages will be in Spanish
```

## Best Practices for Custom Attributes

1. **Follow Naming Conventions**: End attribute names with "Attribute"
2. **Use Descriptive Error Codes**: Make error codes unique and descriptive
3. **Support Null Values**: Handle null values gracefully
4. **Use Numbered Placeholders**: Use {0}, {1}, etc. for localization
5. **Provide Clear Messages**: Write helpful, actionable error messages
6. **Document Parameters**: Use XML documentation for attribute parameters
7. **Consider Performance**: Avoid expensive operations in validation logic
8. **Unit Test Thoroughly**: Test all edge cases and scenarios

## Integration with Source Generators

Your custom attributes will work automatically with EasyValidate's source generators. The analyzer will:

- Validate that your custom attributes inherit from the correct base classes
- Ensure proper method signatures
- Generate appropriate validation code
- Provide IDE support and IntelliSense

---

**Next:** Learn about [Source Generators & Analyzers](analyzers.md) for compile-time validation and optimal performance.

</DocsWrapper>
