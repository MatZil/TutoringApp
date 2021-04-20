using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
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
    public class UsersServiceTests
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public UsersServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;
            _userManager = setup.UserManager;
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetRole())
                .Returns(AppRoles.Student);

            _usersService = new UsersService(
                setup.UserManager,
                _currentUserServiceMock.Object,
                UnitTestSetup.Mapper,
                new ModuleTutorsRepository(setup.Context)
            );
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu", AppRoles.Student)]
        [InlineData("matas.admin@ktu.edu", AppRoles.Admin)]
        public async Task When_GettingUserRole_Expect_CorrectRole(string email, string expectedRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var actualRole = await _usersService.GetRole(user.Id);

            Assert.Equal(expectedRole, actualRole);
        }

        [Theory]
        [InlineData(1)]
        public async Task When_GettingModuleTutors_Expect_CorrectTutors(int moduleId)
        {
            var tutors = await _usersService.GetTutors(moduleId);

            Assert.Collection(tutors,
                t => Assert.Equal("Matas FirstTutor", t.Name),
                t => Assert.Equal("Matas SecondTutor", t.Name)
            );
        }

        [Fact]
        public async Task When_GettingUnconfirmedUsers_Expect_CorrectUsers()
        {
            var actualUsers = await _usersService.GetUnconfirmedUsers();

            Assert.Collection(actualUsers,
                u => Assert.Equal("matas.unconfirmed@ktu.edu", u.Email)
            );
        }

        [Theory]
        [InlineData("matas.unconfirmed@ktu.edu")]
        public async Task When_ConfirmingUser_Expect_UserConfirmed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var actualEmail = await _usersService.ConfirmUser(user.Id);

            Assert.Equal(email, actualEmail);
            Assert.True(user.IsConfirmed);
        }

        [Theory]
        [InlineData("non-existing@email.com")]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_ConfirmingInvalidUser_Expect_Exception(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _usersService.ConfirmUser(user?.Id)
            );
        }

        [Theory]
        [InlineData("matas.unconfirmed@ktu.edu")]
        public async Task When_RejectingUser_Expect_UserRejected(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // ReSharper disable once UseDeconstruction
            var actualEmail = await _usersService.RejectUser(user.Id);

            Assert.Equal(email, actualEmail);

            var userDeleted = await _userManager.FindByEmailAsync(email);
            Assert.Null(userDeleted);
        }

        [Theory]
        [InlineData("non-existing@email.com")]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_RejectingInvalidUser_Expect_Exception(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _usersService.RejectUser(user?.Id)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task When_ResigningFromTutoring_Expect_ModuleTutorRemoved(int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync("matas.tutorius2@ktu.edu");
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            await _usersService.ResignFromTutoring(moduleId);

            var moduleTutorRemoved = await _context.ModuleTutors.FirstOrDefaultAsync(mt => mt.ModuleId == moduleId && mt.TutorId == tutor.Id);
            Assert.Null(moduleTutorRemoved);
        }

        [Theory]
        [InlineData("matas.tutorius1@ktu.edu", 1)]
        public async Task When_GettingStudents_Expect_CorrectStudents(string tutorEmail, int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync(tutorEmail);
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            var actualStudents = await _usersService.GetStudents(moduleId);

            Assert.Collection(actualStudents, student =>
            {
                Assert.Equal("Matas Zilinskas", student.Name);
                Assert.Equal("Informatics", student.Faculty);
                Assert.Equal("Software Systems", student.StudyBranch);
                Assert.Equal(StudentCycleEnum.Bachelor, student.StudentCycle);
                Assert.Equal(StudentYearEnum.FourthYear, student.StudentYear);
            });
        }

        [Fact]
        public async Task When_GettingUser_Expect_CorrectUser()
        {
            var user = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");

            var actualUser = await _usersService.GetUser(user.Id);
            Assert.Equal("Matas FirstTutor", actualUser.Name);
        }


        [Fact]
        public async Task When_GettingIncorrectUser_Expect_Exception()
        {
            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await _usersService.GetUser("don't exist")
            );
        }
    }
}
