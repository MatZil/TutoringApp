using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class EmailTemplate : BaseEntity
    {
        public string Purpose { get; set; }
        public string Content { get; set; }
    }
}
