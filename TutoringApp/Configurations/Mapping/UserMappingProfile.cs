using AutoMapper;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;

namespace TutoringApp.Configurations.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AppUser, StudentDto>()
                .ForMember(d => d.Name, o => o.MapFrom(t => $"{t.FirstName} {t.LastName}"))
                ;
        }
    }
}
