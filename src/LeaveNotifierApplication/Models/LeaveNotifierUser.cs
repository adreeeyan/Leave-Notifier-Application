using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenIddict;

namespace LeaveNotifierApplication.Models
{
    public class LeaveNotifierUser : OpenIddictUser
    {
        public LeaveNotifierUser()
        {

        }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
