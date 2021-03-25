using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class TutoringSessionsService : ITutoringSessionsService
    {
        private readonly IRepository<TutoringSession> _tutoringSessionsRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITimeService _timeService;

        public TutoringSessionsService(
            IRepository<TutoringSession> tutoringSessionsRepository,
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ITimeService timeService)
        {
            _tutoringSessionsRepository = tutoringSessionsRepository;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _timeService = timeService;
        }

        public async Task<IEnumerable<TutoringSessionDto>> GetTutoringSessions()
        {
            var currentUserId = _currentUserService.GetUserId();
            var tutoringSessions = await _tutoringSessionsRepository.GetFiltered(ts => ts.TutorId == currentUserId);

            return tutoringSessions
                .Select(ts => new TutoringSessionDto
                {
                    Id = ts.Id,
                    CreationDate = ts.CreationDate,
                    Evaluation = ts.Evaluation,
                    IsSubscribed = ts.IsSubscribed,
                    ModuleName = ts.Module.Name,
                    ParticipantName = ts.Student.FirstName + " " + ts.Student.LastName,
                    SessionDate = ts.SessionDate,
                    Status = ts.Status,
                    StatusChangeDate = ts.StatusChangeDate
                })
                .OrderByDescending(ts => ts.SessionDate);
        }

        public async Task<IEnumerable<TutoringSessionDto>> GetLearningSessions()
        {
            var currentUserId = _currentUserService.GetUserId();
            var tutoringSessions = await _tutoringSessionsRepository.GetFiltered(ts => ts.StudentId == currentUserId);

            return tutoringSessions
                .Select(ts => new TutoringSessionDto
                {
                    Id = ts.Id,
                    CreationDate = ts.CreationDate,
                    Evaluation = ts.Evaluation,
                    IsSubscribed = ts.IsSubscribed,
                    ModuleName = ts.Module.Name,
                    ParticipantName = ts.Tutor.FirstName + " " + ts.Tutor.LastName,
                    SessionDate = ts.SessionDate,
                    Status = ts.Status,
                    StatusChangeDate = ts.StatusChangeDate
                })
                .OrderByDescending(ts => ts.SessionDate);
        }

        public async Task CreateTutoringSession(TutoringSessionNewDto tutoringSessionNew)
        {
            await ValidateSessionCreation(tutoringSessionNew);

            var tutoringSession = new TutoringSession
            {
                CreationDate = _timeService.GetCurrentTime(),
                IsSubscribed = tutoringSessionNew.IsSubscribed,
                ModuleId = tutoringSessionNew.ModuleId,
                TutorId = _currentUserService.GetUserId(),
                StudentId = tutoringSessionNew.StudentId,
                SessionDate = tutoringSessionNew.SessionDate,
                Status = TutoringSessionStatusEnum.Upcoming
            };

            await _tutoringSessionsRepository.Create(tutoringSession);
        }

        private async Task ValidateSessionCreation(TutoringSessionNewDto tutoringSessionNew)
        {
            var currentUserId = _currentUserService.GetUserId();
            var isEligibleForCreating = await _userManager.Users
                .AnyAsync(u =>
                    u.Id == currentUserId
                    &&
                    u.TutorStudents.Any(ts => 
                        ts.StudentId == tutoringSessionNew.StudentId &&
                        ts.ModuleId == tutoringSessionNew.ModuleId)
                    &&
                    u.TutorModules.Any(tm => tm.ModuleId == tutoringSessionNew.ModuleId)
                    );

            if (!isEligibleForCreating)
            {
                throw new InvalidOperationException("You are not eligible to create a tutoring session for this student in this module.");
            }

            var currentTime = _timeService.GetCurrentTime();
            var difference = tutoringSessionNew.SessionDate.Subtract(currentTime).TotalHours;

            if (difference < 1)
            {
                throw new InvalidOperationException("You may only create a tutoring session one hour from now (or later)");
            }
        }

        public async Task CancelTutoringSession(int id)
        {
            var session = await _tutoringSessionsRepository.GetById(id);
            var currentUserId = _currentUserService.GetUserId();

            if (session.TutorId != currentUserId && session.StudentId != currentUserId)
            {
                throw new InvalidOperationException("You can not cancel this session");
            }

            if (session.Status != TutoringSessionStatusEnum.Upcoming)
            {
                throw new InvalidOperationException("You can not cancel this session");
            }

            session.Status = TutoringSessionStatusEnum.Cancelled;
            session.StatusChangeDate = _timeService.GetCurrentTime();

            await _tutoringSessionsRepository.Update(session);
        }

        public async Task InvertTutoringSessionSubscription(int id)
        {
            var session = await _tutoringSessionsRepository.GetById(id);
            var currentUserId = _currentUserService.GetUserId();

            if (session.TutorId != currentUserId && session.StudentId != currentUserId)
            {
                throw new InvalidOperationException("You can not change subscription status of this session");
            }

            if (session.Status != TutoringSessionStatusEnum.Upcoming)
            {
                throw new InvalidOperationException("You can not change subscription status of this session");
            }

            session.IsSubscribed = !session.IsSubscribed;

            await _tutoringSessionsRepository.Update(session);
        }

        public async Task EvaluateTutoringSession(int id, TutoringSessionEvaluationDto evaluationDto)
        {
            var session = await _tutoringSessionsRepository.GetById(id);

            ValidateSessionEvaluation(session);

            session.Evaluation = evaluationDto.Evaluation;
            session.EvaluationComment = evaluationDto.Comment;

            await _tutoringSessionsRepository.Update(session);
        }

        private void ValidateSessionEvaluation(TutoringSession session)
        {
            var currentUserId = _currentUserService.GetUserId();

            if (session is null)
            {
                throw new InvalidOperationException("You can not evaluate this session: It does not exist.");
            }

            if (session.StudentId != currentUserId)
            {
                throw new InvalidOperationException("You can not evaluate this session: You are not the student.");
            }

            if (session.Status != TutoringSessionStatusEnum.Finished)
            {
                throw new InvalidOperationException("You can not evaluate this session: It's not finished yet");
            }

            if (_timeService.GetCurrentTime().Subtract(session.SessionDate).TotalHours > 2)
            {
                throw new InvalidOperationException("You can not evaluate this session: Time limit has passed already.");
            }

            if (session.Evaluation != null)
            {
                throw new InvalidOperationException("You can not evaluate this session: It has been evaluated already.");
            }
        }
    }
}
