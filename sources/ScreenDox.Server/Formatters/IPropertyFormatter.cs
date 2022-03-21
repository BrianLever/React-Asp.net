namespace ScreenDox.Server.Formatters
{
    public interface IPropertyFormatter<T>
        where T: struct
    {
        /// <summary>
        /// Format date. Show only time when date is equal to 'Today' date on the server.
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <param name="useTodayLabel">When "true", return "Today" label instead of date.</param>
        /// <returns>Formatted date as string</returns>
        string Format(T? value, bool useTodayLabel = false);


        /// <summary>
        /// Format date. Show only time when date is equal to 'Today' date on the server.
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <param name="useTodayLabel">When "true", return "Today" label instead of date.</param>
        /// <returns>Formatted date as string</returns>
        string FormatAsDate(T? value, bool useTodayLabel = false);
    }
}