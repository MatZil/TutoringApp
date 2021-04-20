using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Users;

namespace TutoringApp.Services.Interfaces
{
    public interface IStudentTutorsService
    {
        Task AddStudentTutor(string tutorId, int moduleId);
        Task RemoveStudentTutor(string tutorId, int moduleId);
        Task RemoveTutorStudent(string studentId, int moduleId);
        Task IgnoreTutorStudent(string studentId);
        Task<IEnumerable<IgnoredStudentDto>> GetIgnoredStudents();
        Task UnignoreStudent(string studentId);
        Task<bool> StudentTutorExists(string studentId, string tutorId);
    }
}
