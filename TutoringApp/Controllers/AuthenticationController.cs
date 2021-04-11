using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistration)
        {
            try
            {
                var userId = await _authService.Register(userRegistration);

                await _authService.SendConfirmationEmail(userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            try
            {
                var response = await _authService.Login(userLogin);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto emailConfirmation)
        {
            try
            {
                await _authService.ConfirmEmail(emailConfirmation.Email, emailConfirmation.EncodedToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
