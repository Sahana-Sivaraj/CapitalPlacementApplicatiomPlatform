using Q_APlatform.IServices;
using Q_APlatform.Models;
using Q_APlatform.ViewRequestModels;

namespace Q_APlatform.Services
{
    public class QuestionService:IQuestionService
    {
        private readonly QuestionAnswerDbContext _dbContext;
        private readonly ILogger<ApplicationFormService> _logger;

        public QuestionService(QuestionAnswerDbContext dbContext, ILogger<ApplicationFormService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // create different type of questions with its answers
        public void CreateQuestionWithCategory(List<QuestionRequest> questionRequests)
        {
            var questions = new List<Question>();
            questionRequests.ForEach(x =>
            {
                var question = new Question()
                {
                    QuestionCategryId = x.QuestionCategryId,
                    IsMultiple = x.IsMultiple,
                    QuestionText = x.QuestionText,
                    ApplicationProgramId = (int)x.ApplicationProgramId!
                };

                _dbContext.Questions.AddAsync(question);

                var answers = new List<QuestionAnswer>();
                if (x.Answers!.Any() && x.Answers!.Count > 0)
                {
                    answers = x.Answers?.Select(y => new QuestionAnswer()
                    {
                        AnswerText = y.AnswerText,
                        IsCorrect = y.IsCorrect,
                        QuestionId = question.Id
                    }).ToList();
                    _dbContext.QuestionAnswers.AddRange(answers!);
                }

            });
            _dbContext.SaveChangesAsync();
        }
        // delete the question by its id and answers
        public async Task<object> DeleteQuestionAndAnswer(int questionId)
        {
            var question = _dbContext.Questions.Find(questionId);
            var questionAnswers = _dbContext.QuestionAnswers.Where(x => x.QuestionId == questionId).ToList();
            _dbContext.QuestionAnswers.RemoveRange(questionAnswers);
            _dbContext.Questions.Remove(question!);
            var result = await _dbContext.SaveChangesAsync();
            return "Successfully deleted";
        }
        //get questions by it category type id
        public List<QuestionRequest> GetQuestionsByCategoryId(int categoryId)
        {
            // get questions based on question type id
            var questions = _dbContext.Questions.Where(x => x.QuestionCategryId == categoryId).ToList();
            // join it with Questions answers table to get related questions and answers
            var questionsAnswers = _dbContext.QuestionAnswers.GroupJoin(questions, qu => qu.QuestionId,
                qa => qa.Id, (qu, qa) => new QuestionAnswer()
                {
                    QuestionId = qu.QuestionId,
                    AnswerText = qu.AnswerText,
                    IsCorrect = qu.IsCorrect,
                    Id = qu.Id
                }).AsEnumerable()
                .ToList();

            var questionsWithAnswers = new List<QuestionRequest>();
            // integrating questions with its answers.
            foreach (var question in questions)
            {
                var answers = questionsAnswers.Where(x => x.QuestionId == question.Id).Select(x => new Answers()
                {
                    QuestionId = x.QuestionId,
                    AnswerId = x.Id,
                    AnswerText = x.AnswerText,
                    IsCorrect = x.IsCorrect
                }).ToList();
                questionsWithAnswers.Add(new QuestionRequest
                    ()
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    IsMultiple = question.IsMultiple,
                    Answers = answers
                });
            }
            return questionsWithAnswers;
        }
        // editing single question and it related answers or adding new answer options
        public async Task<object> UpdateAplicationQuestionAnswer(QuestionRequest applicationReuqest)
        {
            var questions = _dbContext.Questions.ToList();
            var questionAnswers = _dbContext.QuestionAnswers.ToList();

            if (applicationReuqest.QuestionId > 0)
            {// update existing question details
                var existingQuestion = questions.Find(y => y.Id == applicationReuqest.QuestionId);
                existingQuestion!.IsMultiple = applicationReuqest.IsMultiple;
                existingQuestion.QuestionText = applicationReuqest.QuestionText;
                _dbContext.Questions.Update(existingQuestion);
                // loop all answers related to existing questions
                applicationReuqest.Answers?.ForEach(ans =>
                {
                    if (ans.AnswerId != null && ans.AnswerId > 0)
                    { // update existing answer details
                        var existingAnswer = questionAnswers.Find(y => y.Id == ans.AnswerId);
                        existingAnswer.AnswerText = ans.AnswerText;
                        existingAnswer.IsCorrect = ans.IsCorrect;
                        existingAnswer.QuestionId = (int)applicationReuqest.QuestionId;
                        _dbContext.QuestionAnswers.Update(existingAnswer);
                    }
                    else
                    {// add new question related answer if available
                        var maxQAId = _dbContext.QuestionAnswers?.OrderByDescending(a => a.Id).FirstOrDefault() != null ?
                        _dbContext.QuestionAnswers?.OrderByDescending(a => a.Id).FirstOrDefault()?.Id : 0;
                        var newAnswer = new QuestionAnswer()
                        {
                            Id = (int)maxQAId + 1,
                            QuestionId = (int)applicationReuqest.QuestionId,
                            AnswerText = ans.AnswerText,
                            IsCorrect = ans.IsCorrect,
                        };
                        _dbContext.QuestionAnswers.AddAsync(newAnswer);
                    }
                });
            }
            var result = await _dbContext.SaveChangesAsync();
            return "Successfully Updated";
        }
    }
}
