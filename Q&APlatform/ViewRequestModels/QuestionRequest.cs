namespace Q_APlatform.ViewRequestModels
{
    // Request model for saving question related details and its answers.
    public class QuestionRequest
    {
        public int? QuestionId { get; set; }
        public int? ApplicationProgramId { get; set; }
        public string? QuestionText { get; set; }
        public bool IsMultiple { get; set; }
        public int QuestionCategryId { get; set; }
        public List<Answers>? Answers { get; set; }
    }
}
