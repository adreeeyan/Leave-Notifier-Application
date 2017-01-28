using AutoMapper;
using LeaveNotifierApplication.Data.Models;

namespace LeaveNotifierApplication.Models
{
    public class LeaveMappingProfile : Profile
    {
        public LeaveMappingProfile()
        {
            CreateMap<Leave, LeaveModel>()
                .ReverseMap();

            CreateMap<LeaveNotifierUser, LeaveNotifierUserModel>()
                .ReverseMap();
        }
    }
}
