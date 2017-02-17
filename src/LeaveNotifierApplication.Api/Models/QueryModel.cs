using System.Collections.Generic;
using System.Linq;
using LeaveNotifierApplication.Data.Extensions;

namespace LeaveNotifierApplication.Api.Models
{
    public class QueryModel<T>
    {
        // For the filter
        public string[] SearchKey { get; set; }
        public string[] SearchValue { get; set; }
        public bool[] IsFull { get; set; }

        // For the sorting
        public string SortOrder { get; set; }
        public bool IsAsc { get; set; }

        // For pagination
        private static int STARTDEFAULT = 0;
        private static int COUNTDEFAULT = 10;
        public int Start { get; set; } = STARTDEFAULT;
        public int Count { get; set; } = COUNTDEFAULT;

        public static IEnumerable<T> Query(IEnumerable<T> list, QueryModel<T> query)
        {
            return list.SortBy(query.SortOrder, query.IsAsc).Where(query.SearchKey, query.SearchValue, query.IsFull).Skip(query.Start).Take(query.Count);
        }
    }
}