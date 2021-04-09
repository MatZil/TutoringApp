using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring.Assignments;

namespace TutoringApp.Services.Interfaces
{
    public interface IAssignmentsService
    {
        Task UpdateAssignments(int moduleId, string studentId, IFormFileCollection assignments);
        Task<IEnumerable<AssignmentDto>> GetAssignments(int moduleId, string tutorId, string studentId);
    }
}
