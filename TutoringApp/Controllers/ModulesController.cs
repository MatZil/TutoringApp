using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> DeleteModule(int id)
        {
            await _modulesService.Delete(id);

            return Ok();
        }
    }
}
