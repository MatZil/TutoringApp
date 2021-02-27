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
        private readonly ICurrentUserService _currentUserService;

        public ModulesService(
            IRepository<Module> modulesRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _modulesRepository = modulesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<NamedEntityDto>> GetAll()
        {
            var userRole = _currentUserService.GetRole(); // TODO: filter for lecturers.
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
