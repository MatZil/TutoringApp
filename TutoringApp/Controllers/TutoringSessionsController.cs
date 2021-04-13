using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = AppRoles.Student)]
    public class TutoringSessionsController : ControllerBase
    {
        private readonly ITutoringSessionsService _tutoringSessionsService;
        private readonly IEmailService _emailService;

        public TutoringSessionsController(
            ITutoringSessionsService tutoringSessionsService,
            IEmailService emailService)
        {
            _tutoringSessionsService = tutoringSessionsService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTutoringSessions()
        {
            try
            {
                var sessions = await _tutoringSessionsService.GetTutoringSessions();

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("learning")]
        public async Task<IActionResult> GetLearningSessions()
        {
            try
            {
                var sessions = await _tutoringSessionsService.GetLearningSessions();

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTutoringSession([FromBody] TutoringSessionNewDto tutoringSessionNew)
        {
            try
            {
                await _tutoringSessionsService.CreateTutoringSession(tutoringSessionNew);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelTutoringSession(int id, [FromBody] TutoringSessionCancelDto tutoringSessionCancel)
        {
            try
            {
                await _tutoringSessionsService.CancelTutoringSession(id, tutoringSessionCancel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/invert")]
        public async Task<IActionResult> InvertTutoringSession(int id)
        {
            try
            {
                await _tutoringSessionsService.InvertTutoringSessionSubscription(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/evaluate")]
        public async Task<IActionResult> EvaluateTutoringSession(int id, [FromBody] TutoringSessionEvaluationDto tutoringEvaluation)
        {
            try
            {
                var aggregate = await _tutoringSessionsService.EvaluateTutoringSession(id, tutoringEvaluation);

                Response.OnCompleted(async () =>
                    await _emailService.SendTutoringSessionEvaluatedEmail(aggregate)
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
