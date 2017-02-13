namespace LeaveNotifierApplication.Models
{
    public class QueryModel
    {
        // For the filter
        public string[] SearchKey { get; set; }
        public string[] SearchValue { get; set; }
        public bool[] IsFull { get; set; }

        // For the sorting
        public string SortOrder { get; set; }
        public bool IsAsc { get; set; }
    }
}