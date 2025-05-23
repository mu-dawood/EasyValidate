---
id: extending
title: Extending EasyValidate
---

# Extending EasyValidate

You can create your own validation attributes by inheriting from the appropriate base class:

- `StringValidationAttributeBase`
- `NumericValidationAttributeBase`
- `DateValidationAttributeBase`
- `CollectionValidationAttributeBase<T>`

Override the `Validate` or `ValidateNumber` method to implement your logic. All error messages should use numbered placeholders for localization.

Example:

```csharp
public class CustomAttribute : StringValidationAttributeBase
{
    public override string ErrorCode => "CustomValidationError";
    public override AttributeResult Validate(string propertyName, string? value)
    {
        // Custom logic
    }
}
```
