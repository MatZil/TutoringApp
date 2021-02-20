using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class GlobalSetting : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
