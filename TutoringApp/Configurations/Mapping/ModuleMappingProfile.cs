using AutoMapper;
using TutoringApp.Data.Dtos.Base;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Models;

namespace TutoringApp.Configurations.Mapping
{
    public class ModuleMappingProfile : Profile
    {
        public ModuleMappingProfile()
        {
            CreateMap<Module, NamedEntityDto>();
            CreateMap<ModuleNewDto, Module>(MemberList.Source);
        }
    }
}
