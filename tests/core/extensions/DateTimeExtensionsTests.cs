using Netflow;
namespace Netflow.Tests.Extensions;
public class DateTimeExtensionsTests
{
    [Fact]
    public void FirstDayOfMonth_ShouldReturnFirstDay()
    {
        // Arrange
        DateTime date = new DateTime(2023, 8, 15);

        // Act
        DateTime result = date.FirstDayOfMonth();

        // Assert
        Assert.Equal(new DateTime(2023, 8, 1), result);
    }

    [Fact]
    public void FirstDayOfWeek_ShouldReturnFirstDay()
    {
        // Arrange
        DateTime date = new DateTime(2023, 8, 15); // A Tuesday

        // Act
        DateTime result = date.FirstDayOfWeek();

        // Assert
        Assert.Equal(new DateTime(2023, 8, 13), result); // Sunday before 8/15
    }
}