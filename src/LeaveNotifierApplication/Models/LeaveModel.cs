using LeaveNotifierApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Models
{
    public class LeaveModel
    {
        public Means Means { get; set; }
        public string Justification { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Status Status { get; set; }

        // for the user
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
