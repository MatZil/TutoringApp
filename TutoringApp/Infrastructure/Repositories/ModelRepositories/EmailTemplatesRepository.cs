using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class EmailTemplatesRepository : BaseRepository<EmailTemplate>
    {
        public EmailTemplatesRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.EmailTemplates;
        }
    }
}
