﻿using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Dtos.Users
{
    public class StudentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StudentCycleEnum StudentCycle { get; set; }
        public StudentYearEnum StudentYear { get; set; }
        public string Faculty { get; set; }
        public string StudyBranch { get; set; }
    }
}
