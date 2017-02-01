using System;
using OpenIddict;

namespace LeaveNotifierApplication.Data.Models
{
    public class LeaveNotifierUser : OpenIddictUser
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
