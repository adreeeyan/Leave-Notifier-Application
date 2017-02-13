using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeaveNotifierApplication.Data.Models
{
    public class LeaveNotifierUser : IdentityUser
    {
        public LeaveNotifierUser()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
