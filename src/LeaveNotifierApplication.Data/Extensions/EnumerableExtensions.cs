using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeaveNotifierApplication.Data.Extensions
{
    public static class EnumerableExtensions
    {
        // For sorting
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, string sortOrder, bool asc)
        {
            return asc ? source.OrderBy(sortOrder) : source.OrderByDescending(sortOrder);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string param)
        {
            return source.OrderBy(item => GetPropertyValue(item, param));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string param)
        {
            return source.OrderByDescending(item => GetPropertyValue(item, param));
        }
        // End for sorting

        // For filtering
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, string[] keys, string[] values, bool[] isFull)
        {
            if (keys == null || keys.Length == 0)
            {
                return source;
            }
            var query = keys.Zip(values.Zip(isFull, Tuple.Create), (key, tuple) => new { Key = key, Value = tuple.Item1, IsFull = tuple.Item2 });

            foreach (var q in query)
            {
                // Check fist if key is a pair
                // If it is, then check if it is a DateTime (for now only DateTime is supported as a range)
                var keyPair = GetPair(q.Key);
                var valuePair = GetPair(q.Value);
                if (keyPair != null && valuePair != null)
                {
                    source = source.Where(item =>
                    {
                        var fromProperty = GetPropertyValue(item, keyPair[0]);
                        var fromPropertyType = fromProperty.GetType();
                        var toProperty = GetPropertyValue(item, keyPair[1]);
                        var toPropertyType = toProperty.GetType();
                        if (fromPropertyType.Equals(typeof(DateTime)) && toPropertyType.Equals(typeof(DateTime)))
                        {
                            DateTime fromPropertyValue = DateTime.Parse(fromProperty.ToString());
                            DateTime toPropertyValue = DateTime.Parse(toProperty.ToString());
                            DateTime fromValue = DateTime.Parse(valuePair[0]);
                            DateTime toValue = DateTime.Parse(valuePair[1]);

                            // fromValue must be later than fromPropertyType and toValue must be earlier than toPropertyValue
                            return fromValue.CompareTo(fromPropertyValue) <= 0 && toValue.CompareTo(toPropertyValue) >= 0;
                        }
                        return false;
                    });
                }
                else if (q.IsFull)
                {
                    source = source.Where(item => {
                        var property = GetPropertyValue(item, q.Key);
                        if (property == null)
                        {
                            return true;
                        }
                        return property.ToString().ToLower().Equals(q.Value.ToLower());
                    });
                }
                else
                {
                    source = source.Where(item => {
                        var property = GetPropertyValue(item, q.Key);
                        if (property == null)
                        {
                            return true;
                        }
                        return property.ToString().ToLower().Contains(q.Value.ToLower());
                    });
                }
            }

            return source;
        }

        // Get the actual properties from the pair
        // This is used in case of range (e.g. DateTime range, from and to)
        private static string[] GetPair(string propertyName)
        {
            var pairWord = "pair";
            var seperator = "|:|";
            // Check if identifier that it is a pair is there
            // It must have the "pair:" keyword as first value
            if (propertyName.Length < pairWord.Length + seperator.Length || propertyName.Substring(0, pairWord.Length + seperator.Length) != pairWord + seperator)
            {
                return null;
            }

            // Get the two parts
            var pair = propertyName.Split(new[] { seperator }, StringSplitOptions.None);
            return new[]
            {
                pair[1],
                pair[2]
            };
        }
        // End for filtering

        // Helper for getting the property of a class (includes nested property)
        public static object GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                foreach (var prop in propertyName.Split('.').Select(p => obj.GetType().GetProperty(p)))
                {
                    obj = prop.GetValue(obj, null);
                }
                return obj;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}