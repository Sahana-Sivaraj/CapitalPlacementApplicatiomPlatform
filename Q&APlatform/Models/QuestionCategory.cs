namespace Q_APlatform.Models
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
