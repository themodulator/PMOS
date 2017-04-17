using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Pmos
{
    public static class ExceptionExtensions
    {

        #region Version 1 - (2017/02/09 - Note: Methods need documentation)

        public static string ToString(this Exception ex, string delimiter)
        {
            List<Exception> ee = ex.ToList();

            string[] items = ee.Select(x => x.Message).ToArray();

            return string.Join(delimiter, items);
        }

        public static Exception ToHtml(this Exception ex)
        {
            List<Exception> ee = ex.ToList();

            List<string> errors = ee.Select(x => x.Message).ToList();


            TagBuilder tag = new TagBuilder("span");

            foreach(string e in errors)
            {
                tag.InnerHtml += e + "</br>";
            }

            return new Exception(tag.ToString());


        }

        public static List<Exception> ToList(this Exception ex)
        {

            List<Exception> ee = new List<Exception>();

            ex.EnumerateExceptions(ref ee);
            return ee;

        }

        public static string[] ToStringArray(this Exception ex)
        {
            return ex.ToList().Select(x => x.Message).ToArray();
        }

        /// <summary>
        /// Creates a string array of inner exceptions
        /// </summary>
        /// <remarks>
        /// Added 02/02/2017 09:26 a.m.
        /// </remarks>
        /// <param name="ex">The inner exception</param>
        /// <param name="text">The top error message</param>
        /// <returns>string[]</returns>
        public static string[] ToStringArray(this Exception ex, string text)
        {

            Exception e = new Exception(text, ex);

            return e.ToStringArray();
        }

        private static void EnumerateExceptions(this Exception ex, ref List<Exception> items)
        {

            if (ex.GetType() == typeof(DbEntityValidationException))
            {
                DbEntityValidationException e = (DbEntityValidationException)ex;

                var verrs = e.EntityValidationErrors.Select(x => x.ValidationErrors);

                foreach (var x in verrs)
                {
                    foreach (var y in x)
                    {
                        items.Add(new Exception(y.ErrorMessage));
                    }
                }
            }
            else
                items.Add(ex);

            

            if (ex.InnerException != null)
                ex.InnerException.EnumerateExceptions(ref items);
        }

        public static Exception ToException(this ModelStateDictionary model)
        {
            List<string> errors = new List<string>();

            foreach (var m in model.Values.Where(x => x.Errors.Any()))
            {

                foreach (var e in m.Errors)
                {

                    if(e.Exception != null | !string.IsNullOrEmpty(e.ErrorMessage))
                    {

                        if (string.IsNullOrEmpty(e.ErrorMessage) & e.Exception != null)
                            errors.Add(e.Exception.Message);
                        else
                            errors.Add(e.ErrorMessage);
                    }
                }
            }

            if (errors.Count == 0)
                return null;
            
            string msg = string.Join("<br>", errors.ToArray());

            return new Exception(msg);

        }

        public static void AddException(this ModelStateDictionary model, Exception ex)
        {
            
            string[] messages = ex.ToList().Select(x => x.Message).ToArray();

            foreach(string m in messages)
            {
                model.AddModelError("", m);
            }

        }

        public static Exception ToException(this List<Exception> items)
        {
            List<String> messages = new List<string>();

            foreach(var ex in items)
            {
                List<Exception> mm = ex.ToList();

                string[] mitems = mm.Select(x => x.Message).ToArray();

                messages.AddRange(mitems);

            }

            return messages.ToException();
        }

        public static Exception ToException(this IEnumerable<string> items)
        {

            items.Reverse();

            Exception ex = new Exception(items.First());

            foreach (string item in items)
            {
                if (item != items.First())
                {
                    ex.AddException(new Exception(item));
                }
            }

            return ex;
        }

        public static void AddException(this Exception ex, Exception e)
        {
            Exception l = ex.GetLastInnerException();

            typeof(Exception)
                .GetField("_innerException", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(l, e);
        }

        public static Exception GetLastInnerException(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            else
                return ex.InnerException.GetLastInnerException();
        }

        #endregion

        
    }
}
