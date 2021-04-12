using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IStudentTutorsService
    {
        Task AddStudentTutor(string tutorId, int moduleId);
        Task RemoveStudentTutor(string tutorId, int moduleId);
        Task RemoveTutorStudent(string studentId, int moduleId);
        Task IgnoreTutorStudent(string studentId);
    }
}
