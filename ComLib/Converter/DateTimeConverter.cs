using System;

namespace ComLib.Converter
{
    public static class DateTimeConverter
    {
        /// <summary>
        /// Converts a string with the format of datePattern to its equivalent DateTime value.
        /// </summary>
        /// <param name="param">The string to convert.</param>
        /// <param name="datePattern">The date pattern of the given string.</param>
        /// <returns>The converted DateTime value.</returns>
        public static DateTime ToDateTime(this string param, string datePattern = "MM/dd/yyyy")
        {
            //var dateInfo = new System.Globalization.DateTimeFormatInfo {ShortDatePattern = datePattern};
            DateTime validDate = DateTime.ParseExact(param, datePattern, System.Globalization.CultureInfo.InvariantCulture);
            return validDate;
        }

        // & Okay we removed any usage of ExpressionParser. And there we go~ ↑↑

        // /* Sorry that the above is hidden. But we cannot make a MethodCallExpression without giving all the parameters (including default parameters)
        // * which is needed in ExpressionParserCore.cs
        // * Anyway, it won't make the developers in trouble. 
        // */

        //public static DateTime ToDateTime(this string param)
        //{
        //    return ToDateTime(param, "MM/dd/yyyy");
        //}

        /// <summary>
        /// Converts a string with the format of datePattern to its equivalent DateTime value despite any exception (returns null on error).
        /// </summary>
        /// <param name="param">The string to convert.</param>
        /// <param name="datePattern">The date pattern of the given string.</param>
        /// <returns>The converted DateTime value.</returns>
        public static DateTime? ToDateTimeSilent(this string param, string datePattern="MM/dd/yyyy")
        {
            try
            {
                DateTime validDate = DateTime.ParseExact(param, datePattern, System.Globalization.CultureInfo.InvariantCulture);
                return validDate;
            } catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTimeLoose(this string param, string datePattern="MM/dd/yyyy")
        {
            if(string.IsNullOrWhiteSpace(param))
            {
                return null;
            } else
            {
                return ToDateTime(param, datePattern);
            }
        }

    }
}
