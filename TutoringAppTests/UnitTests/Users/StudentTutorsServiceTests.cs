using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Users;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Users
{
    public class StudentTutorsServiceTests
    {
        private readonly IStudentTutorsService _studentTutorsService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public StudentTutorsServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;
            _userManager = setup.UserManager;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            _studentTutorsService = new StudentTutorsService(
                new StudentTutorsRepository(setup.Context),
                _currentUserServiceMock.Object,
                new Mock<ILogger<IStudentTutorsService>>().Object,
                new ModuleTutorsRepository(setup.Context),
                new TutoringSessionsRepository(setup.Context),
                new StudentTutorIgnoresRepository(setup.Context)
            );
        }

        [Theory]
        [InlineData("matas.tutorius2@ktu.edu", 1)]
        public async Task When_AddingTutor_Expect_TutorAdded(string email, int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync(email);
            await _studentTutorsService.AddStudentTutor(tutor.Id, moduleId);

            var tutorAdded = await _context.StudentTutors.FirstAsync(st => st.TutorId == tutor.Id && st.Student.Email == "matas.zilinskas@ktu.edu");

            Assert.Equal(moduleId, tutorAdded.ModuleId);
        }

        [Theory]
        [InlineData("matas.tutorius2@ktu.edu", 2)]
        public async Task When_AddingNonExistingTutor_Expect_Exception(string email, int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _studentTutorsService.AddStudentTutor(tutor.Id, moduleId)
            );
        }

        [Theory]
        [InlineData("matas.tutorius3@ktu.edu", 2)]
        public async Task When_AddingIgnoringTutor_Expect_Exception(string email, int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _studentTutorsService.AddStudentTutor(tutor.Id, moduleId)
            );
        }

        [Theory]
        [InlineData("matas.tutorius3@ktu.edu", 2)]
        public async Task When_AddingSelf_Expect_Exception(string email, int moduleId)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == email).Id);

            var tutor = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _studentTutorsService.AddStudentTutor(tutor.Id, moduleId)
            );
        }

        [Theory]
        [InlineData("matas.tutorius1@ktu.edu", 1)]
        public async Task When_RemovingTutorAsStudent_Expect_TutorRemoved(string email, int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync(email);

            await _studentTutorsService.RemoveStudentTutor(tutor.Id, moduleId);

            var tutorExists = await _context.StudentTutors.AnyAsync(st => st.TutorId == tutor.Id && st.Student.Email == "matas.zilinskas@ktu.edu");

            Assert.False(tutorExists);
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu", 1)]
        public async Task When_RemovingStudentAsTutor_Expect_StudentRemoved(string email, int moduleId)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            var student = await _userManager.FindByEmailAsync(email);
            await _studentTutorsService.RemoveTutorStudent(student.Id, moduleId);

            var studentExists = await _context.StudentTutors.AnyAsync(st => st.StudentId == student.Id && st.Tutor.Email == "matas.tutorius1@ktu.edu");

            Assert.False(studentExists);
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_IgnoringStudent_Expect_StudentIgnored(string studentEmail)
        {
            var tutor = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            var student = await _userManager.FindByEmailAsync(studentEmail);
            await _studentTutorsService.IgnoreTutorStudent(student.Id);

            var exists = await _context.StudentTutorIgnores.AnyAsync(sti => sti.TutorId == tutor.Id && sti.StudentId == student.Id);

            Assert.True(exists);
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_IgnoringStudent_Expect_UpcomingSessionsDeleted(string studentEmail)
        {
            var tutor = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            var student = await _userManager.FindByEmailAsync(studentEmail);
            await _studentTutorsService.IgnoreTutorStudent(student.Id);

            var exists = await _context.TutoringSessions.AnyAsync(ts => ts.TutorId == tutor.Id && ts.StudentId == student.Id && ts.Status == TutoringSessionStatusEnum.Upcoming);

            Assert.False(exists);
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_IgnoringStudent_Expect_StudentTutorDeleted(string studentEmail)
        {
            var tutor = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            var student = await _userManager.FindByEmailAsync(studentEmail);
            await _studentTutorsService.IgnoreTutorStudent(student.Id);

            var exists = await _context.StudentTutors.AnyAsync(ts => ts.TutorId == tutor.Id && ts.StudentId == student.Id);

            Assert.False(exists);
        }
    }
}
