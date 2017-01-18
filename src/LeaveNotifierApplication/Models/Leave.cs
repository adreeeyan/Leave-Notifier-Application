using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Models
{
    public class Leave
    {
        public int id { get; set; }
        public Means means { get; set; }
        public string justification { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public Status status { get; set; }
        public string user { get; set; }
    }
}
