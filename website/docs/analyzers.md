---
id: analyzers
title: Source Generators & Analyzers
sidebar_position: 6
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid } from '@site/src/components/DocsComponents';

<DocsWrapper>

# Source Generators & Analyzers

EasyValidate leverages the power of Roslyn source generators and analyzers to provide compile-time validation code generation, IDE support, and zero-runtime reflection. This results in high-performance validation with excellent developer experience.

## Overview

The EasyValidate.Analyzers package provides:

- **Source Generation**: Automatic creation of validation methods at compile time
- **Compile-time Analysis**: Early detection of attribute usage errors
- **IDE Integration**: IntelliSense, tooltips, and quick fixes
- **Performance Optimization**: Zero reflection, direct method calls
- **Type Safety**: Compile-time validation of attribute usage

## Source Generation

### How It Works

When you apply validation attributes to your properties, the source generator automatically creates validation methods:

```csharp
// Your code
public class User
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Range(18, 99)]
    public int Age { get; set; }
}
```

The source generator produces code similar to:

```csharp
// Generated code (partial class)
public partial class User : IValidate
{
    public ValidationResult Validate()
    {
        return Validate(ValidationResult.GetDefaultFormatter());
    }

    public ValidationResult Validate(IFormatter formatter)
    {
        var result = new ValidationResult(formatter);
        
        // Email validation
        result.TryAddError(nameof(Email), new NotEmptyAttribute(), 
            (v) => v.Validate(nameof(Email), Email));
        result.TryAddError(nameof(Email), new EmailAddressAttribute(), 
            (v) => v.Validate(nameof(Email), Email));
        
        // Age validation
        result.TryAddError(nameof(Age), new RangeAttribute(18, 99), 
            (v) => v.ValidateNumber(nameof(Age), Age));
        
        return result;
    }
}
```

### Generated Methods

The source generator creates these methods automatically:

#### `ValidationResult Validate()`
Validates the object using the default formatter.

#### `ValidationResult Validate(IFormatter formatter)`
Validates the object using a custom formatter for localization or custom error formatting.

### Nested Object Validation

The generator handles nested objects and collections automatically:

```csharp
public class Order
{
    [NotEmpty]
    public string OrderNumber { get; set; }

    // Nested object - automatically validated
    public Customer Customer { get; set; }

    // Collection of validated objects
    [NotEmpty<OrderItem>]
    public List<OrderItem> Items { get; set; }
}

// Generated validation includes:
// 1. Direct property validation (OrderNumber)
// 2. Nested object validation (Customer.Validate())
// 3. Collection validation and item validation
```

## Analyzers

### Validation Method Analyzer (VAL001)

Ensures that custom validation attributes have properly implemented validation methods.

#### What It Checks

- **Method Signature**: Validates that `Validate` methods have exactly two parameters
- **Parameter Types**: Ensures first parameter is `string` (property name)
- **Return Type**: Verifies methods return `AttributeResult`
- **Base Class Inheritance**: Confirms proper inheritance from validation base classes

#### Example Violations

```csharp
// ❌ Incorrect - missing second parameter
public class BadAttribute : StringValidationAttributeBase
{
    public override AttributeResult Validate(string propertyName) // VAL001
    {
        return new AttributeResult { IsValid = true };
    }
}

// ❌ Incorrect - wrong return type
public class BadAttribute : StringValidationAttributeBase
{
    public override bool Validate(string propertyName, string? value) // VAL001
    {
        return true;
    }
}

// ✅ Correct
public class GoodAttribute : StringValidationAttributeBase
{
    public override AttributeResult Validate(string propertyName, string? value)
    {
        return new AttributeResult { IsValid = true };
    }
}
```

### Attribute Usage Analyzer

Validates that validation attributes are used correctly on appropriate property types.

#### What It Checks

- **Type Compatibility**: Ensures numeric attributes are applied to numeric properties
- **Collection Attributes**: Verifies collection attributes are used on collection types
- **Date Attributes**: Confirms date attributes are applied to DateTime properties
- **Null Handling**: Validates nullable type usage

#### Example Violations

```csharp
public class Product
{
    [Range(1, 100)] // ❌ Error: Range attribute on string property
    public string Name { get; set; }

    [NotEmpty] // ❌ Error: String attribute on numeric property
    public int Price { get; set; }

    [PastDate] // ❌ Error: Date attribute on string property
    public string CreatedDate { get; set; }
}
```

## IDE Integration

### IntelliSense Support

The analyzers provide rich IntelliSense support:

- **Attribute Suggestions**: Auto-complete for validation attributes
- **Parameter Help**: Tooltips showing attribute parameters
- **Usage Examples**: Documentation and examples in hover text

### Quick Fixes

When the analyzer detects issues, it provides quick fixes:

