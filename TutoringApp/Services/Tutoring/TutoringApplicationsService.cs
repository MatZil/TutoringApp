﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class TutoringApplicationsService : ITutoringApplicationsService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITimeService _timeService;
        private readonly IRepository<TutoringApplication> _tutoringApplicationsRepository;

        public TutoringApplicationsService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ITimeService timeService,
            IRepository<TutoringApplication> tutoringApplicationsRepository)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _timeService = timeService;
            _tutoringApplicationsRepository = tutoringApplicationsRepository;
        }

        public async Task ApplyForTutoring(int moduleId, TutoringApplicationNewDto tutoringApplicationNew)
        {
            var currentUserId = _currentUserService.GetUserId();
            var currentUser = await _userManager.Users
                .Include(u => u.TutorModules)
                .Include(u => u.TutoringApplications)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            if (currentUser.TutorModules.Any(tm => tm.ModuleId == moduleId))
            {
                throw new InvalidOperationException($"Could not apply for tutoring: user (id='{currentUser.Id}') is already a tutor in this module.");
            }

            var tutoringApplication = new TutoringApplication
            {
                ModuleId = moduleId,
                StudentId = currentUserId,
                RequestDate = _timeService.GetCurrentTime(),
                MotivationalLetter = tutoringApplicationNew.MotivationalLetter
            };
            currentUser.TutoringApplications.Add(tutoringApplication);

            await _userManager.UpdateAsync(currentUser);
        }

        public async Task<IEnumerable<TutoringApplicationDto>> GetTutoringApplications()
        {
            var tutoringApplications = await _tutoringApplicationsRepository.GetAll();

            var tutoringApplicationDtos = tutoringApplications.Select(ta => new TutoringApplicationDto
            {
                Id = ta.Id,
                ModuleName = ta.Module.Name,
                Email = ta.Student.Email,
                Faculty = ta.Student.Faculty,
                MotivationalLetter = ta.MotivationalLetter,
                RequestDate = ta.RequestDate,
                StudentCycle = ta.Student.StudentCycle,
                StudentYear = ta.Student.StudentYear,
                StudentName = $"{ta.Student.FirstName} {ta.Student.LastName}",
                StudyBranch = ta.Student.StudyBranch
            });

            return tutoringApplicationDtos;
        }

        public async Task<(string email, string module)> ConfirmApplication(int applicationId)
        {
            var tutor = await _userManager.Users
                .Include(u => u.TutorModules)
                .Include(u => u.TutoringApplications)
                .ThenInclude(a => a.Module)
                .FirstAsync(u => u.TutoringApplications.Any(ta => ta.Id == applicationId));

            var application = tutor.TutoringApplications.First(ta => ta.Id == applicationId);
            var tutorModule = new ModuleTutor
            {
                ModuleId = application.ModuleId,
                TutorId = tutor.Id
            };

            tutor.TutorModules.Add(tutorModule);
            await _userManager.UpdateAsync(tutor);
            await _tutoringApplicationsRepository.Delete(application);

            return (tutor.Email, application.Module.Name);
        }

        public async Task<(string email, string module)> RejectApplication(int applicationId)
        {
            var application = await _tutoringApplicationsRepository.GetById(applicationId);
            await _tutoringApplicationsRepository.Delete(application);

            return (application.Student.Email, application.Module.Name);
        }
    }
}
