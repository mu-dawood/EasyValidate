using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Dates.FutureDate;

public class FutureDateSimpleTests
{
    [Fact]
    public void FutureDate_ValidFutureDate_ReturnsValidResult()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(30);
        var model = new FutureDateModel
        {
            EventDate = futureDate,
            DeadlineDate = DateTime.Now.AddMonths(2),
            ScheduledDate = DateTime.Now.AddYears(1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void FutureDate_InvalidPastDate_ReturnsInvalidResult()
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(-30);
        var model = new FutureDateModel
        {
            EventDate = pastDate, // Invalid: past date
            DeadlineDate = DateTime.Now.AddMonths(2),
            ScheduledDate = DateTime.Now.AddYears(1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.EventDate));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName == nameof(model.DeadlineDate));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName == nameof(model.ScheduledDate));

        var eventDateErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.EventDate)).ToList();
        Assert.Single(eventDateErrors);
        Assert.Contains("must be a future date", eventDateErrors[0].FormattedMessage);
    }

    [Fact]
    public void FutureDate_InvalidCurrentDate_ReturnsInvalidResult()
    {
        // Arrange
        var currentDate = DateTime.Now;
        var model = new FutureDateModel
        {
            EventDate = currentDate, // Invalid: current date (not future)
            DeadlineDate = DateTime.Now.AddMonths(2),
            ScheduledDate = DateTime.Now.AddYears(1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.EventDate));

        var eventDateErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.EventDate)).ToList();
        Assert.Single(eventDateErrors);
        Assert.Contains("must be a future date", eventDateErrors[0].FormattedMessage);
    }

    [Fact]
    public void FutureDate_NullableValue_ReturnsValidResult()
    {
        // Arrange
        var model = new FutureDateModel
        {
            EventDate = DateTime.Now.AddDays(30),
            DeadlineDate = null, // Null should be valid for nullable
            ScheduledDate = DateTime.Now.AddYears(1)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName == nameof(model.DeadlineDate));
        Assert.True(result.IsValid() || !result.GetAllErrors().Any());
    }

    [Theory]
    [InlineData(1)] // 1 day in future
    [InlineData(7)] // 1 week in future
    [InlineData(30)] // 1 month in future
    [InlineData(365)] // 1 year in future
    public void FutureDate_ValidFutureDates_ReturnsValidResult(int daysInFuture)
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(daysInFuture);
        var model = new FutureDateModel
        {
            EventDate = futureDate,
            DeadlineDate = DateTime.Now.AddDays(10),
            ScheduledDate = DateTime.Now.AddDays(20)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData(-1)] // 1 day in past
    [InlineData(-7)] // 1 week in past
    [InlineData(-30)] // 1 month in past
    [InlineData(-365)] // 1 year in past
    public void FutureDate_InvalidPastDates_ReturnsInvalidResult(int daysInPast)
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(daysInPast);
        var model = new FutureDateModel
        {
            EventDate = pastDate,
            DeadlineDate = DateTime.Now.AddDays(10),
            ScheduledDate = DateTime.Now.AddDays(20)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.EventDate));

        var eventDateErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.EventDate)).ToList();
        Assert.Single(eventDateErrors);
        Assert.Contains("must be a future date", eventDateErrors[0].FormattedMessage);
    }

    [Fact]
    public void FutureDate_MultipleInvalidDates_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new FutureDateModel
        {
            EventDate = DateTime.Now.AddDays(-1), // Invalid: past
            DeadlineDate = DateTime.Now.AddDays(-10), // Invalid: past
            ScheduledDate = DateTime.Now.AddDays(-30) // Invalid: past
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.ErrorsCount);
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.EventDate));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.DeadlineDate));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.ScheduledDate));
    }

    [Fact]
    public void FutureDate_VeryDistantFuture_ReturnsValidResult()
    {
        // Arrange
        var distantFuture = DateTime.Now.AddYears(100);
        var model = new FutureDateModel
        {
            EventDate = distantFuture,
            DeadlineDate = DateTime.Now.AddYears(50),
            ScheduledDate = DateTime.Now.AddYears(25)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }
}
