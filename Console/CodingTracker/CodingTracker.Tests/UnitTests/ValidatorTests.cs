using CodingTracker.Validators;

namespace CodingTracker.Tests.UnitTests;

public class ValidatorTests
{
    [Theory]
    [InlineData("29-2-24", true)]   // Leap year
    [InlineData("29-2-23", false)]  // Non-leap year
    [InlineData("31-4-24", false)]  // April only has 30 days
    [InlineData("1-1-24", true)]    // Single digit day/month
    [InlineData("01-01-24", true)]  // Double-digit with leading zero
    [InlineData("12-12-2", true)]   // Single digit year (2002)
    [InlineData("12-12-2024", false)] // Four digit year should fail exact match
    [InlineData("invalid", false)] // Not date/time related string
    [InlineData("", false)] // Not date/time related string
    public void ValidateStringAsDate_ReturnsCorrectResult(string date, bool expectedResult)
    {
        // Arrange
        
        // Act
        var result = Validator.ValidateStringAsDate(date);
        
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("00:00", true)] // Midnight
    [InlineData("12:00", true)] // Double-digit hour and minute
    [InlineData("12:00:00", false)] // Add seconds
    [InlineData("1:00", false)] // Single-digit hour
    [InlineData("25:61", false)] // Invalid time
    [InlineData("invalid", false)] // Not date/time related string
    [InlineData("", false)] // Not date/time related string
    public void ValidateStringAsTime_ReturnsCorrectResult(string time, bool expectedResult)
    {
        // Arrange
        
        // Act
        var result = Validator.ValidateStringAsTime(time);
        
        // Assert
        Assert.Equal(expectedResult, result);
    }
}
