

using EasyValidate.Abstractions;
using EasyValidate.Attributes;

namespace ConsoleTest;

public partial class TimingUser(bool logResult)
{
    private bool logResult = logResult;
    public void EnableLogging()
    {
        logResult = true;
    }


    private void RunAndLog(string testName, Action action)
    {
        if (logResult)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            Console.WriteLine($"{testName}: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
        else
        {
            action();
        }
    }
    // Template: NotNullAttribute timing
    private void TimeNotNullAttribute_Valid()
    {
        RunAndLog("NotNullAttribute (valid)", () =>
        {
            var attr = new NotNullAttribute();
            var result = attr.Validate("TestProperty", "valid string");
        });
    }

    private void TimeNotNullAttribute_Invalid()
    {
        RunAndLog("NotNullAttribute (invalid)", () =>
        {
            var attr = new NotNullAttribute();
            var result = attr.Validate("TestProperty", null);
        });
    }

    private void TimeMaxLengthAttribute_Valid()
    {
        RunAndLog("MaxLengthAttribute (valid)", () =>
        {
            var attr = new MaxLengthAttribute(10);
            var result = attr.Validate("TestProperty", "1234567890");
        });
    }

    private void TimeMaxLengthAttribute_Invalid()
    {
        RunAndLog("MaxLengthAttribute (invalid)", () =>
        {
            var attr = new MaxLengthAttribute(5);
            var result = attr.Validate("TestProperty", "1234567890");
        });
    }

    private void TimeMinLengthAttribute_Valid()
    {
        RunAndLog("MinLengthAttribute (valid)", () =>
        {
            var attr = new MinLengthAttribute(5);
            var result = attr.Validate("TestProperty", "12345");
        });
    }

    private void TimeMinLengthAttribute_Invalid()
    {
        RunAndLog("MinLengthAttribute (invalid)", () =>
        {
            var attr = new MinLengthAttribute(10);
            var result = attr.Validate("TestProperty", "12345");
        });
    }

    private void TimeRangeAttribute_Valid()
    {
        RunAndLog("RangeAttribute (valid)", () =>
        {
            var attr = new RangeAttribute(1, 10, 0);
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeRangeAttribute_Invalid()
    {
        RunAndLog("RangeAttribute (invalid)", () =>
        {
            var attr = new RangeAttribute(1, 10, 0);
            var result = attr.Validate("TestProperty", 20);
        });
    }

    // General Attributes
    private void TimeNotDefaultAttribute_Valid()
    {
        RunAndLog("NotDefaultAttribute (valid)", () =>
        {
            var attr = new NotDefaultAttribute();
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeNotDefaultAttribute_Invalid()
    {
        RunAndLog("NotDefaultAttribute (invalid)", () =>
        {
            var attr = new NotDefaultAttribute();
            var result = attr.Validate("TestProperty", 0);
        });
    }

    private void TimeEqualToAttribute_Valid()
    {
        RunAndLog("EqualToAttribute (valid)", () =>
        {
            var attr = new EqualToAttribute("test");
            var result = attr.Validate("TestProperty", "test");
        });
    }

    private void TimeEqualToAttribute_Invalid()
    {
        RunAndLog("EqualToAttribute (invalid)", () =>
        {
            var attr = new EqualToAttribute("test");
            var result = attr.Validate("TestProperty", "other");
        });
    }

    private void TimeNotEqualToAttribute_Valid()
    {
        RunAndLog("NotEqualToAttribute (valid)", () =>
        {
            var attr = new NotEqualToAttribute("test");
            var result = attr.Validate("TestProperty", "other");
        });
    }

    private void TimeNotEqualToAttribute_Invalid()
    {
        RunAndLog("NotEqualToAttribute (invalid)", () =>
        {
            var attr = new NotEqualToAttribute("test");
            var result = attr.Validate("TestProperty", "test");
        });
    }

    private void TimeOptionalAttribute_Valid()
    {
        RunAndLog("OptionalAttribute (valid)", () =>
        {
            var attr = new OptionalAttribute();
            var result = attr.Validate("TestProperty", "any value");
        });
    }

    private void TimeOptionalAttribute_Invalid()
    {
        RunAndLog("OptionalAttribute (invalid)", () =>
        {
            var attr = new OptionalAttribute();
            var result = attr.Validate("TestProperty", null);
        });
    }

    // String Attributes
    private void TimeEmailAddressAttribute_Valid()
    {
        RunAndLog("EmailAddressAttribute (valid)", () =>
        {
            var attr = new EmailAddressAttribute();
            var result = attr.Validate("TestProperty", "test@example.com");
        });
    }

    private void TimeEmailAddressAttribute_Invalid()
    {
        RunAndLog("EmailAddressAttribute (invalid)", () =>
        {
            var attr = new EmailAddressAttribute();
            var result = attr.Validate("TestProperty", "invalid-email");
        });
    }

    private void TimeGuidAttribute_Valid()
    {
        RunAndLog("GuidAttribute (valid)", () =>
        {
            var attr = new GuidAttribute();
            var result = attr.Validate("TestProperty", "550e8400-e29b-41d4-a716-446655440000");
        });
    }

    private void TimeGuidAttribute_Invalid()
    {
        RunAndLog("GuidAttribute (invalid)", () =>
        {
            var attr = new GuidAttribute();
            var result = attr.Validate("TestProperty", "not-a-guid");
        });
    }

    private void TimeUrlAttribute_Valid()
    {
        RunAndLog("UrlAttribute (valid)", () =>
        {
            var attr = new UrlAttribute();
            var result = attr.Validate("TestProperty", "https://www.example.com");
        });
    }

    private void TimeUrlAttribute_Invalid()
    {
        RunAndLog("UrlAttribute (invalid)", () =>
        {
            var attr = new UrlAttribute();
            var result = attr.Validate("TestProperty", "not-a-url");
        });
    }

    private void TimeAlphaAttribute_Valid()
    {
        RunAndLog("AlphaAttribute (valid)", () =>
        {
            var attr = new AlphaAttribute();
            var result = attr.Validate("TestProperty", "OnlyLetters");
        });
    }

    private void TimeAlphaAttribute_Invalid()
    {
        RunAndLog("AlphaAttribute (invalid)", () =>
        {
            var attr = new AlphaAttribute();
            var result = attr.Validate("TestProperty", "Letters123");
        });
    }

    private void TimeAlphaNumericAttribute_Valid()
    {
        RunAndLog("AlphaNumericAttribute (valid)", () =>
        {
            var attr = new AlphaNumericAttribute();
            var result = attr.Validate("TestProperty", "Letters123");
        });
    }

    private void TimeAlphaNumericAttribute_Invalid()
    {
        RunAndLog("AlphaNumericAttribute (invalid)", () =>
        {
            var attr = new AlphaNumericAttribute();
            var result = attr.Validate("TestProperty", "Letters123!");
        });
    }

    private void TimeContainsAttribute_Valid()
    {
        RunAndLog("ContainsAttribute (valid)", () =>
        {
            var attr = new ContainsAttribute("test");
            var result = attr.Validate("TestProperty", "testing string");
        });
    }

    private void TimeContainsAttribute_Invalid()
    {
        RunAndLog("ContainsAttribute (invalid)", () =>
        {
            var attr = new ContainsAttribute("test");
            var result = attr.Validate("TestProperty", "other string");
        });
    }

    private void TimeNotContainsAttribute_Valid()
    {
        RunAndLog("NotContainsAttribute (valid)", () =>
        {
            var attr = new NotContainsAttribute("test");
            var result = attr.Validate("TestProperty", "other string");
        });
    }

    private void TimeNotContainsAttribute_Invalid()
    {
        RunAndLog("NotContainsAttribute (invalid)", () =>
        {
            var attr = new NotContainsAttribute("test");
            var result = attr.Validate("TestProperty", "testing string");
        });
    }

    private void TimeStartsWithAttribute_Valid()
    {
        RunAndLog("StartsWithAttribute (valid)", () =>
        {
            var attr = new StartsWithAttribute("test");
            var result = attr.Validate("TestProperty", "testing string");
        });
    }

    private void TimeStartsWithAttribute_Invalid()
    {
        RunAndLog("StartsWithAttribute (invalid)", () =>
        {
            var attr = new StartsWithAttribute("test");
            var result = attr.Validate("TestProperty", "other string");
        });
    }

    private void TimeEndsWithAttribute_Valid()
    {
        RunAndLog("EndsWithAttribute (valid)", () =>
        {
            var attr = new EndsWithAttribute("test");
            var result = attr.Validate("TestProperty", "string test");
        });
    }

    private void TimeEndsWithAttribute_Invalid()
    {
        RunAndLog("EndsWithAttribute (invalid)", () =>
        {
            var attr = new EndsWithAttribute("test");
            var result = attr.Validate("TestProperty", "string other");
        });
    }

    private void TimeNotEmptyAttribute_Valid()
    {
        RunAndLog("NotEmptyAttribute (valid)", () =>
        {
            var attr = new NotEmptyAttribute();
            var result = attr.Validate("TestProperty", "not empty");
        });
    }

    private void TimeNotEmptyAttribute_Invalid()
    {
        RunAndLog("NotEmptyAttribute (invalid)", () =>
        {
            var attr = new NotEmptyAttribute();
            var result = attr.Validate("TestProperty", "");
        });
    }

    private void TimeUppercaseAttribute_Valid()
    {
        RunAndLog("UppercaseAttribute (valid)", () =>
        {
            var attr = new UppercaseAttribute();
            var result = attr.Validate("TestProperty", "UPPERCASE");
        });
    }

    private void TimeUppercaseAttribute_Invalid()
    {
        RunAndLog("UppercaseAttribute (invalid)", () =>
        {
            var attr = new UppercaseAttribute();
            var result = attr.Validate("TestProperty", "lowercase");
        });
    }

    private void TimeLowercaseAttribute_Valid()
    {
        RunAndLog("LowercaseAttribute (valid)", () =>
        {
            var attr = new LowercaseAttribute();
            var result = attr.Validate("TestProperty", "lowercase");
        });
    }

    private void TimeLowercaseAttribute_Invalid()
    {
        RunAndLog("LowercaseAttribute (invalid)", () =>
        {
            var attr = new LowercaseAttribute();
            var result = attr.Validate("TestProperty", "UPPERCASE");
        });
    }

    private void TimeFileExtensionAttribute_Valid()
    {
        RunAndLog("FileExtensionAttribute (valid)", () =>
        {
            var attr = new FileExtensionAttribute(".txt", ".pdf");
            var result = attr.Validate("TestProperty", "document.txt");
        });
    }

    private void TimeFileExtensionAttribute_Invalid()
    {
        RunAndLog("FileExtensionAttribute (invalid)", () =>
        {
            var attr = new FileExtensionAttribute(".txt", ".pdf");
            var result = attr.Validate("TestProperty", "document.exe");
        });
    }

    private void TimeISBNAttribute_Valid()
    {
        RunAndLog("ISBNAttribute (valid)", () =>
        {
            var attr = new ISBNAttribute();
            var result = attr.Validate("TestProperty", "978-0-596-52068-7");
        });
    }

    private void TimeISBNAttribute_Invalid()
    {
        RunAndLog("ISBNAttribute (invalid)", () =>
        {
            var attr = new ISBNAttribute();
            var result = attr.Validate("TestProperty", "invalid-isbn");
        });
    }

    private void TimeCreditCardAttribute_Valid()
    {
        RunAndLog("CreditCardAttribute (valid)", () =>
        {
            var attr = new CreditCardAttribute();
            var result = attr.Validate("TestProperty", "4111111111111111");
        });
    }

    private void TimeCreditCardAttribute_Invalid()
    {
        RunAndLog("CreditCardAttribute (invalid)", () =>
        {
            var attr = new CreditCardAttribute();
            var result = attr.Validate("TestProperty", "invalid-card");
        });
    }

    private void TimePhoneAttribute_Valid()
    {
        RunAndLog("PhoneAttribute (valid)", () =>
        {
            var attr = new PhoneAttribute();
            var result = attr.Validate("TestProperty", "+1-555-123-4567");
        });
    }

    private void TimePhoneAttribute_Invalid()
    {
        RunAndLog("PhoneAttribute (invalid)", () =>
        {
            var attr = new PhoneAttribute();
            var result = attr.Validate("TestProperty", "invalid-phone");
        });
    }

    private void TimeIpAddressAttribute_Valid()
    {
        RunAndLog("IpAddressAttribute (valid)", () =>
        {
            var attr = new IpAddressAttribute();
            var result = attr.Validate("TestProperty", "192.168.1.1");
        });
    }

    private void TimeIpAddressAttribute_Invalid()
    {
        RunAndLog("IpAddressAttribute (invalid)", () =>
        {
            var attr = new IpAddressAttribute();
            var result = attr.Validate("TestProperty", "invalid-ip");
        });
    }

    private void TimeHexAttribute_Valid()
    {
        RunAndLog("HexAttribute (valid)", () =>
        {
            var attr = new HexAttribute();
            var result = attr.Validate("TestProperty", "1A2B3C");
        });
    }

    private void TimeHexAttribute_Invalid()
    {
        RunAndLog("HexAttribute (invalid)", () =>
        {
            var attr = new HexAttribute();
            var result = attr.Validate("TestProperty", "invalid-hex!");
        });
    }

    private void TimeAsciiAttribute_Valid()
    {
        RunAndLog("AsciiAttribute (valid)", () =>
        {
            var attr = new AsciiAttribute();
            var result = attr.Validate("TestProperty", "ASCII Text");
        });
    }

    private void TimeAsciiAttribute_Invalid()
    {
        RunAndLog("AsciiAttribute (invalid)", () =>
        {
            var attr = new AsciiAttribute();
            var result = attr.Validate("TestProperty", "Non-ASCII: ñ");
        });
    }

    private void TimeMacAddressAttribute_Valid()
    {
        RunAndLog("MacAddressAttribute (valid)", () =>
        {
            var attr = new MacAddressAttribute();
            var result = attr.Validate("TestProperty", "00:1B:44:11:3A:B7");
        });
    }

    private void TimeMacAddressAttribute_Invalid()
    {
        RunAndLog("MacAddressAttribute (invalid)", () =>
        {
            var attr = new MacAddressAttribute();
            var result = attr.Validate("TestProperty", "invalid-mac");
        });
    }

    // Numeric Attributes
    private void TimeGreaterThanOrEqualToAttribute_Valid()
    {
        RunAndLog("GreaterThanOrEqualToAttribute (valid)", () =>
        {
            var attr = new GreaterThanOrEqualToAttribute(5);
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeGreaterThanOrEqualToAttribute_Invalid()
    {
        RunAndLog("GreaterThanOrEqualToAttribute (invalid)", () =>
        {
            var attr = new GreaterThanOrEqualToAttribute(5);
            var result = attr.Validate("TestProperty", 3);
        });
    }

    private void TimeLessThanOrEqualToAttribute_Valid()
    {
        RunAndLog("LessThanOrEqualToAttribute (valid)", () =>
        {
            var attr = new LessThanOrEqualToAttribute(10);
            var result = attr.Validate("TestProperty", 10);
        });
    }

    private void TimeLessThanOrEqualToAttribute_Invalid()
    {
        RunAndLog("LessThanOrEqualToAttribute (invalid)", () =>
        {
            var attr = new LessThanOrEqualToAttribute(10);
            var result = attr.Validate("TestProperty", 15);
        });
    }

    private void TimeNotZeroAttribute_Valid()
    {
        RunAndLog("NotZeroAttribute (valid)", () =>
        {
            var attr = new NotZeroAttribute();
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeNotZeroAttribute_Invalid()
    {
        RunAndLog("NotZeroAttribute (invalid)", () =>
        {
            var attr = new NotZeroAttribute();
            var result = attr.Validate("TestProperty", 0);
        });
    }

    private void TimePrimeAttribute_Valid()
    {
        RunAndLog("PrimeAttribute (valid)", () =>
        {
            var attr = new PrimeAttribute();
            var result = attr.Validate("TestProperty", 7);
        });
    }

    private void TimePrimeAttribute_Invalid()
    {
        RunAndLog("PrimeAttribute (invalid)", () =>
        {
            var attr = new PrimeAttribute();
            var result = attr.Validate("TestProperty", 8);
        });
    }

    private void TimeFibonacciAttribute_Valid()
    {
        RunAndLog("FibonacciAttribute (valid)", () =>
        {
            var attr = new FibonacciAttribute();
            var result = attr.Validate("TestProperty", 8);
        });
    }

    private void TimeFibonacciAttribute_Invalid()
    {
        RunAndLog("FibonacciAttribute (invalid)", () =>
        {
            var attr = new FibonacciAttribute();
            var result = attr.Validate("TestProperty", 9);
        });
    }

    private void TimeMultipleOfAttribute_Valid()
    {
        RunAndLog("MultipleOfAttribute (valid)", () =>
        {
            var attr = new MultipleOfAttribute(3);
            var result = attr.Validate("TestProperty", 9);
        });
    }

    private void TimeMultipleOfAttribute_Invalid()
    {
        RunAndLog("MultipleOfAttribute (invalid)", () =>
        {
            var attr = new MultipleOfAttribute(3);
            var result = attr.Validate("TestProperty", 10);
        });
    }

    private void TimeDivisibleByAttribute_Valid()
    {
        RunAndLog("DivisibleByAttribute (valid)", () =>
        {
            var attr = new DivisibleByAttribute(4);
            var result = attr.Validate("TestProperty", 12);
        });
    }

    private void TimeDivisibleByAttribute_Invalid()
    {
        RunAndLog("DivisibleByAttribute (invalid)", () =>
        {
            var attr = new DivisibleByAttribute(4);
            var result = attr.Validate("TestProperty", 10);
        });
    }

    private void TimePowerOfAttribute_Valid()
    {
        RunAndLog("PowerOfAttribute (valid)", () =>
        {
            var attr = new PowerOfAttribute(2);
            var result = attr.Validate("TestProperty", 8);
        });
    }

    private void TimePowerOfAttribute_Invalid()
    {
        RunAndLog("PowerOfAttribute (invalid)", () =>
        {
            var attr = new PowerOfAttribute(2);
            var result = attr.Validate("TestProperty", 9);
        });
    }

    private void TimeMinDigitsAttribute_Valid()
    {
        RunAndLog("MinDigitsAttribute (valid)", () =>
        {
            var attr = new MinDigitsAttribute(3);
            var result = attr.Validate("TestProperty", 123);
        });
    }

    private void TimeMinDigitsAttribute_Invalid()
    {
        RunAndLog("MinDigitsAttribute (invalid)", () =>
        {
            var attr = new MinDigitsAttribute(3);
            var result = attr.Validate("TestProperty", 12);
        });
    }

    private void TimeMaxDigitsAttribute_Valid()
    {
        RunAndLog("MaxDigitsAttribute (valid)", () =>
        {
            var attr = new MaxDigitsAttribute(3);
            var result = attr.Validate("TestProperty", 123);
        });
    }

    private void TimeMaxDigitsAttribute_Invalid()
    {
        RunAndLog("MaxDigitsAttribute (invalid)", () =>
        {
            var attr = new MaxDigitsAttribute(3);
            var result = attr.Validate("TestProperty", 1234);
        });
    }
    private void TimeGreaterThanAttribute_Valid()
    {
        RunAndLog("GreaterThanAttribute (valid)", () =>
        {
            var attr = new GreaterThanAttribute(5);
            var result = attr.Validate("TestProperty", 10);
        });
    }

    private void TimeGreaterThanAttribute_Invalid()
    {
        RunAndLog("GreaterThanAttribute (invalid)", () =>
        {
            var attr = new GreaterThanAttribute(5);
            var result = attr.Validate("TestProperty", 3);
        });
    }

    private void TimeLessThanAttribute_Valid()
    {
        RunAndLog("LessThanAttribute (valid)", () =>
        {
            var attr = new LessThanAttribute(10);
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeLessThanAttribute_Invalid()
    {
        RunAndLog("LessThanAttribute (invalid)", () =>
        {
            var attr = new LessThanAttribute(10);
            var result = attr.Validate("TestProperty", 15);
        });
    }

    private void TimePositiveAttribute_Valid()
    {
        RunAndLog("PositiveAttribute (valid)", () =>
        {
            var attr = new PositiveAttribute();
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimePositiveAttribute_Invalid()
    {
        RunAndLog("PositiveAttribute (invalid)", () =>
        {
            var attr = new PositiveAttribute();
            var result = attr.Validate("TestProperty", -5);
        });
    }

    private void TimeNegativeAttribute_Valid()
    {
        RunAndLog("NegativeAttribute (valid)", () =>
        {
            var attr = new NegativeAttribute();
            var result = attr.Validate("TestProperty", -5);
        });
    }

    private void TimeNegativeAttribute_Invalid()
    {
        RunAndLog("NegativeAttribute (invalid)", () =>
        {
            var attr = new NegativeAttribute();
            var result = attr.Validate("TestProperty", 5);
        });
    }

    private void TimeEvenNumberAttribute_Valid()
    {
        RunAndLog("EvenNumberAttribute (valid)", () =>
        {
            var attr = new EvenNumberAttribute();
            var result = attr.Validate("TestProperty", 4);
        });
    }

    private void TimeEvenNumberAttribute_Invalid()
    {
        RunAndLog("EvenNumberAttribute (invalid)", () =>
        {
            var attr = new EvenNumberAttribute();
            var result = attr.Validate("TestProperty", 3);
        });
    }

    private void TimeOddNumberAttribute_Valid()
    {
        RunAndLog("OddNumberAttribute (valid)", () =>
        {
            var attr = new OddNumberAttribute();
            var result = attr.Validate("TestProperty", 3);
        });
    }

    private void TimeOddNumberAttribute_Invalid()
    {
        RunAndLog("OddNumberAttribute (invalid)", () =>
        {
            var attr = new OddNumberAttribute();
            var result = attr.Validate("TestProperty", 4);
        });
    }

    // Collection Attributes
    private void TimeHasElementsAttribute_Valid()
    {
        RunAndLog("HasElementsAttribute (valid)", () =>
        {
            var attr = new HasElementsAttribute();
            var result = attr.Validate("TestProperty", new[] { 1, 2, 3 });
        });
    }

    private void TimeHasElementsAttribute_Invalid()
    {
        RunAndLog("HasElementsAttribute (invalid)", () =>
        {
            var attr = new HasElementsAttribute();
            var result = attr.Validate("TestProperty", new int[0]);
        });
    }

    private void TimeUniqueElementsAttribute_Valid()
    {
        RunAndLog("UniqueElementsAttribute (valid)", () =>
        {
            var attr = new UniqueElementsAttribute();
            var result = attr.Validate("TestProperty", new[] { 1, 2, 3 });
        });
    }

    private void TimeUniqueElementsAttribute_Invalid()
    {
        RunAndLog("UniqueElementsAttribute (invalid)", () =>
        {
            var attr = new UniqueElementsAttribute();
            var result = attr.Validate("TestProperty", new[] { 1, 2, 2, 3 });
        });
    }

    // Date Attributes
    private void TimeFutureDateAttribute_Valid()
    {
        RunAndLog("FutureDateAttribute (valid)", () =>
        {
            var attr = new FutureDateAttribute();
            var result = attr.Validate("TestProperty", DateTime.Now.AddDays(1));
        });
    }

    private void TimeFutureDateAttribute_Invalid()
    {
        RunAndLog("FutureDateAttribute (invalid)", () =>
        {
            var attr = new FutureDateAttribute();
            var result = attr.Validate("TestProperty", DateTime.Now.AddDays(-1));
        });
    }

    private void TimePastDateAttribute_Valid()
    {
        RunAndLog("PastDateAttribute (valid)", () =>
        {
            var attr = new PastDateAttribute();
            var result = attr.Validate("TestProperty", DateTime.Now.AddDays(-1));
        });
    }

    private void TimePastDateAttribute_Invalid()
    {
        RunAndLog("PastDateAttribute (invalid)", () =>
        {
            var attr = new PastDateAttribute();
            var result = attr.Validate("TestProperty", DateTime.Now.AddDays(1));
        });
    }

    private void TimeMinAgeAttribute_Valid()
    {
        RunAndLog("MinAgeAttribute (valid)", () =>
        {
            var attr = new MinAgeAttribute(18);
            var result = attr.Validate("TestProperty", DateTime.Now.AddYears(-25));
        });
    }

    private void TimeMinAgeAttribute_Invalid()
    {
        RunAndLog("MinAgeAttribute (invalid)", () =>
        {
            var attr = new MinAgeAttribute(18);
            var result = attr.Validate("TestProperty", DateTime.Now.AddYears(-10));
        });
    }

    private void TimeMaxAgeAttribute_Valid()
    {
        RunAndLog("MaxAgeAttribute (valid)", () =>
        {
            var attr = new MaxAgeAttribute(65);
            var result = attr.Validate("TestProperty", DateTime.Now.AddYears(-30));
        });
    }

    private void TimeMaxAgeAttribute_Invalid()
    {
        RunAndLog("MaxAgeAttribute (invalid)", () =>
        {
            var attr = new MaxAgeAttribute(65);
            var result = attr.Validate("TestProperty", DateTime.Now.AddYears(-70));
        });
    }

    private void TimeLeapYearAttribute_Valid()
    {
        RunAndLog("LeapYearAttribute (valid)", () =>
        {
            var attr = new LeapYearAttribute();
            var result = attr.Validate("TestProperty", new DateTime(2024, 1, 1));
        });
    }

    private void TimeLeapYearAttribute_Invalid()
    {
        RunAndLog("LeapYearAttribute (invalid)", () =>
        {
            var attr = new LeapYearAttribute();
            var result = attr.Validate("TestProperty", new DateTime(2023, 1, 1));
        });
    }

    private void TimeUTCAttribute_Valid()
    {
        RunAndLog("UTCAttribute (valid)", () =>
        {
            var attr = new UTCAttribute();
            var result = attr.Validate("TestProperty", DateTime.UtcNow);
        });
    }

    private void TimeUTCAttribute_Invalid()
    {
        RunAndLog("UTCAttribute (invalid)", () =>
        {
            var attr = new UTCAttribute();
            var result = attr.Validate("TestProperty", DateTime.Now);
        });
    }

    // Public runners
    public void RunValid()
    {
        TimeNotNullAttribute_Valid();
        TimeMaxLengthAttribute_Valid();
        TimeMinLengthAttribute_Valid();
        TimeRangeAttribute_Valid();
        TimeNotDefaultAttribute_Valid();
        TimeEqualToAttribute_Valid();
        TimeNotEqualToAttribute_Valid();
        TimeOptionalAttribute_Valid();
        TimeEmailAddressAttribute_Valid();
        TimeGuidAttribute_Valid();
        TimeUrlAttribute_Valid();
        TimeAlphaAttribute_Valid();
        TimeAlphaNumericAttribute_Valid();
        TimeContainsAttribute_Valid();
        TimeNotContainsAttribute_Valid();
        TimeStartsWithAttribute_Valid();
        TimeEndsWithAttribute_Valid();
        TimeNotEmptyAttribute_Valid();
        TimeUppercaseAttribute_Valid();
        TimeLowercaseAttribute_Valid();
        TimeFileExtensionAttribute_Valid();
        TimeISBNAttribute_Valid();
        TimeCreditCardAttribute_Valid();
        TimePhoneAttribute_Valid();
        TimeIpAddressAttribute_Valid();
        TimeHexAttribute_Valid();
        TimeAsciiAttribute_Valid();
        TimeMacAddressAttribute_Valid();
        TimeGreaterThanAttribute_Valid();
        TimeLessThanAttribute_Valid();
        TimePositiveAttribute_Valid();
        TimeNegativeAttribute_Valid();
        TimeEvenNumberAttribute_Valid();
        TimeOddNumberAttribute_Valid();
        TimeGreaterThanOrEqualToAttribute_Valid();
        TimeLessThanOrEqualToAttribute_Valid();
        TimeNotZeroAttribute_Valid();
        TimePrimeAttribute_Valid();
        TimeFibonacciAttribute_Valid();
        TimeMultipleOfAttribute_Valid();
        TimeDivisibleByAttribute_Valid();
        TimePowerOfAttribute_Valid();
        TimeMinDigitsAttribute_Valid();
        TimeMaxDigitsAttribute_Valid();
        TimeHasElementsAttribute_Valid();
        TimeUniqueElementsAttribute_Valid();
        TimeFutureDateAttribute_Valid();
        TimePastDateAttribute_Valid();
        TimeMinAgeAttribute_Valid();
        TimeMaxAgeAttribute_Valid();
        TimeLeapYearAttribute_Valid();
        TimeUTCAttribute_Valid();
    }

    public void RunNonInvalid()
    {
        TimeNotNullAttribute_Invalid();
        TimeMaxLengthAttribute_Invalid();
        TimeMinLengthAttribute_Invalid();
        TimeRangeAttribute_Invalid();
        TimeNotDefaultAttribute_Invalid();
        TimeEqualToAttribute_Invalid();
        TimeNotEqualToAttribute_Invalid();
        TimeOptionalAttribute_Invalid();
        TimeEmailAddressAttribute_Invalid();
        TimeGuidAttribute_Invalid();
        TimeUrlAttribute_Invalid();
        TimeAlphaAttribute_Invalid();
        TimeAlphaNumericAttribute_Invalid();
        TimeContainsAttribute_Invalid();
        TimeNotContainsAttribute_Invalid();
        TimeStartsWithAttribute_Invalid();
        TimeEndsWithAttribute_Invalid();
        TimeNotEmptyAttribute_Invalid();
        TimeUppercaseAttribute_Invalid();
        TimeLowercaseAttribute_Invalid();
        TimeFileExtensionAttribute_Invalid();
        TimeISBNAttribute_Invalid();
        TimeCreditCardAttribute_Invalid();
        TimePhoneAttribute_Invalid();
        TimeIpAddressAttribute_Invalid();
        TimeHexAttribute_Invalid();
        TimeAsciiAttribute_Invalid();
        TimeMacAddressAttribute_Invalid();
        TimeGreaterThanAttribute_Invalid();
        TimeLessThanAttribute_Invalid();
        TimePositiveAttribute_Invalid();
        TimeNegativeAttribute_Invalid();
        TimeEvenNumberAttribute_Invalid();
        TimeOddNumberAttribute_Invalid();
        TimeGreaterThanOrEqualToAttribute_Invalid();
        TimeLessThanOrEqualToAttribute_Invalid();
        TimeNotZeroAttribute_Invalid();
        TimePrimeAttribute_Invalid();
        TimeFibonacciAttribute_Invalid();
        TimeMultipleOfAttribute_Invalid();
        TimeDivisibleByAttribute_Invalid();
        TimePowerOfAttribute_Invalid();
        TimeMinDigitsAttribute_Invalid();
        TimeMaxDigitsAttribute_Invalid();
        TimeHasElementsAttribute_Invalid();
        TimeUniqueElementsAttribute_Invalid();
        TimeFutureDateAttribute_Invalid();
        TimePastDateAttribute_Invalid();
        TimeMinAgeAttribute_Invalid();
        TimeMaxAgeAttribute_Invalid();
        TimeLeapYearAttribute_Invalid();
        TimeUTCAttribute_Invalid();
    }
}





