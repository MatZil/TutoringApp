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
        private readonly IStudentTutorsService _studentTutorsService;
        private readonly IAssignmentsService _assignmentsService;

        public ModulesController(
            IModulesService modulesService,
            ITutoringApplicationsService tutoringApplicationsService,
            IUsersService usersService,
            IStudentTutorsService studentTutorsService, 
            IAssignmentsService assignmentsService)
        {
            _modulesService = modulesService;
            _tutoringApplicationsService = tutoringApplicationsService;
            _usersService = usersService;
            _studentTutorsService = studentTutorsService;
            _assignmentsService = assignmentsService;
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

        #region Student Tutors

        [HttpPost("{moduleId}/tutors/{tutorId}")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> AddStudentTutor(int moduleId, string tutorId)
        {
            try
            {
                await _studentTutorsService.AddStudentTutor(tutorId, moduleId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{moduleId}/tutors/{tutorId}")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> RemoveStudentTutor(int moduleId, string tutorId)
        {
            await _studentTutorsService.RemoveStudentTutor(tutorId, moduleId);

            return Ok();
        }

        [HttpDelete("{moduleId}/students/{studentId}")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<IActionResult> RemoveTutorStudent(int moduleId, string studentId)
        {
            await _studentTutorsService.RemoveTutorStudent(studentId, moduleId);

            return Ok();
        }

        #endregion

        #region Assignments
        [HttpPatch("{moduleId}/students/{studentId}/assignments")]
        public async Task<IActionResult> UploadAssignments(int moduleId, string studentId)
        {
            try
            {
                var formFiles = Request.Form.Files;
                await _assignmentsService.UploadAssignments(moduleId, studentId, formFiles);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{moduleId}/tutors/{tutorId}/students/{studentId}/assignments")]
        public async Task<IActionResult> GetAssignments(int moduleId, string tutorId, string studentId)
        {
            try
            {
                var assignments = await _assignmentsService.GetAssignments(moduleId, tutorId, studentId);

                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assignments/{assignmentId}/submit")]
        public async Task<IActionResult> UploadSubmission(int assignmentId)
        {
            try
            {
                var formFiles = Request.Form.Files;
                await _assignmentsService.UploadSubmission(assignmentId, formFiles);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("assignments/{assignmentId}/evaluate")]
        public async Task<IActionResult> EvaluateSubmission(int assignmentId, [FromQuery] int evaluation)
        {
            try
            {
                await _assignmentsService.EvaluateSubmission(assignmentId, evaluation);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("assignments/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(int assignmentId)
        {
            try
            {
                await _assignmentsService.DeleteAssignment(assignmentId);

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
