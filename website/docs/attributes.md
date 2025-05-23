---
id: attributes
title: Attributes Reference
sidebar_position: 4
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid ,AttributeCard ,InlineSnippet} from '@site/src/components/DocsComponents';

<DocsWrapper>

# Validation Attributes Reference

<DocSection 
  title="Comprehensive Validation Attributes" 
  subtitle="EasyValidate provides a comprehensive set of validation attributes organized by data type. All attributes are designed to be lightweight, localization-ready, and provide clear error messages."
  icon="🏷️"
  background="gradient"
>

<FeatureGrid>
  <FeatureCard
    icon="📝"
    title="String Validation"
    description="Email, URL, phone numbers, and text patterns"
    color="primary"
  />
  <FeatureCard
    icon="🔢"
    title="Numeric Validation"
    description="Ranges, precision, and mathematical constraints"
    color="secondary"
  />
  <FeatureCard
    icon="📅"
    title="Date & Time"
    description="Date ranges, future/past validation, and time constraints"
    color="accent"
  />
  <FeatureCard
    icon="📦"
    title="Collections"
    description="Array sizes, list constraints, and item validation"
    color="success"
  />
</FeatureGrid>

</DocSection>

## String Validation Attributes

### Basic String Validation

#### `NotEmpty`
Validates that a string is not null, empty, or whitespace.

<InlineSnippet language="csharp">
{`
[NotEmpty]
public string Name { get; set; }
`}
</InlineSnippet>

#### `MinLength(int minLength)`
Validates minimum string length.

<InlineSnippet language="csharp">
{`
[MinLength(2)]
public string FirstName { get; set; }
`}
</InlineSnippet>

#### `MaxLength(int maxLength)`
Validates maximum string length.

<InlineSnippet language="csharp">
{`
[MaxLength(50)]
public string LastName  { get; set; }
`}
</InlineSnippet>

#### `ExactLength(int length)`
Validates exact string length.

<InlineSnippet language="csharp">
{`
[ExactLength(10)]
public string ProductCode  { get; set; }
`}
</InlineSnippet>

### String Content Validation

#### `StartsWith(string prefix)`
Validates that string starts with specified prefix.

<InlineSnippet language="csharp">
{`
[StartsWith("USER_")]
public string UserId  { get; set; }
`}
</InlineSnippet>

#### `EndsWith(string suffix)`
Validates that string ends with specified suffix.

<InlineSnippet language="csharp">
{`
[EndsWith(".com")]
public string Domain  { get; set; }
`}
</InlineSnippet>

#### `Contains(string substring)`
Validates that string contains specified substring.
<InlineSnippet language="csharp">
{`
[Contains("@")]
public string EmailAddress  { get; set; }
`}
</InlineSnippet>

#### `NotContains(string substring)`
Validates that string does not contain specified substring.
<InlineSnippet language="csharp">
{`
[NotContains("admin")]
public string Username  { get; set; }
`}
</InlineSnippet>

#### `Matches(string pattern)`
Validates string against regular expression pattern.
<InlineSnippet language="csharp">
{`
[Matches(@"^[A-Z][a-z]+$")]
public string PropertyName  { get; set; }
`}
</InlineSnippet>

### String Format Validation

#### `Alpha`
Validates that string contains only alphabetic characters (A-Z, a-z).
<InlineSnippet language="csharp">
{`
[Alpha]
public string CountryCode  { get; set; }
`}
</InlineSnippet>

#### `AlphaNumeric`
Validates that string contains only alphanumeric characters.
<InlineSnippet language="csharp">
{`
[AlphaNumeric]
public string Token  { get; set; }
`}
</InlineSnippet>

#### `Lowercase`
Validates that string is all lowercase.
<InlineSnippet language="csharp">
{`
[Lowercase]
public string Slug  { get; set; }
`}
</InlineSnippet>

#### `Uppercase`
Validates that string is all uppercase.
<InlineSnippet language="csharp">
{`
[Uppercase]
public string StatusCode  { get; set; }
`}
</InlineSnippet>

#### `Ascii`
Validates that string contains only ASCII characters (0x00-0x7F).
<InlineSnippet language="csharp">
{`
[Ascii]
public string AsciiText  { get; set; }
`}
</InlineSnippet>

#### `DisallowWhitespace`
Validates that string contains no whitespace characters.
<InlineSnippet language="csharp">
{`
[DisallowWhitespace]
public string ApiKey  { get; set; }
`}
</InlineSnippet>

#### `PrintableAscii`
Validates that string contains only printable ASCII characters.
<InlineSnippet language="csharp">
{`
[PrintableAscii]
public string DisplayText  { get; set; }
`}
</InlineSnippet>

#### `CommonPrintable`
Validates that string contains only common printable characters.
<InlineSnippet language="csharp">
{`
[CommonPrintable]
public string SafeText  { get; set; }
`}
</InlineSnippet>

### Specialized String Validation

#### `EmailAddress`
Validates email address format.
<InlineSnippet language="csharp">
{`
[EmailAddress]
public string Email  { get; set; }
`}
</InlineSnippet>

