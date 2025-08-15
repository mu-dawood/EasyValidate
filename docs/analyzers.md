---
title: EasyValidate Analyzers
layout: default
---

# Code Analyzers

EasyValidate includes Roslyn code analyzers that provide compile-time validation and feedback to help catch issues early in the development process. These analyzers ensure proper usage of validation attributes and methods.

## Overview

The analyzer package provides static code analysis rules that help catch validation issues at compile time. These rules are designed to:

- Ensure proper implementation of custom validation attributes
- Validate correct usage of validation attributes
- Provide immediate feedback in the IDE
- Prevent common validation configuration errors

## Analyzer Rules

### VAL001: Invalid Validate method in derived class

**Category:** Design  
**Severity:** Error  
**Source:** ValidationAttributeAnalyzer.cs

**Description:**  
This rule ensures that classes deriving from validation attributes properly implement the required `Validate` method signature.

**Message:**  
Class '{0}' must have at least one 'Validate(string, Type)' method with exactly two parameters, and all 'Validate' methods must have exactly two parameters. Ensure all 'Validate' methods return 'EasyValidate.Abstractions.AttributeResult'.

**Example of violation:**
```csharp
public class CustomValidationAttribute : StringValidationAttributeBase
{
    // ❌ Wrong signature - missing parameters
    public override AttributeResult Validate()
    {
        return new AttributeResult();
    }
}
```

**Correct implementation:**
```csharp
public class CustomValidationAttribute : StringValidationAttributeBase
{
    // ✅ Correct signature
    public override AttributeResult Validate(string value, Type propertyType)
    {
        return new AttributeResult();
    }
}
```

### VAL002: Invalid Attribute Usage

**Category:** Usage  
**Severity:** Error  
**Source:** IsCompatibleWithValidateMethods.cs

**Description:**  
This rule validates that validation attributes are applied to compatible targets and follow expected usage patterns.

**Message:**  
The attribute '{0}' is used incorrectly. Ensure it is applied to the correct target and follows the expected usage.

**Example of violation:**
```csharp
public class Example
{
    // ❌ Wrong usage - applying string validation to numeric property
    [EmailAddress]
    public int Number { get; set; }
}
```

**Correct implementation:**
```csharp
public class Example
{
    // ✅ Correct usage - string validation on string property
    [EmailAddress]
    public string Email { get; set; }
    
    // ✅ Correct usage - numeric validation on numeric property
    [Range(0, 100)]
    public int Number { get; set; }
}
```

### VAL004: Invalid PowerOfAttribute Usage

**Category:** Usage  
**Severity:** Error  
**Source:** PowerOfAttributeUsage.cs

**Description:**  
This rule ensures that the `PowerOfAttribute` is used with valid values (greater than 1).

**Message:**  
The value passed to 'PowerOfAttribute' must be greater than 1

**Example of violation:**
```csharp
public class Example
{
    // ❌ Invalid - power must be greater than 1
    [PowerOf(1)]
    public int Value { get; set; }
    
    // ❌ Invalid - power must be greater than 1
    [PowerOf(0)]
    public int AnotherValue { get; set; }
}
```

**Correct implementation:**
```csharp
public class Example
{
    // ✅ Valid - power of 2
    [PowerOf(2)]
    public int Value { get; set; }
    
    // ✅ Valid - power of 3
    [PowerOf(3)]
    public int CubicValue { get; set; }
}
```

### VAL005: Invalid DivisibleByAttribute Usage

**Category:** Usage  
**Severity:** Error  
**Source:** DivisibleByAttributeUsage.cs

**Description:**  
This rule ensures that the `DivisibleByAttribute` is not used with zero as the divisor.

**Message:**  
The divisor passed to 'DivisibleByAttribute' cannot be zero

**Example of violation:**
```csharp
public class Example
{
    // ❌ Invalid - cannot divide by zero
    [DivisibleBy(0)]
    public int Value { get; set; }
}
```

**Correct implementation:**
```csharp
public class Example
{
    // ✅ Valid - divisible by 2 (even numbers)
    [DivisibleBy(2)]
    public int EvenValue { get; set; }
    
    // ✅ Valid - divisible by 5
    [DivisibleBy(5)]
    public int MultipleOfFive { get; set; }
}
```

### VAL010: Invalid Collection Element Attribute Usage

**Category:** Usage  
**Severity:** Error  
**Source:** CollectionElementAttributeUsage.cs

**Description:**  
This rule ensures that collection element validation attributes are applied to properties with compatible collection types.

**Message:**  
The element passed to '{0}' must match the property type

**Example of violation:**
```csharp
public class Example
{
    // ❌ Invalid - string validation on numeric collection
    [Contains("text")]
    public List<int> Numbers { get; set; }
}
```

**Correct implementation:**
```csharp
public class Example
{
    // ✅ Valid - string validation on string collection
    [Contains("text")]
    public List<string> Texts { get; set; }
    
    // ✅ Valid - numeric validation on numeric collection
    [Contains(42)]
    public List<int> Numbers { get; set; }
}
```

## IDE Integration

The analyzers integrate seamlessly with popular IDEs:

### Visual Studio
- Real-time error highlighting
- Quick fixes and suggestions
- Error list integration
- IntelliSense support

### Visual Studio Code
- Error squiggles in editor
- Problems panel integration
- Hover tooltips with rule details

### JetBrains Rider
- Inspection highlighting
- Solution-wide analysis
- Quick-fix suggestions

## Configuration

You can configure analyzer behavior in your project file:

```xml
<PropertyGroup>
  <!-- Disable specific analyzer rules -->
  <NoWarn>$(NoWarn);VAL004</NoWarn>
  
  <!-- Treat analyzers as warnings instead of errors -->
  <WarningsAsErrors />
  <WarningsNotAsErrors>VAL001;VAL002</WarningsNotAsErrors>
</PropertyGroup>
```

## EditorConfig Support

Configure analyzer severity using EditorConfig:

```ini
# .editorconfig
[*.cs]
# Set specific rule severity
dotnet_diagnostic.VAL001.severity = error
dotnet_diagnostic.VAL002.severity = warning
dotnet_diagnostic.VAL004.severity = suggestion
```

## Suppression

Suppress analyzer warnings when needed:

```csharp
public class Example
{
    // Suppress specific rule for this property
#pragma warning disable VAL004
    [PowerOf(1)] // Suppressed for this specific case
    public int SpecialValue { get; set; }
#pragma warning restore VAL004
}
```

## Troubleshooting

### Analyzers Not Running

1. Ensure the analyzer package is properly referenced:
   ```xml
   <PackageReference Include="EasyValidate.Analyzers" Version="1.0.0">
     <PrivateAssets>all</PrivateAssets>
     <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
   </PackageReference>
   ```

2. Clean and rebuild the solution
3. Restart the IDE
4. Check the Error List / Problems panel for analyzer messages

### Performance Issues

If analyzers are causing performance issues:

1. Disable specific rules that are not needed
2. Use solution-wide analysis sparingly
3. Configure analyzer execution in larger solutions

---

For more information about extending EasyValidate with custom analyzers, see the [Extending EasyValidate](index.md#extending-easyvalidate) section.
