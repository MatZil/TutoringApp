using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IUsersService
    {
        Task<string> GetRole(string userId);
    }
}
