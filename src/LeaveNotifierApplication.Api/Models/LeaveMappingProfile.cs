using AutoMapper;
using LeaveNotifierApplication.Data.Models;

namespace LeaveNotifierApplication.Api.Models
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
