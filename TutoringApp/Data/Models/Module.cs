using System.Collections.Generic;
using TutoringApp.Data.Models.Base;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringApp.Data.Models
{
    public class Module : BaseEntity
    {
        public string Name { get; set; }

        public IList<ModuleTutor> ModuleTutors { get; set; }
    }
}