- **Fix Method Signatures**: Automatically correct validation method signatures
- **Add Missing Methods**: Generate required validation methods
- **Fix Return Types**: Convert incorrect return types to `AttributeResult`

### Error Highlighting

Real-time error highlighting for:

- Incorrect attribute usage
- Invalid method signatures
- Type mismatches
- Missing implementations

## Performance Benefits

### Zero Reflection

Traditional validation frameworks use reflection at runtime, which has performance costs:

```csharp
// Traditional approach (slow)
foreach (var property in type.GetProperties())
{
    var attributes = property.GetCustomAttributes<ValidationAttribute>();
    foreach (var attr in attributes)
    {
        var result = attr.IsValid(property.GetValue(obj));
        // Process result...
    }
}
```

EasyValidate generates direct method calls:

```csharp
// EasyValidate approach (fast)
result.TryAddError(nameof(Email), new EmailAddressAttribute(), 
    (v) => v.Validate(nameof(Email), Email));
```

### Compile-time Optimization

The generator can optimize validation code:

- **Inlining**: Simple validations can be inlined
- **Early Returns**: Skip unnecessary validations
- **Type-specific Code**: Generate optimized code for each type

### Memory Efficiency

- **No Reflection Metadata**: Reduced memory footprint
- **Minimal Allocations**: Efficient object creation
- **Cached Instances**: Reuse attribute instances where possible

## Configuration

### MSBuild Properties

Control source generation behavior through MSBuild properties:

```xml
<PropertyGroup>
  <!-- Enable/disable source generation -->
  <EasyValidateGenerateCode>true</EasyValidateGenerateCode>
  
  <!-- Control analyzer severity -->
  <WarningLevel>4</WarningLevel>
  
  <!-- Treat analyzer warnings as errors -->
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  
  <!-- Suppress specific analyzer rules -->
  <NoWarn>VAL001</NoWarn>
</PropertyGroup>
```

### EditorConfig Integration

Configure analyzer behavior in `.editorconfig`:

```ini
[*.cs]
# Configure EasyValidate analyzers
dotnet_diagnostic.VAL001.severity = error

# Disable for generated files
[{**/Generated/**,**/*.g.cs}]
dotnet_diagnostic.VAL001.severity = none
```

## Troubleshooting

### Common Issues

#### Generator Not Running

**Problem**: Source generation isn't working.

**Solutions**:
1. Ensure `EasyValidate.Analyzers` package is installed
2. Check that your project targets a supported framework (.NET Standard 2.0+)
3. Clean and rebuild the solution
4. Restart the IDE

#### Partial Class Errors

**Problem**: "Type must be declared partial" errors.

**Solution**: Ensure your classes are declared as `partial`:

```csharp
public partial class User // Must be partial
{
    [NotEmpty]
    public string Name { get; set; }
}
```

#### Analyzer Warnings

**Problem**: VAL001 warnings on custom attributes.

**Solution**: Ensure your validation methods have correct signatures:

```csharp
public class CustomAttribute : StringValidationAttributeBase
{
    public override string ErrorCode => "CustomError";
    
    // Correct signature
    public override AttributeResult Validate(string propertyName, string? value)
    {
        // Implementation
    }
}
```

### Debugging Source Generation

#### Enable MSBuild Verbosity

```xml
<PropertyGroup>
  <MSBuildVerbosity>diagnostic</MSBuildVerbosity>
</PropertyGroup>
```

#### Generated Code Location

Generated files are typically located at:
```
obj/Debug/netX.X/Generated/EasyValidate/EasyValidate.EasyValidateGenerator/
```

#### Analyzer Diagnostics

Enable analyzer logging:

```xml
<PropertyGroup>
  <RunAnalyzersDuringBuild>true</RunAnalyzersDuringBuild>
  <ReportAnalyzer>true</ReportAnalyzer>
</PropertyGroup>
```

## Advanced Configuration

### Custom Generator Attributes

You can control what the generator produces:

```csharp
// Exclude from generation
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class InternalModel
{
    // This class won't get validation generated
}
```

### Conditional Compilation

Use preprocessor directives for conditional validation:

```csharp
public class Model
{
#if DEBUG
    [NotEmpty] // Only validate in debug builds
#endif
    public string DebugProperty { get; set; }
}
```

## Integration with Build Pipelines

### CI/CD Considerations

1. **Build Performance**: Source generation happens at compile time
2. **Deterministic Builds**: Generated code is deterministic across builds
3. **Cache Friendly**: Generated files can be cached between builds
4. **No Runtime Dependencies**: Only compile-time dependency on analyzers

### Package Distribution

When distributing packages that use EasyValidate:

```xml
<PackageReference Include="EasyValidate" Version="1.0.0" />
<PackageReference Include="EasyValidate.Analyzers" Version="1.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

---

**Next:** Learn about [Localization](localization.md) for multi-language error messages.

</DocsWrapper>
