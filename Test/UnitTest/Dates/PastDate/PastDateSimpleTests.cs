using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Dates.PastDate;

public class PastDateSimpleTests
{
    [Fact]
    public void ValidPastDates_ShouldReturnValid()
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(-1);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(pastDate),
            OptionalPastDate = DateTime.Now.AddMonths(-6),
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)),
            OptionalDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            CreatedAt = DateTimeOffset.Now.AddHours(-5)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void FutureEventDate_ShouldReturnInvalid()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(1);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(futureDate),
            OptionalPastDate = DateTime.Now.AddDays(-1),
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            CreatedAt = DateTimeOffset.Now.AddDays(-1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("EventDate"));
    }

    [Fact]
    public void TodayDate_ShouldReturnInvalid()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(today),
            OptionalPastDate = DateTime.Now.AddDays(-1),
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            CreatedAt = DateTimeOffset.Now.AddDays(-1)
        };

        // Act
        var result = model.Validate();

        // Assert
        // Note: This depends on the exact implementation - some past date validators 
        // might consider "today" as valid, others might not
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("EventDate"));
    }

    [Fact]
    public void OptionalPastDate_WithNull_ShouldReturnValid()
    {
        // Arrange
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            OptionalPastDate = null,
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            CreatedAt = DateTimeOffset.Now.AddDays(-1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.HasErrors());
        Assert.True(result.IsValid());
    }

    [Theory]
    [InlineData(-1)]    // Yesterday
    [InlineData(-7)]    // Week ago
    [InlineData(-30)]   // Month ago
    [InlineData(-365)]  // Year ago
    public void EventDate_WithVariousPastDays_ShouldReturnValid(int daysAgo)
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(daysAgo);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(pastDate)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("EventDate")));
    }

    [Theory]
    [InlineData(1)]     // Tomorrow
    [InlineData(7)]     // Week from now
    [InlineData(30)]    // Month from now
    [InlineData(365)]   // Year from now
    public void EventDate_WithFutureDays_ShouldReturnInvalid(int daysFromNow)
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(daysFromNow);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(futureDate)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("EventDate"));
    }

    [Fact]
    public void DateOnly_WithPastDate_ShouldReturnValid()
    {
        // Arrange
        var pastDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10));
        var model = new PastDateModel
        {
            BirthDate = pastDate
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("BirthDate")));
    }

    [Fact]
    public void DateOnly_WithFutureDate_ShouldReturnInvalid()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));
        var model = new PastDateModel
        {
            BirthDate = futureDate
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("BirthDate"));
    }

    [Fact]
    public void DateTimeOffset_WithPastDate_ShouldReturnValid()
    {
        // Arrange
        var pastOffset = DateTimeOffset.Now.AddHours(-2);
        var model = new PastDateModel
        {
            CreatedAt = pastOffset
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("CreatedAt")));
    }

    [Fact]
    public void DateTimeOffset_WithFutureDate_ShouldReturnInvalid()
    {
        // Arrange
        var futureOffset = DateTimeOffset.Now.AddHours(2);
        var model = new PastDateModel
        {
            CreatedAt = futureOffset
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("CreatedAt"));
    }

    [Fact]
    public void MultipleFutureDates_ShouldReturnMultipleErrors()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(5);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(futureDate),
            OptionalPastDate = futureDate,
            BirthDate = DateOnly.FromDateTime(futureDate),
            OptionalDateOnly = DateOnly.FromDateTime(futureDate),
            CreatedAt = DateTimeOffset.Now.AddDays(1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Fact]
    public void VeryOldDate_ShouldReturnValid()
    {
        // Arrange
        var veryOldDate = new DateTime(1900, 1, 1);
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(veryOldDate),
            BirthDate = DateOnly.FromDateTime(veryOldDate),
            CreatedAt = new DateTimeOffset(veryOldDate)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void MinDateTime_ShouldReturnValid()
    {
        // Arrange
        var model = new PastDateModel
        {
            EventDate = DateOnly.FromDateTime(DateTime.MinValue),
            BirthDate = DateOnly.MinValue,
            CreatedAt = DateTimeOffset.MinValue
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }
}
