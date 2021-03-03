using System.Threading.Tasks;

namespace TutoringApp.Infrastructure.Repositories.Interfaces
{
    public interface IModuleTutorsRepository
    {
        Task Delete(int moduleId, string tutorId);
    }
}
