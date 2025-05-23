---
id: localization
title: Localization
---

# Localization

All error messages in EasyValidate are localization-ready. Use resource files or custom formatters for multi-language support.

- Error messages use numbered placeholders (e.g., `{0}`) for easy translation.
- You can provide your own `IFormatter` implementation for custom formatting.

Example:

```csharp
public class MyFormatter : IFormatter
{
    public string Format(string message, params object?[] args)
    {
        // Custom localization logic
    }
}
```

See [Extending EasyValidate](extending.md) for more.
