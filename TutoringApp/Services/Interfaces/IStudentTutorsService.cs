using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IStudentTutorsService
    {
        Task AddStudentTutor(string tutorId);
        Task RemoveStudentTutor(string tutorId);
        Task RemoveTutorStudent(string studentId);
    }
}
