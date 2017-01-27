using AutoMapper;
using LeaveNotifierApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Models
{
    public class LeaveMappingProfile : Profile
    {
        public LeaveMappingProfile()
        {
            CreateMap<Leave, LeaveModel>();
        }
    }
}
