using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using System.Text.RegularExpressions;


namespace Pmos
{
    public static class StringExtensions
    {

        public static string FileSize(this int len)
        {
            long l = 0;

            if (long.TryParse(len.ToString(), out l))
                return l.FileSize();
            else
                return "0";

        }


        public static string FileSize(this long len)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };

            int order = 0;
            while (len >= 1024 && ++order < sizes.Length)
            {
                len = len / 1024;
            }

            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }

        public static string Strip(this string text)
        {
            text = string.IsNullOrEmpty(text) ? "" : Regex.Replace(text, @"[^A-Za-z0-9]+", "");

            return text;
        }

        public static string StripNonPrintable(this string text)
        {
            text = string.IsNullOrEmpty(text) ? "" : Regex.Replace(text, @"[^\u0000-\u007F]+", string.Empty);

            text = text.Replace("\n", string.Empty);

            return text;
        }


        public static string GetNullableString(this double? value)
        {
            return (value == null) ? "" : value.Value.ToString();
        }

        public static void SetNullableValue(this double? type, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                type = null;
            }
            else
            {
                Double d = 0;
                if (double.TryParse(value, out d))
                    type = d;
            }
        }



        public static Guid? ToGuid(this string text, bool throwException)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (throwException)
                    throw new Exception("Guid cannot be null");
                else
                    return null;
            }
            else
            {
                Guid g = Guid.Empty;

                if (Guid.TryParse(text, out g))
                    return g;
                else
                {
                    if (throwException)
                        throw new Exception("Guid is not in the right format");
                    else
                        return null;
                }
            }
        }

        public static string ToGuidFromObject(this object item)
        {
            if (item == null)
                throw new Exception("The guid cannot be null");
            else
            {
                return item.ToString().ToGuid(true).Value.ToString().Trim().ToLower();
            }
        }

        public static string ToGuid(this string guid)
        {
            return guid.ToGuid(true).Value.ToString().Trim().ToLower();
        }

        public static string DateStamp(this DateTime date)
        {
            string value = date.Year.ToString() + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0');

            return value;
        }

        public static string TimeStamp(this DateTime date)
        {
            string value = date.Hour.ToString().PadLeft(2, '0') + date.Minute.ToString().PadLeft(2, '0') + date.Second.ToString().PadLeft(2, '0'); 

            return value;
        }

        public static string DateTimeStamp(this DateTime date)
        {

            string value = date.DateStamp() + date.TimeStamp();

            return value;

        }
    }
}
