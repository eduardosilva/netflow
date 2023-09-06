namespace Netflow;

/// <summary>
/// This class contains date time extensions.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Get the first day of a month using the source as a reference year/month.
    /// </summary>
    /// <param name="source">Reference source.</param>
    /// <returns>Returns the first day of a month.</returns>
    public static DateTime FirstDayOfMonth(this DateTime source)
    {
        return new DateTime(source.Year, source.Month, 1);
    }

    /// <summary>
    /// Get the first day of the week (Sunday) using the source as a reference year/month/day.
    /// </summary>
    /// <param name="source">Reference source.</param>
    /// <returns>Returns the first day of the week (Sunday).</returns>
    public static DateTime FirstDayOfWeek(this DateTime source)
    {
        int diff = (7 + (source.DayOfWeek - DayOfWeek.Sunday)) % 7;
        return source.Date.AddDays(-diff);
    }

}
