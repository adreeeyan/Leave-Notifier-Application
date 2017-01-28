using LeaveNotifierApplication.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveNotifierApplication.Models
{
    public class LeaveModel
    {
        public int Id { get; set; }
        [Required]
        public Means Means { get; set; }
        [Required]
        [MinLength(5)]
        public string Justification { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Status Status { get; set; }

        // for the user
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
