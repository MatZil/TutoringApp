using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Base;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Modules
{
    public class ModulesService : IModulesService
    {
        private readonly IRepository<Module> _modulesRepository;
        private readonly IMapper _mapper;

        public ModulesService(
            IRepository<Module> modulesRepository,
            IMapper mapper)
        {
            _modulesRepository = modulesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NamedEntityDto>> GetAll()
        {
            var modules = await _modulesRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<NamedEntityDto>>(modules);

            return dtos;
        }

        public async Task<int> Create(ModuleNewDto moduleNew)
        {
            var module = _mapper.Map<Module>(moduleNew);
            var id = await _modulesRepository.Create(module);

            return id;
        }

        public async Task Delete(int id)
        {
            var module = await _modulesRepository.GetById(id);

            if (module != null)
            {
                await _modulesRepository.Delete(module);
            }
        }
    }
}
