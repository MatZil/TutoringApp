namespace TutoringApp.Data.Models.JoiningTables
{
    public class ModuleTutor
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public Module Module { get; set; }
        public int ModuleId { get; set; }
    }
}
