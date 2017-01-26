using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeaveNotifierApplication.Models
{
    public class LeaveNotifierUser : IdentityUser
    {
        public LeaveNotifierUser()
        {

        }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
