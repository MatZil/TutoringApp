using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Tutoring;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Tutoring
{
    public class AssignmentsServiceTests
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public AssignmentsServiceTests()
        {
            var setup = new UnitTestSetup();

            _userManager = setup.UserManager;
            _context = setup.Context;

            var filesServiceMock = new Mock<IFilesService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            _assignmentsService = new AssignmentsService(
                _currentUserServiceMock.Object,
                new AssignmentsRepository(setup.Context),
                new StudentTutorsRepository(setup.Context),
                filesServiceMock.Object
            );
        }

        [Fact]
        public async Task When_UploadingAssignments_Expect_AssignmentCreated()
        {
            var student = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");
            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "any", "assignment.pdf")
            };

            await _assignmentsService.UploadAssignments(1, student.Id, formFiles);

            var assignment = await _context.Assignments.FirstAsync(a => a.Id == 4);

            Assert.Equal("assignment.pdf", assignment.AssignmentFileName);
        }

        [Fact]
        public async Task When_UploadingAssignmentsAsNotTutor_Expect_Exception()
        {
            var student = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");
            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "any", "assignment.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadAssignments(2, student.Id, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingExistingAssignments_Expect_Exception()
        {
            var student = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");
            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "any", "First.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadAssignments(1, student.Id, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingTooLargeAssignments_Expect_Exception()
        {
            var student = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");
            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 66666666, "any", "any.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadAssignments(1, student.Id, formFiles)
            );
        }

        [Fact]
        public async Task When_GettingAssignments_Expect_CorrectAssignments()
        {
            var assignments = await _assignmentsService.GetAssignments(1,
                _userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id,
                _userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id
            );

            Assert.Collection(assignments,
                a => Assert.Equal("First.pdf", a.FileName),
                a => Assert.Equal("Second.pdf", a.FileName),
                a => Assert.Equal("Third.pdf", a.FileName)
                );
        }

        [Fact]
        public async Task When_UploadingSubmission_Expect_SubmissionUploaded()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "any", "submit.pdf")
            };

            await _assignmentsService.UploadSubmission(3, formFiles);

            var assignment = await _context.Assignments.FirstAsync(a => a.Id == 3);

            Assert.Equal("submit.pdf", assignment.SubmissionFileName);
        }

        [Fact]
        public async Task When_UploadingManySubmissionFiles_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "any", "submit.pdf"),
                new FormFile(Stream.Null, 0, 0, "any", "submit.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
               await _assignmentsService.UploadSubmission(3, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingSubmissionTooBig_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 66666666, "any", "submit.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadSubmission(3, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingSubmissionToNonExistingAssignment_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 666, "any", "submit.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadSubmission(99, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingSubmissionAsTutor_Expect_Exception()
        {
            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 666, "any", "submit.pdf")
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.UploadSubmission(3, formFiles)
            );
        }

        [Fact]
        public async Task When_UploadingSubmissionToEvaluatedAssignment_Expect_EvaluationReset()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var formFiles = new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 666, "any", "submit-edit.pdf")
            };

            await _assignmentsService.UploadSubmission(1, formFiles);

            var updatedAssignment = await _context.Assignments.FirstAsync(a => a.Id == 1);

            Assert.Null(updatedAssignment.SubmissionEvaluation);
            Assert.Equal("submit-edit.pdf", updatedAssignment.SubmissionFileName);
        }

        [Fact]
        public async Task When_EvaluatingSubmission_Expect_SubmissionEvaluated()
        {
            await _assignmentsService.EvaluateSubmission(2, 6);

            var assignment = await _context.Assignments.FirstAsync(a => a.Id == 2);

            Assert.Equal(6, assignment.SubmissionEvaluation);
        }

        [Fact]
        public async Task When_EvaluatingNonExistingSubmission_Expect_Exception()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.EvaluateSubmission(99, 6)
            );
        }

        [Fact]
        public async Task When_EvaluatingSubmissionAsStudent_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await _assignmentsService.EvaluateSubmission(2, 6)
            );
        }

        [Theory]
        [InlineData(11)]
        [InlineData(-1)]
        public async Task When_EvaluatingSubmissionIncorrectly_Expect_Exception(int evaluation)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.EvaluateSubmission(2, evaluation)
            );
        }

        [Fact]
        public async Task When_DeletingAssignment_Expect_AssignmentDeleted()
        {
            await _assignmentsService.DeleteAssignment(2);

            var exists = await _context.Assignments.AnyAsync(a => a.Id == 2);
            Assert.False(exists);
        }

        [Fact]
        public async Task When_DeletingNonExistingAssignment_Expect_NoException()
        {
            await _assignmentsService.DeleteAssignment(99);
        }

        [Fact]
        public async Task When_DeletingAssignmentAsStudent_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.DeleteAssignment(2)
            );
        }

        [Theory]
        [InlineData("First.pdf")]
        [InlineData("Submit.doc")]
        public async Task When_DownloadingFile_Expect_NoException(string fileName)
        {
            await _assignmentsService.DownloadAssignmentFile(1, fileName);
        }

        [Fact]
        public async Task When_DownloadingNonExistingAssignment_Expect_Exception()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.DownloadAssignmentFile(99, "any")
            );
        }

        [Fact]
        public async Task When_DownloadingForeignAssignment_Expect_Exception()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu").Id);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.DownloadAssignmentFile(1, "First.pdf")
            );
        }

        [Fact]
        public async Task When_DownloadingNonExistingAssignmentFile_Expect_Exception()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _assignmentsService.DownloadAssignmentFile(1, "any")
            );
        }
    }
}
