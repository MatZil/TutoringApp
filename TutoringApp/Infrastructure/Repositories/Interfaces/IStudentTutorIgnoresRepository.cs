using System.Threading.Tasks;

namespace TutoringApp.Infrastructure.Repositories.Interfaces
{
    public interface IStudentTutorIgnoresRepository
    {
        Task<bool> TutorIgnoresStudent(string tutorId, string studentId);
    }
}
