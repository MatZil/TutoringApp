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
        private readonly IStudentTutorsService _studentTutorsService;

        public UsersController(
            IUsersService usersService, 
            IEmailService emailService, 
            IChatsService chatsService,
            IStudentTutorsService studentTutorsService)
        {
            _usersService = usersService;
            _emailService = emailService;
            _chatsService = chatsService;
            _studentTutorsService = studentTutorsService;
        }

        [HttpGet("tutors")]
        [Authorize(Roles = AppRoles.Student + "," + AppRoles.Admin)]
        public async Task<IActionResult> GetTutors([FromQuery] int moduleId)
        {
            var tutors = await _usersService.GetTutors(moduleId);

            return Ok(tutors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _usersService.GetUser(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPost("{studentId}/ignore")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> IgnoreStudent(string studentId)
        {
            try
            {
                await _studentTutorsService.IgnoreTutorStudent(studentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{studentId}/unignore")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> UnignoreStudent(string studentId)
        {
            try
            {
                await _studentTutorsService.UnignoreStudent(studentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ignored")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> GetIgnoredStudents()
        {
            try
            {
                var students = await _studentTutorsService.GetIgnoredStudents();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("student-tutor-exists")]
        public async Task<IActionResult> StudentTutorExists([FromQuery] string studentId, [FromQuery] string tutorId)
        {
            try
            {
                var exists = await _studentTutorsService.StudentTutorExists(studentId, tutorId);

                return Ok(exists);
            }
            catch (Exception ex)
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
                await _chatsService.PostChatMessage(userId, chatMessageNew);

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/chat-messages")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> GetMessages(string userId, [FromQuery] int moduleId)
        {
            try
            {
                var chatMessage = await _chatsService.GetChatMessages(userId, moduleId);

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
