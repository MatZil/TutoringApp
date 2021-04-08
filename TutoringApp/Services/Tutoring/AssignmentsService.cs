using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<Assignment> _assignmentsRepository;
        private readonly ITimeService _timeService;

        private const string AssignmentsRoot = "Assignments/";

        public AssignmentsService(
            ICurrentUserService currentUserService,
            IRepository<Assignment> assignmentsRepository,
            ITimeService timeService)
        {
            _currentUserService = currentUserService;
            _assignmentsRepository = assignmentsRepository;
            _timeService = timeService;
        }

        public async Task UpdateAssignments(int moduleId, string studentId, IFormFileCollection assignments)
        {
            var tutorId = _currentUserService.GetUserId();

            Directory.CreateDirectory($"{AssignmentsRoot}{tutorId}");
            Directory.CreateDirectory($"{AssignmentsRoot}{tutorId}/{studentId}");

            var assignmentEntities = new List<Assignment>();
            foreach (var assignment in assignments)
            {
                await using var stream = new FileStream($"{AssignmentsRoot}{tutorId}/{studentId}/{assignment.FileName}", FileMode.Create);

                await assignment.CopyToAsync(stream);

                assignmentEntities.Add(new Assignment
                {
                    AssignmentFileName = assignment.FileName,
                    CreationDate = _timeService.GetCurrentTime(),
                    StudentId = studentId,
                    TutorId = tutorId
                });
            }

            await _assignmentsRepository.CreateMany(assignmentEntities);
        }
    }
}
