using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Base;
using TutoringApp.Data.Dtos.Modules;

namespace TutoringApp.Services.Interfaces
{
    public interface IModulesService
    {
        Task<IEnumerable<NamedEntityDto>> GetAll();
        Task<int> Create(ModuleNewDto moduleNew);
        Task Delete(int id);
    }
}
