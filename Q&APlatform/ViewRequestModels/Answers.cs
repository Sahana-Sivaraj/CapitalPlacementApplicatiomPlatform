namespace Q_APlatform.ViewRequestModels
{// Answer details model
    public class Answers
    {
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
