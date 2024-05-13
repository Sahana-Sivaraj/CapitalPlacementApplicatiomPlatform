using System.ComponentModel.DataAnnotations.Schema;

namespace Q_APlatform.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId {  get; set; }
        public int? AnswerId { get; set; }
        public string AnswerText { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}
