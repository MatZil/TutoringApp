using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Aggregates;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;

namespace TutoringApp.Services.Interfaces
{
    public interface ITutoringSessionsService
    {
        Task<IEnumerable<TutoringSessionDto>> GetTutoringSessions();
        Task<IEnumerable<TutoringSessionDto>> GetLearningSessions();
        Task CreateTutoringSession(TutoringSessionNewDto tutoringSessionNew);
        Task CancelTutoringSession(int id);
        Task InvertTutoringSessionSubscription(int id);
        Task<SessionEvaluationEmailAggregate> EvaluateTutoringSession(int id, TutoringSessionEvaluationDto evaluationDto);
    }
}
