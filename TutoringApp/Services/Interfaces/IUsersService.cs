using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Users;

namespace TutoringApp.Services.Interfaces
{
    public interface IUsersService
    {
        Task<string> GetRole(string userId);
        Task<IEnumerable<TutorDto>> GetTutors(int moduleId);
    }
}
