using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IAssignmentsService
    {
        Task UpdateAssignments(int moduleId, string studentId, IFormFileCollection assignments);
    }
}
