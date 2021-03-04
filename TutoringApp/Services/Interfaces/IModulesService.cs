using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Base;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Dtos.Users;

namespace TutoringApp.Services.Interfaces
{
    public interface IModulesService
    {
        Task<IEnumerable<NamedEntityDto>> GetAll();
        Task<int> Create(ModuleNewDto moduleNew);
        Task Delete(int id);
        Task<UserModuleMetadataDto> GetUserModuleMetadata(int moduleId);
    }
}
