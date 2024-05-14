namespace Q_APlatform.ViewRequestModels
{
    // Request model for saving basic program details and multiple questions details.
    public class ApplicationQuestionAnswerFormRequest
    {
        public int? ApplicationFormId { get; set; }
        public string PrgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public List<QuestionRequest> QuestionRequests { get; set; }
    }
}
