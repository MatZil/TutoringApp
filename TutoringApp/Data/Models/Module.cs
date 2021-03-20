using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TutoringApp.Data.Models.Base;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringApp.Data.Models
{
    public class Module : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public IList<ModuleTutor> ModuleTutors { get; set; }
        public IList<TutoringApplication> TutoringApplications { get; set; }
        public IList<StudentTutor> StudentTutors { get; set; }
        public IList<ChatMessage> ChatMessages { get; set; }
    }
}
