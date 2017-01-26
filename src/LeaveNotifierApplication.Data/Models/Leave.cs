using System;

namespace LeaveNotifierApplication.Data.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public Means Means { get; set; }
        public string Justification { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Status Status { get; set; }
        public LeaveNotifierUser User { get; set; }
    }
}
