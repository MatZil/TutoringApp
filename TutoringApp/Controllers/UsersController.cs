using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Chats;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;
        private readonly IChatsService _chatsService;

        public UsersController(
            IUsersService usersService, 
            IEmailService emailService, 
            IChatsService chatsService)
        {
            _usersService = usersService;
            _emailService = emailService;
            _chatsService = chatsService;
        }

        [HttpGet("tutors")]
        [Authorize(Roles = AppRoles.Student + "," + AppRoles.Admin)]
        public async Task<IActionResult> GetTutors([FromQuery] int moduleId)
        {
            var tutors = await _usersService.GetTutors(moduleId);

            return Ok(tutors);
        }

        [HttpGet("students")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> GetStudents([FromQuery] int moduleId)
        {
            var students = await _usersService.GetStudents(moduleId);

            return Ok(students);
        }

        [HttpGet("unconfirmed")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> GetUnconfirmedUsers()
        {
            var unconfirmedUsers = await _usersService.GetUnconfirmedUsers();

            return Ok(unconfirmedUsers);
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> ConfirmUser(string id)
        {
            try
            {
                var email = await _usersService.ConfirmUser(id);

                Response.OnCompleted(async () =>
                    await _emailService.SendUserConfirmedEmail(email));

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> RejectUser(string id, [FromQuery] string rejectionReason)
        {
            try
            {
                var email = await _usersService.RejectUser(id);

                Response.OnCompleted(async () =>
                    await _emailService.SendUserRejectedEmail(email, rejectionReason));

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Chats

        [HttpPost("{userId}/chat-messages")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> PostMessage(string userId, [FromBody] ChatMessageNewDto chatMessageNew)
        {
            try
            {
                var chatMessage = await _chatsService.PostChatMessage(userId, chatMessageNew);

                return Ok(chatMessage);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/chat-messages")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> GetMessages(string userId)
        {
            try
            {
                var chatMessage = await _chatsService.GetChatMessages(userId);

                return Ok(chatMessage);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
