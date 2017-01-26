using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeaveNotifierApplication.Data.Models
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
