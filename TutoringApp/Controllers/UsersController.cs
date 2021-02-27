using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("tutors")]
        [Authorize(Roles = AppRoles.Tutor + "," + AppRoles.Student)]
        public async Task<IActionResult> GetTutors([FromQuery] int moduleId)
        {
            var tutors = await _usersService.GetTutors(moduleId);

            return Ok(tutors);
        }
    }
}