#### `Phone`
Validates phone number format (basic international/US format).
<InlineSnippet language="csharp">
{`
[Phone]
public string PhoneNumber  { get; set; }
`}
</InlineSnippet>

#### `Url`
Validates URL format.
<InlineSnippet language="csharp">
{`
[Url]
public string Website  { get; set; }
`}
</InlineSnippet>

#### `CreditCard`
Validates credit card number using Luhn algorithm.
<InlineSnippet language="csharp">
{`
[CreditCard]
public string CardNumber  { get; set; }
`}
</InlineSnippet>

#### `Guid`
Validates GUID format.
<InlineSnippet language="csharp">
{`
[Guid]
public string Id  { get; set; }
`}
</InlineSnippet>

#### `Hex`
Validates hexadecimal string format.
<InlineSnippet language="csharp">
{`
[Hex]
public string ColorCode  { get; set; }
`}
</InlineSnippet>

#### `IpAddress`
Validates IPv4 or IPv6 address format.
<InlineSnippet language="csharp">
{`
[IpAddress]
public string ServerAddress  { get; set; }
`}
</InlineSnippet>

#### `BaseEncoding(BaseType encodingType)`
Validates Base16, Base32, Base58, Base62, Base64, or Base85 encoding.
<InlineSnippet language="csharp">
{`
[BaseEncoding(BaseEncodingAttribute.BaseType.Base64)]
public string EncodedData  { get; set; }
`}
</InlineSnippet>

## Numeric Validation Attributes

### Sign Validation

#### `Positive`
Validates that number is positive (greater than zero).
<InlineSnippet language="csharp">
{`
[Positive]
public decimal Price  { get; set; }
`}
</InlineSnippet>

#### `Negative`
Validates that number is negative (less than zero).
<InlineSnippet language="csharp">
{`
[Negative]
public int TemperatureChange  { get; set; }
`}
</InlineSnippet>

#### `NonZero`
Validates that number is not zero.
<InlineSnippet language="csharp">
{`
[NonZero]
public int Quantity  { get; set; }
`}
</InlineSnippet>

### Range Validation

#### `Range(double min, double max, RangeBoundary boundary = RangeBoundary.Inclusive)`
Validates numeric range with configurable boundary inclusivity.
<InlineSnippet language="csharp">
{`
[Range(0, 100)] // Inclusive by default
public int Percentage  { get; set; }

[Range(0, 100, RangeBoundary.Exclusive)]
public double Score  { get; set; }
`}
</InlineSnippet>

#### `GreaterThan(double value)`
Validates that number is greater than specified value.
<InlineSnippet language="csharp">
{`
[GreaterThan(0)]
public decimal Amount  { get; set; }
`}
</InlineSnippet>

#### `GreaterThanOrEqualTo(double value)`
Validates that number is greater than or equal to specified value.
<InlineSnippet language="csharp">
{`
[GreaterThanOrEqualTo(18)]
public int Age  { get; set; }
`}
</InlineSnippet>

#### `LessThan(double value)`
Validates that number is less than specified value.
<InlineSnippet language="csharp">
{`
[LessThan(100)]
public int Progress  { get; set; }
`}
</InlineSnippet>

#### `LessThanOrEqualTo(double value)`
Validates that number is less than or equal to specified value.
<InlineSnippet language="csharp">
{`
[LessThanOrEqualTo(5)]
public int Rating  { get; set; }
`}
</InlineSnippet>

### Mathematical Validation

#### `EvenNumber`
Validates that number is even.
<InlineSnippet language="csharp">
{`
[EvenNumber]
public int PairCount  { get; set; }
`}
</InlineSnippet>

#### `OddNumber`
Validates that number is odd.
<InlineSnippet language="csharp">
{`
[OddNumber]
public int UniqueId  { get; set; }
`}
</InlineSnippet>

#### `MultipleOf(int divisor)`
Validates that number is a multiple of specified divisor.
<InlineSnippet language="csharp">
{`
[MultipleOf(5)]
public int Increment  { get; set; }
`}
</InlineSnippet>

#### `Prime`
Validates that number is prime.
<InlineSnippet language="csharp">
{`
[Prime]
public int PrimeNumber  { get; set; }
`}
</InlineSnippet>

### Digit Validation

#### `MaxDigits(int maxDigits)`
Validates maximum number of digits.
<InlineSnippet language="csharp">
{`
[MaxDigits(10)]
public long AccountNumber  { get; set; }
`}
</InlineSnippet>

#### `MinDigits(int minDigits)`
Validates minimum number of digits.
<InlineSnippet language="csharp">
{`
[MinDigits(4)]
public int Pin  { get; set; }
`}
</InlineSnippet>

## Date and Time Validation Attributes

### Date Range Validation

#### `PastDate`
Validates that date is in the past.
<InlineSnippet language="csharp">
{`
[PastDate]
public DateTime BirthDate  { get; set; }
`}
</InlineSnippet>

