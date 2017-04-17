using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pmos
{
    public static class DateExtensions
    {
        public static string GetTimestamp(this DateTime dt)
        {
            string d = dt.ToString("yyyy/MM/dd hh:mm:ss");

            return Regex.Replace(d, @"[^A-Za-z0-9]+", "");
        }

        public static DateTime? ToNullableDate(this string value)
        {
            DateTime d = DateTime.MaxValue;

            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                if (DateTime.TryParse(value, out d))
                    return d;
                else
                    return  null;
            }
        }



        public static string ToNullableDateString(this DateTime? date, string format)
        {

            return (date == null) ? "" : 
                string.IsNullOrEmpty(format) ? 
                date.Value.ToString() : date.Value.ToString(format);
        }

        public static string MonthName(this DateTime dt)
        {
            return dt.ToString("MMMM", CultureInfo.InvariantCulture);
        }

        public static string MonthName(this int month)
        {
            DateTime d = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day);

            return d.MonthName();
        }

        public static int GetMonthNumber(this string month)
        {
            string date = month + " 1, " + DateTime.Now.Year.ToString();

            DateTime d = DateTime.MinValue;

            if (DateTime.TryParse(date, out d))
                return d.Month;
            else
                return -1;
        }
    }
}
