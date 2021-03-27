﻿using System;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringApp.Services.Tutoring;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Tutoring
{
    public class TutoringSessionsServiceTests
    {
        private readonly ITutoringSessionsService _tutoringSessionsService;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TutoringSessionsServiceTests()
        {
            var setup = new UnitTestSetup();

            _userManager = setup.UserManager;
            _context = setup.Context;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            _tutoringSessionsService = new TutoringSessionsService(
                new TutoringSessionsRepository(setup.Context),
                setup.UserManager,
                _currentUserServiceMock.Object,
                new TimeService()
            );
        }

        [Fact]
        public async Task When_GettingTutoringSessions_Expect_CorrectSessions()
        {
            var sessions = await _tutoringSessionsService.GetTutoringSessions();

            Assert.Collection(sessions,
                s => Assert.Equal(TutoringSessionStatusEnum.Upcoming, s.Status),
                s => Assert.Equal(TutoringSessionStatusEnum.Finished, s.Status),
                s => Assert.Equal(TutoringSessionStatusEnum.Cancelled, s.Status)
                );
        }

        [Fact]
        public async Task When_GettingLearningSessions_Expect_CorrectSessions()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var sessions = await _tutoringSessionsService.GetLearningSessions();

            Assert.Collection(sessions,
                s => Assert.Equal(TutoringSessionStatusEnum.Upcoming, s.Status),
                s => Assert.Equal(TutoringSessionStatusEnum.Finished, s.Status),
                s => Assert.Equal(TutoringSessionStatusEnum.Cancelled, s.Status)
            );
        }

        [Fact]
        public async Task When_CreatingTutoringSession_Expect_SessionCreated()
        {
            var tutoringSessionNew = new TutoringSessionNewDto
            {
                IsSubscribed = true,
                ModuleId = 1,
                SessionDate = DateTimeOffset.Now.AddDays(3),
                StudentId = _userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id
            };

            await _tutoringSessionsService.CreateTutoringSession(tutoringSessionNew);

            var sessionCreated = await _context.TutoringSessions.FirstAsync(ts => ts.SessionDate.Date == DateTimeOffset.Now.AddDays(3).Date);

            Assert.True(sessionCreated.IsSubscribed);
        }

        [Fact]
        public async Task When_CreatingTutoringSessionForNotStudent_Expect_Exception()
        {
            var tutoringSessionNew = new TutoringSessionNewDto
            {
                IsSubscribed = true,
                ModuleId = 1,
                SessionDate = DateTimeOffset.Now.AddDays(3),
                StudentId = _userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu").Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _tutoringSessionsService.CreateTutoringSession(tutoringSessionNew)
            );
        }

        [Fact]
        public async Task When_CreatingTutoringSessionWithPastDate_Expect_Exception()
        {
            var tutoringSessionNew = new TutoringSessionNewDto
            {
                IsSubscribed = true,
                ModuleId = 1,
                SessionDate = DateTimeOffset.Now.AddDays(-3),
                StudentId = _userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _tutoringSessionsService.CreateTutoringSession(tutoringSessionNew)
            );
        }

        [Fact]
        public async Task When_CancellingTutoringSession_Expect_SessionCancelled()
        {
            await _tutoringSessionsService.CancelTutoringSession(3);

            var session = await _context.TutoringSessions.FirstAsync(ts => ts.Id == 3);

            Assert.Equal(TutoringSessionStatusEnum.Cancelled, session.Status);
            Assert.Equal(DateTimeOffset.Now.Date, session.StatusChangeDate.GetValueOrDefault().Date);
        }

        [Fact]
        public async Task When_InvertingSessionSubscription_Expect_SubscriptionInverted()
        {
            await _tutoringSessionsService.InvertTutoringSessionSubscription(3);

            var session = await _context.TutoringSessions.FirstAsync(ts => ts.Id == 3);

            Assert.False(session.IsSubscribed);
        }

        [Fact]
        public async Task When_EvaluatingTutoringSession_Expect_SessionEvaluated()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            var evaluation = new TutoringSessionEvaluationDto
            {
                Evaluation = TutoringSessionEvaluationEnum.Good,
                Comment = "Lacked charisma"
            };

            await _tutoringSessionsService.EvaluateTutoringSession(2, evaluation);

            var session = await _context.TutoringSessions.FirstAsync(ts => ts.Id == 2);

            Assert.Equal(TutoringSessionEvaluationEnum.Good, session.Evaluation);
            Assert.Equal("Lacked charisma", session.EvaluationComment);
        }
    }
}
