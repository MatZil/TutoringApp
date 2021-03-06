﻿using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models.JoiningTables
{
    public class StudentTutor : BaseEntity
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public Module Module { get; set; }
        public int ModuleId { get; set; }
    }
}
