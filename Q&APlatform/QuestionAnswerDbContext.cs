using Microsoft.EntityFrameworkCore;
using Q_APlatform.Models;

namespace Q_APlatform
{
    public class QuestionAnswerDbContext:DbContext
    {
        // Question model to save all question details
        public DbSet<Question> Questions { get; set; }
        // save application related details
        public DbSet<ApplicationForm> ApplicationForms { get; set; }
        // Save multiple option,drop down questions related answers
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        // question related types
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        // system users
        public DbSet<User> Users { get; set; }
        // user submission user answers
        public DbSet<UserAnswer> UserAnswers { get; set; }

        public QuestionAnswerDbContext(DbContextOptions<QuestionAnswerDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
