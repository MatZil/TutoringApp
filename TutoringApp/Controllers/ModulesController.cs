using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Dtos.Tutoring;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModulesService _modulesService;
        private readonly ITutoringApplicationsService _tutoringApplicationsService;
        private readonly IUsersService _usersService;

        public ModulesController(
            IModulesService modulesService,
            ITutoringApplicationsService tutoringApplicationsService,
            IUsersService usersService)
        {
            _modulesService = modulesService;
            _tutoringApplicationsService = tutoringApplicationsService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _modulesService.GetAll();

            return Ok(modules);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> CreateModule([FromBody] ModuleNewDto moduleNew)
        {
            try
            {
                var id = await _modulesService.Create(moduleNew);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                await _modulesService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{moduleId}/metadata")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> GetUserMetadata(int moduleId)
        {
            var metadata = await _modulesService.GetUserModuleMetadata(moduleId);

            return Ok(metadata);
        }

        #region Tutoring applications

        [HttpPost("{moduleId}/apply")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> ApplyForTutoring(int moduleId, [FromBody] TutoringApplicationNewDto tutoringApplicationNew)
        {
            try
            {
                await _tutoringApplicationsService.ApplyForTutoring(moduleId, tutoringApplicationNew);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{moduleId}/resign")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> ResignFromTutoring(int moduleId)
        {
            try
            {
                await _usersService.ResignFromTutoring(moduleId);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
