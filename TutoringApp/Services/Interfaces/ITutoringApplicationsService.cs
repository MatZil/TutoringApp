using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring;

namespace TutoringApp.Services.Interfaces
{
    public interface ITutoringApplicationsService
    {
        Task ApplyForTutoring(int moduleId, TutoringApplicationNewDto tutoringApplicationNew);
        Task<IEnumerable<TutoringApplicationDto>> GetTutoringApplications();
        Task<(string email, string module)> ConfirmApplication(int applicationId);
        Task<(string email, string module)> RejectApplication(int applicationId);
    }
}
