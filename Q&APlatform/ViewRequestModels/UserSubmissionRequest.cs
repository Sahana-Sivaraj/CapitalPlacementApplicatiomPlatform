namespace Q_APlatform.ViewRequestModels
{
    //Request model to save questions and question related answers.
    public class UserSubmissionRequest
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string AnswerText { get; set; }
    }
}