#### `FutureDate`
Validates that date is in the future.
<InlineSnippet language="csharp">
{`
[FutureDate]
public DateTime ExpirationDate  { get; set; }
`}
</InlineSnippet>

#### `Today`
Validates that date is today.
<InlineSnippet language="csharp">
{`
[Today]
public DateTime ProcessingDate  { get; set; }
`}
</InlineSnippet>

#### `NotInFuture`
Validates that date is today or in the past.
<InlineSnippet language="csharp">
{`
[NotInFuture]
public DateTime CompletedDate  { get; set; }
`}
</InlineSnippet>

#### `NotInPast`
Validates that date is today or in the future.
<InlineSnippet language="csharp">
{`
[NotInPast]
public DateTime ScheduledDate  { get; set; }
`}
</InlineSnippet>

#### `DateRange(string minimum, string maximum)`
Validates that date falls within specified range.
<InlineSnippet language="csharp">
{`
[DateRange("2020-01-01", "2025-12-31")]
public DateTime EventDate  { get; set; }
`}
</InlineSnippet>

### Day of Week Validation

#### `Weekend`
Validates that date falls on weekend (Saturday or Sunday).
<InlineSnippet language="csharp">
{`
[Weekend]
public DateTime LeisureDate  { get; set; }
`}
</InlineSnippet>

#### `Weekday`
Validates that date falls on weekday (Monday through Friday).
<InlineSnippet language="csharp">
{`
[Weekday]
public DateTime BusinessDate  { get; set; }
`}
</InlineSnippet>

#### `DayOfWeek(DayOfWeek dayOfWeek)`
Validates that date falls on specific day of week.
<InlineSnippet language="csharp">
{`
[DayOfWeek(DayOfWeek.Monday)]
public DateTime MeetingDate  { get; set; }
`}
</InlineSnippet>

### Specific Date Components

#### `Month(int month)`
Validates that date falls in specific month (1-12).
<InlineSnippet language="csharp">
{`
[Month(12)]
public DateTime ChristmasEvent  { get; set; }
`}
</InlineSnippet>

#### `Year(int year)`
Validates that date falls in specific year.
<InlineSnippet language="csharp">
{`
[Year(2025)]
public DateTime CurrentYearEvent  { get; set; }
`}
</InlineSnippet>

## Collection Validation Attributes

### Collection Size Validation

#### `NotEmpty<T>`
Validates that collection is not null or empty.
<InlineSnippet language="csharp">
{`
[NotEmpty<string>]
public List<string> Tags { get; set; }
`}
</InlineSnippet>

#### `MinLength<T>(int minimum)`
Validates minimum collection size.
<InlineSnippet language="csharp">
{`
[MinLength<string>(1)]
public string[] Categories  { get; set; }
`}
</InlineSnippet>

#### `MaxLength<T>(int maximum)`
Validates maximum collection size.
<InlineSnippet language="csharp">
{`
[MaxLength<int>(10)]
public List<int> Scores  { get; set; }
`}
</InlineSnippet>

#### `Length<T>(int exactLength)`
Validates exact collection size.
<InlineSnippet language="csharp">
{`
[Length<byte>(16)]
public byte[] Hash  { get; set; }
`}
</InlineSnippet>

### Collection Content Validation

#### `Contains<T>(T expectedValue)`
Validates that collection contains specific value.
<InlineSnippet language="csharp">
{`
[Contains<string>("required")]
public List<string> Features  { get; set; }
`}
</InlineSnippet>

#### `NotContain<T>(T forbiddenValue)`
Validates that collection does not contain specific value.
<InlineSnippet language="csharp">
{`
[NotContain<string>("banned")]
public string[] Words  { get; set; }
`}
</InlineSnippet>

#### `Unique<T>`
Validates that all collection elements are unique.
<InlineSnippet language="csharp">
{`
[Unique<string>]
public List<string> UniqueNames  { get; set; }
`}
</InlineSnippet>

#### `Single<T>`
Validates that collection contains exactly one element.
<InlineSnippet language="csharp">
{`
[Single<string>]
public string[] PrimaryEmail  { get; set; }
`}
</InlineSnippet>

#### `SingleOrNone<T>`
Validates that collection contains zero or one element.
<InlineSnippet language="csharp">
{`
[SingleOrNone<string>]
public List<string> OptionalValue { get; set; }
`}
</InlineSnippet>

## General Validation Attributes

#### `NotNull`
Validates that object is not null.
<InlineSnippet language="csharp">
{`
[NotNull]
public object Data  { get; set; }
`}
</InlineSnippet>

## Usage Notes

- **Null Handling**: Most attributes handle null values gracefully
- **Localization**: All error messages support localization through the `IFormatter` interface
- **Performance**: Attributes are optimized for minimal overhead
- **Composition**: Multiple attributes can be applied to the same property
- **Custom Messages**: Override error messages using custom formatters

## Creating Custom Attributes

See [Extending EasyValidate](extending.md) for guidance on creating your own validation attributes.

---

**Next:** Learn how to [extend EasyValidate](extending.md) with custom validation logic.

</DocsWrapper>
