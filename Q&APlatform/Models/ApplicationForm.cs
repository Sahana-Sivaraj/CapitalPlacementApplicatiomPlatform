namespace Q_APlatform.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }
        public string PrgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
