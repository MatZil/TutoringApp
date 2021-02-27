using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Data.Seeders;

namespace TutoringApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<TutoringSession> TutoringSessions { get; set; }
        public DbSet<TutoringRequest> TutoringRequests { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<TutorEvaluation> TutorEvaluations { get; set; }
        public DbSet<StudentTutor> StudentTutors { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleTutor> ModuleTutors { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureTutoringSessions(builder);
            ConfigureChatMessages(builder);
            ConfigureAssignments(builder);
            ConfigureTutorEvaluations(builder);
            ConfigureStudentTutors(builder);
            ConfigureModuleTutors(builder);
            ConfigureModules(builder);

            builder.SeedModules();

            builder.ApplyConfiguration(new RoleConfiguration());
        }

        private static void ConfigureModules(ModelBuilder builder)
        {
            builder.Entity<Module>()
                .HasIndex(m => m.Name)
                .IsUnique();
        }

        private static void ConfigureTutoringSessions(ModelBuilder builder)
        {
            builder.Entity<TutoringSession>()
                .HasOne(ts => ts.Student)
                .WithMany(u => u.LearningSessions)
                .HasForeignKey(ts => ts.StudentId);

            builder.Entity<TutoringSession>()
                .HasOne(ts => ts.Tutor)
                .WithMany(u => u.TutoredSessions)
                .HasForeignKey(ts => ts.TutorId);
        }

        private static void ConfigureChatMessages(ModelBuilder builder)
        {
            builder.Entity<ChatMessage>()
                .HasOne(cm => cm.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(cm => cm.SenderId);

            builder.Entity<ChatMessage>()
                .HasOne(cm => cm.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(cm => cm.ReceiverId);
        }

        private static void ConfigureAssignments(ModelBuilder builder)
        {
            builder.Entity<Assignment>()
                .HasOne(a => a.Student)
                .WithMany(u => u.StudentAssignments)
                .HasForeignKey(a => a.StudentId);

            builder.Entity<Assignment>()
                .HasOne(a => a.Tutor)
                .WithMany(u => u.TutorAssignments)
                .HasForeignKey(a => a.TutorId);
        }

        private static void ConfigureTutorEvaluations(ModelBuilder builder)
        {
            builder.Entity<TutorEvaluation>()
                .HasOne(te => te.Student)
                .WithMany(u => u.StudentEvaluations)
                .HasForeignKey(te => te.StudentId);

            builder.Entity<TutorEvaluation>()
                .HasOne(te => te.Tutor)
                .WithMany(u => u.TutorEvaluations)
                .HasForeignKey(te => te.TutorId);
        }

        private static void ConfigureStudentTutors(ModelBuilder builder)
        {
            builder.Entity<StudentTutor>()
                .HasOne(sti => sti.Student)
                .WithMany(u => u.IgnoresFromTutors)
                .HasForeignKey(sti => sti.StudentId);

            builder.Entity<StudentTutor>()
                .HasOne(sti => sti.Tutor)
                .WithMany(u => u.IgnoresToStudents)
                .HasForeignKey(sti => sti.TutorId);
        }

        private static void ConfigureModuleTutors(ModelBuilder builder)
        {
            builder.Entity<ModuleTutor>()
                .HasOne(mt => mt.Module)
                .WithMany(m => m.ModuleTutors)
                .HasForeignKey(mt => mt.ModuleId);

            builder.Entity<ModuleTutor>()
                .HasOne(mt => mt.Tutor)
                .WithMany(u => u.TutorModules)
                .HasForeignKey(mt => mt.TutorId);

            builder.Entity<ModuleTutor>()
                .HasKey(mt => new { mt.ModuleId, mt.TutorId });
        }
    }
}
