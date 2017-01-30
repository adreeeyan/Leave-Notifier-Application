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
            return source.OrderBy(t => GetPropertyValue(t, param));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string param)
        {
            return source.OrderByDescending(t => GetPropertyValue(t, param));
        }
        // End for sorting

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