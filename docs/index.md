---
title: EasyValidate
layout: default
---

# EasyValidate

![EasyValidate Logo](logo.png)

A modern, fast, and extensible attribute-based validation library for .NET, featuring source generators, analyzers, and a rich set of ready-to-use validation attributes for strings, numbers, dates, and collections.

## Features

- ⚡ Attribute-based validation for properties and fields
- 🧩 Extensible: add your own custom attributes
- 🏷️ Rich built-in attribute set for strings, numbers, dates, and collections
- 🛡️ Source generators for zero-reflection, high-performance validation
- 🕵️‍♂️ Roslyn analyzers for compile-time safety
- 🌍 Localization-ready error messages
- 📦 NuGet-ready, OSS-friendly

## Quick Start

```csharp
public class UserDto
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Range(18, 99)]
    public int Age { get; set; }

    [MaxLength(20)]
    public string Username { get; set; }
}
```

## Installation

Install from NuGet:

```shell
Install-Package EasyValidate
```

Or with .NET CLI:

```shell
dotnet add package EasyValidate
```

## Documentation

- [Getting Started](#quick-start)
- [Attributes Reference](#attributes-reference)
- [Extending EasyValidate](#extending-easyvalidate)
- [Source Generators & Analyzers](#source-generators--analyzers)
- [Localization](#localization)
- [Contributing](#contributing)

## Attributes Reference

### String Attributes
- `NotEmpty`, `MinLength`, `MaxLength`, `ExactLength`, `StartsWith`, `EndsWith`, `Contains`, `NotContains`, `Matches`, `Alpha`, `AlphaNumeric`, `Lowercase`, `Uppercase`, `EmailAddress`, `Phone`, `Url`, `CreditCard`, `Guid`, `Ascii`, `DisallowWhitespace`, `Hex`, `IpAddress`, `BaseEncoding`, `PrintableAscii`, `CommonPrintable`

### Numeric Attributes
- `Positive`, `Negative`, `NonZero`, `EvenNumber`, `OddNumber`, `Range`, `GreaterThan`, `GreaterThanOrEqualTo`, `LessThan`, `LessThanOrEqualTo`, `MultipleOf`, `Prime`, `MaxDigits`, `MinDigits`

### Date Attributes
- `PastDate`, `FutureDate`, `Today`, `NotInFuture`, `NotInPast`, `Weekend`, `Weekday`, `Month`, `Year`, `DateRange`, `DayOfWeek`

### Collection Attributes
- `NotEmpty`, `MinLength`, `MaxLength`, `Length`, `Contains`, `NotContain`, `Unique`, `Single`, `SingleOrNone`

## Modern UI Demo

<div style="display: flex; gap: 2rem; flex-wrap: wrap;">
  <div style="flex: 1 1 300px; background: #f8f9fa; border-radius: 12px; padding: 2rem; box-shadow: 0 2px 8px #0001;">
    <h3>Attribute Example</h3>
    <pre><code>[EmailAddress]
public string Email { get; set; }</code></pre>
  </div>
  <div style="flex: 1 1 300px; background: #f8f9fa; border-radius: 12px; padding: 2rem; box-shadow: 0 2px 8px #0001;">
    <h3>Analyzer Feedback</h3>
    <ul>
      <li>✔️ Compile-time validation</li>
      <li>✔️ IDE tooltips and quick fixes</li>
      <li>✔️ Zero runtime reflection</li>
    </ul>
  </div>
</div>

## Source Generators & Analyzers

EasyValidate uses Roslyn source generators to create fast, reflection-free validation code at compile time. Analyzers ensure correct attribute usage and provide IDE feedback.

## Extending EasyValidate

You can create your own validation attributes by inheriting from the appropriate base class (e.g., `StringValidationAttributeBase`, `NumericValidationAttributeBase`).

## Localization

All error messages are localization-ready. Use resource files or custom formatters for multi-language support.

## Contributing

Pull requests and issues are welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for details.

---

© 2025 EasyValidate Authors. MIT License.
