using System.ComponentModel.DataAnnotations.Schema;

namespace Q_APlatform.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public int QuestionId {  get; set; }
        public string AnswerText {  get; set; }
        public bool IsCorrect { get; set; }
       
        public Question Question { get; set; }
    }
}
