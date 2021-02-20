using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class GlobalSettingsRepository : BaseRepository<GlobalSetting>
    {
        public GlobalSettingsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.GlobalSettings;
        }
    }
}
