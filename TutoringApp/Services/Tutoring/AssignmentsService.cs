using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class AssignmentsService : IAssignmentsService
    {
        public async Task UpdateAssignments(int moduleId, string studentId, IFormFileCollection assignments)
        {
            foreach (var assignment in assignments)
            {
                using (var stream = new FileStream($"Assignments/{assignment.FileName}", FileMode.Create))
                {
                    await assignment.CopyToAsync(stream);
                }
            }
        }
    }
}
