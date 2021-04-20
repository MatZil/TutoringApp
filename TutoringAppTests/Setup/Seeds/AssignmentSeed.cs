using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class AssignmentSeed
    {
        public static async Task Seed(DbSet<Assignment> assignmentSet, UserManager<AppUser> userManager)
        {
            var assignments = new List<Assignment>
            {
                new Assignment
                {
                    AssignmentFileName = "First.pdf",
                    ModuleId = 1,
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    SubmissionEvaluation = 8,
                    SubmissionFileName = "Submit.doc"
                },
                new Assignment
                {
                    AssignmentFileName = "Second.pdf",
                    ModuleId = 1,
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    SubmissionFileName = "Submit.doc"
                },
                new Assignment
                {
                    AssignmentFileName = "Third.pdf",
                    ModuleId = 1,
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu")
                }
            };

            await assignmentSet.AddRangeAsync(assignments);
        }
    }
}
