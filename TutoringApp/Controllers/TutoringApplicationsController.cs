using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Authorize(Roles = AppRoles.Admin)]
    [Route("api/[controller]")]
    public class TutoringApplicationsController : ControllerBase
    {
        private readonly ITutoringApplicationsService _tutoringApplicationsService;
        private readonly IEmailService _emailService;

        public TutoringApplicationsController(
            ITutoringApplicationsService tutoringApplicationsService,
            IEmailService emailService)
        {
            _tutoringApplicationsService = tutoringApplicationsService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTutoringApplications()
        {
            var tutoringApplications = await _tutoringApplicationsService.GetTutoringApplications();

            return Ok(tutoringApplications);
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmTutoringApplication(int id)
        {
            try
            {
                var email = await _tutoringApplicationsService.ConfirmApplication(id);

                Response.OnCompleted(async () =>
                    await _emailService.SendTutoringApplicationConfirmedEmail(email)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectTutoringApplication(int id)
        {
            try
            {
                var email = await _tutoringApplicationsService.RejectApplication(id);

                Response.OnCompleted(async () =>
                    await _emailService.SendTutoringApplicationRejectedEmail(email)
                );

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
