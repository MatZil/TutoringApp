using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModulesService _modulesService;

        public ModulesController(IModulesService modulesService)
        {
            _modulesService = modulesService;
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
            await _modulesService.Delete(id);

            return Ok();
        }
    }
}
