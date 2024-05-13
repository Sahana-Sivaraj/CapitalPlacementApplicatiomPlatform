using Microsoft.EntityFrameworkCore;
using Q_APlatform.Models;

namespace Q_APlatform
{
    public class QuestionAnswerDbContext:DbContext
    {

        public DbSet<Question> Questions { get; set; }
        public DbSet<ApplicationForm> ApplicationForms { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        public QuestionAnswerDbContext(DbContextOptions<QuestionAnswerDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultContainer("QuestionAnsAnswerPlatformContainer");
            modelBuilder.Entity<QuestionCategory>()
                  .ToContainer(nameof(QuestionCategory));
            modelBuilder.Entity<ApplicationForm>()
                  .ToContainer(nameof(ApplicationForm));
            modelBuilder.Entity<Question>()
                .ToContainer(nameof(Question));

            modelBuilder.Entity<QuestionAnswer>()
                   .ToContainer(nameof(QuestionAnswer));

            modelBuilder.Entity<User>()
                   .ToContainer(nameof(User));

            modelBuilder.Entity<UserAnswer>()
                  .ToContainer(nameof(UserAnswer));
            base.OnModelCreating(modelBuilder);
        }
    }
}
