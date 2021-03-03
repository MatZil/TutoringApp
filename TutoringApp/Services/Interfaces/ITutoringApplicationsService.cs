using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring;

namespace TutoringApp.Services.Interfaces
{
    public interface ITutoringApplicationsService
    {
        Task ApplyForTutoring(int moduleId, TutoringApplicationNewDto tutoringApplicationNew);
        Task ResignFromTutoring(int moduleId);
        Task<IEnumerable<TutoringApplicationDto>> GetTutoringApplications();
    }
}
