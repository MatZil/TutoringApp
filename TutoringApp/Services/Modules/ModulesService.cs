using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Base;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Dtos.Users;
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
        private readonly ILogger<IModulesService> _logger;

        public ModulesService(
            IRepository<Module> modulesRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<IModulesService> logger)
        {
            _modulesRepository = modulesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IEnumerable<NamedEntityDto>> GetAll()
        { 
            var modules = await _modulesRepository.GetAll();
            var moduleDtos = _mapper.Map<IEnumerable<NamedEntityDto>>(modules);

            return moduleDtos;
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
                if (module.ModuleTutors.Any())
                {
                    var errorMessage = $"Could not delete module (id='{id}'): it contains some tutors.";
                    _logger.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                await _modulesRepository.Delete(module);
            }
        }

        public async Task<UserModuleMetadataDto> GetUserModuleMetadata(int moduleId)
        {
            var userId = _currentUserService.GetUserId();
            var module = await _modulesRepository.GetById(moduleId);

            var canResignFromTutoring = module.ModuleTutors.Any(mt => mt.TutorId == userId);
            var metadata = new UserModuleMetadataDto
            {
                CanApplyForTutoring = !canResignFromTutoring && module.TutoringApplications.All(mt => mt.StudentId != userId),
                CanResignFromTutoring = canResignFromTutoring
            };

            return metadata;
        }

        public async Task<ModuleDto> GetModule(int moduleId)
        {
            var module = await _modulesRepository.GetById(moduleId);

            if (module is null)
            {
                throw new InvalidOperationException($"Could not fetch module (id='{moduleId}')): it does not exist.");
            }

            return new ModuleDto
            {
                Name = module.Name
            };
        }
    }
}
