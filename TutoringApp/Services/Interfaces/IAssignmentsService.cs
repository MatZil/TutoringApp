using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring.Assignments;

namespace TutoringApp.Services.Interfaces
{
    public interface IAssignmentsService
    {
        Task UploadAssignments(int moduleId, string studentId, IFormFileCollection formFiles);
        Task<IEnumerable<AssignmentDto>> GetAssignments(int moduleId, string tutorId, string studentId);
        Task UploadSubmission(int assignmentId, IFormFileCollection formFiles);
        Task EvaluateSubmission(int assignmentId, int evaluation);
        Task DeleteAssignment(int assignmentId);
    }
}
