---
id: attributes
title: Attributes Reference
---

# Attributes Reference

## String Attributes
- `NotEmpty`, `MinLength`, `MaxLength`, `ExactLength`, `StartsWith`, `EndsWith`, `Contains`, `NotContains`, `Matches`, `Alpha`, `AlphaNumeric`, `Lowercase`, `Uppercase`, `EmailAddress`, `Phone`, `Url`, `CreditCard`, `Guid`, `Ascii`, `DisallowWhitespace`, `Hex`, `IpAddress`, `BaseEncoding`, `PrintableAscii`, `CommonPrintable`

## Numeric Attributes
- `Positive`, `Negative`, `NonZero`, `EvenNumber`, `OddNumber`, `Range`, `GreaterThan`, `GreaterThanOrEqualTo`, `LessThan`, `LessThanOrEqualTo`, `MultipleOf`, `Prime`, `MaxDigits`, `MinDigits`

## Date Attributes
- `PastDate`, `FutureDate`, `Today`, `NotInFuture`, `NotInPast`, `Weekend`, `Weekday`, `Month`, `Year`, `DateRange`, `DayOfWeek`

## Collection Attributes
- `NotEmpty`, `MinLength`, `MaxLength`, `Length`, `Contains`, `NotContain`, `Unique`, `Single`, `SingleOrNone`

See [Extending EasyValidate](extending.md) to create your own attributes.
