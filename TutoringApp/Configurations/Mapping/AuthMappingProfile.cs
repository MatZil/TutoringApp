using AutoMapper;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Data.Models;

namespace TutoringApp.Configurations.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UserRegistrationDto, AppUser>(MemberList.Source)
                .ForSourceMember(t => t.Password, d => d.DoNotValidate())
                .ForMember(d => d.UserName, o => o.MapFrom(t => t.Email));
        }
    }
}
