using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Users;

namespace TutoringApp.Services.Interfaces
{
    public interface ITutoringService
    {
        Task ApplyForTutoring(int moduleId, TutoringApplicationNewDto tutoringApplicationNew);
        Task ResignFromTutoring(int moduleId);
    }
}
