using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Models.Base;

namespace Services.Helpers
{
    public class UtilityHelper
    {
        //string minupulation

        public static string CleanKeywords(string keywords)
        {
            try
            {
                //remove special characters
                keywords = keywords.Trim()
                    .Replace("'", "")
                    .Replace("`", "")
                    .Replace("\"", "")
                    .Replace("#", "")
                    .Replace("*", "")
                    .Replace("!", "")
                    .Replace("@", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("{", "")
                    .Replace("}", "")
                    .Replace("-", " ")
                    .Replace("/", " ")
                    .Replace("~", " ")
                    .Replace(":", " ")
                    .Replace(";", " ")
                    .Replace("#", " ")
                    .Replace("%", " ")
                    .Replace(",", " ")
                    .Replace("^", " ")
                    .Replace("_", " ")
                    .Replace("+", " ")
                    .Replace("=", " ")
                    .Replace("&", " ");
                //remove some conjections
                keywords = keywords.Replace(" a ", "")
                    .Replace(" an ", "");
                //remove double spaces
                keywords = Regex.Replace(keywords, @"\s+", " ");
            }
            catch
            {
                keywords = string.Empty;
            }

            return keywords.ToLower().Trim();
        }

        public static string UppercaseFirst(string input)
        {
            var newString = input;
            if (!String.IsNullOrEmpty(input))
            {
                newString = input.First().ToString().ToUpper() + input.Substring(1);
            }
            return newString;
        }

        public static ListPaging GetPaging(IEnumerable<object> data, ListFilter filter)
        {
            var startRecord = (filter.page - 1) * filter.rows;
            var _showing = data.Skip(startRecord).Take(filter.rows);
            var endRecord = (startRecord + _showing.Count());
            var pagedData = new ListPaging
            {
                records = data.Count(),
                pages = (int)Math.Ceiling(data.Count() / (float)filter.rows),
                rows = filter.rows,
                page = filter.page,
                sort = filter.sort,
                dir = filter.dir,
                message = data.Any() ? startRecord + "-" + endRecord : "No results found",
                data = _showing
            };
            return pagedData;
        }

        //set default values, prevent nulls, and validate
        public static int DefaultVal(int? currentValue, int defaultValue)
        {
            int myint = defaultValue;
            if (currentValue != null)
            {
                if (int.TryParse(currentValue.ToString(), out myint))
                {
                    myint = Convert.ToInt32(currentValue);
                }
                if (myint == 0 && defaultValue != 0)
                {
                    myint = defaultValue;
                }
            }
            return myint;
        }

        public static bool DefaultVal(bool? currentValue, bool defaultValue)
        {
            bool mybool = defaultValue;
            if (currentValue != null)
            {
                if (currentValue.HasValue)
                {
                    mybool = Convert.ToBoolean(currentValue);
                }
            }
            return mybool;
        }

        public static DateTime DefaultVal(DateTime currentValue, DateTime defaultValue)
        {
            DateTime value = defaultValue;
            if (DateTime.TryParse(currentValue.ToString("d"), out defaultValue))
            {
                value = currentValue;
                if (value.Year < 1901)
                {
                    value = defaultValue;
                }
            }
            return value;
        }

        public static DateTime DefaultVal(DateTime? currentValue, DateTime defaultValue)
        {
            DateTime value = defaultValue;
            if (currentValue != null)
            {
                DateTime currValue = Convert.ToDateTime(currentValue);
                if (DateTime.TryParse(currValue.ToString("d"), out defaultValue))
                {
                    value = currValue;
                    if (value.Year < 1901)
                    {
                        value = defaultValue;
                    }
                }
            }
            return value;
        }

        public static DateTime? DefaultVal(DateTime? currentValue, DateTime? defaultValue)
        {
            DateTime? value = defaultValue;
            if (currentValue != null)
            {
                DateTime currValue = Convert.ToDateTime(currentValue);
                if (defaultValue != null)
                {
                    DateTime defaultVal = Convert.ToDateTime(defaultValue);
                    if (DateTime.TryParse(currValue.ToString("d"), out defaultVal))
                    {
                        value = currValue;
                    }
                }
            }
            return value;
        }

        public static string DefaultVal(string currentValue, string defaultValue)
        {
            string mystr = defaultValue;
            if (!string.IsNullOrEmpty(currentValue))
            {
                mystr = WebUtility.HtmlDecode(currentValue);
            }
            return mystr.Trim();
        }

        public static bool IsValid(object obj)
        {
            bool isValid = false;
            if (obj != null)
            {
                if (obj is string)
                {
                    if (!string.IsNullOrEmpty(obj.ToString()))
                    {
                        isValid = true;
                    }
                }
                else if (obj is DateTime)
                {
                    DateTime value;
                    if (DateTime.TryParse(obj.ToString(), out value))
                    {
                        value = (DateTime)obj;
                        if (value.Year > 1901)
                        {
                            isValid = true;
                        }
                    }
                }
                else if (obj is bool)
                {
                    if (!(bool)obj)
                    {
                        isValid = true;
                    }
                }
                else if (obj is int)
                {
                    int myint = 0;
                    if (int.TryParse(obj.ToString(), out myint))
                    {
                        myint = (int)obj;
                    }
                    if (myint > 0)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }

        //dates
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime RoundDateTime(DateTime dateTime)
        {
            var updated = dateTime.AddMinutes(30);
            return new DateTime(updated.Year, updated.Month, updated.Day,
                                 updated.Hour, 0, 0, dateTime.Kind);
        }

        public static string FormatDbDate(DateTime date)
        {
            string format = "yyyy-MM-dd";
            date = DefaultVal(date, Convert.ToDateTime("1900-01-01"));
            return date.ToString(format);
        }
    }
}
