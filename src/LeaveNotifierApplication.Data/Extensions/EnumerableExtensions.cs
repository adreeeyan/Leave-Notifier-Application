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

            foreach(var q in query){
                if(q.IsFull){
                    source = source.Where(item => GetPropertyValue(item, q.Key).ToString().ToLower().Equals(q.Value.ToLower()));  
                }else{
                    source = source.Where(item => GetPropertyValue(item, q.Key).ToString().ToLower().Contains(q.Value.ToLower()));                    
                }
            }

            return source;
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