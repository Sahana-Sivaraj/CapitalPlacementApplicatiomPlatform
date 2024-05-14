using Microsoft.AspNetCore.Mvc;
using Q_APlatform.Controllers;
using Q_APlatform.IServices;
using Q_APlatform.Models;
using Q_APlatform.ViewRequestModels;
using static System.Net.Mime.MediaTypeNames;

namespace Q_APlatform.Services
{
    public class ApplicationFormService : IApplicationFormService
    {
        private readonly QuestionAnswerDbContext _dbContext;
        private readonly ILogger<ApplicationFormService> _logger;

        public ApplicationFormService(QuestionAnswerDbContext dbContext, ILogger<ApplicationFormService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // create application details with questions and answers
        public async Task<object> CreateApplication(ApplicationQuestionAnswerFormRequest applicationReuqest)
        {
            var maxObj = _dbContext.ApplicationForms?.OrderByDescending(a => a.Id).FirstOrDefault();
            var maxId = maxObj != null ? maxObj.Id : 0;
            // add new application details
            var applicationForm = new ApplicationForm()
            {
                PrgramTitle = applicationReuqest.PrgramTitle,
                ProgramDescription = applicationReuqest.ProgramDescription,
                Id = (int)maxId! + 1
            };
            _dbContext.ApplicationForms?.Add(applicationForm);
            // loop newly added question to add them
            applicationReuqest.QuestionRequests.ForEach(async x =>
            {
                // add newly added questions
                var maxObj = _dbContext.Questions?.OrderByDescending(a => a.Id).FirstOrDefault();
                var maxQId = maxObj != null ? maxObj.Id : 0;
                var question = new Question()
                {
                    Id = (int)maxQId + 1,
                    QuestionCategryId = x.QuestionCategryId,
                    IsMultiple = x.IsMultiple,
                    QuestionText = x.QuestionText,
                    ApplicationProgramId = applicationForm.Id,
                };

                _dbContext.Questions?.Add(question);
             // add newly added questions related answers
                var answers = new List<QuestionAnswer>();
                if (x.Answers!.Any() && x.Answers!.Count > 0)
                {
                    x.Answers.ForEach(ans =>
                    {
                        var maxObj = _dbContext.QuestionAnswers?.OrderByDescending(a => a.Id).FirstOrDefault();
                        var maxQAId = maxObj != null ? maxObj.Id : 0;

                        var y = new QuestionAnswer()
                        {
                            Id = (int)maxQAId + 1,
                            AnswerText = ans.AnswerText,
                            IsCorrect = ans.IsCorrect,
                            QuestionId = question.Id
                        };
                        _dbContext.QuestionAnswers?.Add(y!);
                        _dbContext.SaveChanges();
                    });

                }
                 _dbContext.SaveChanges();
            });
            await _dbContext.SaveChangesAsync();
            return "Success";
        }
        // update application details,questions and answers also add new question related answers
        public async Task<object> UpdateAplicationQuestionAnswer(ApplicationQuestionAnswerFormRequest applicationReuqest)
        {
            var existingApplicationForm = _dbContext.ApplicationForms.Find(applicationReuqest.ApplicationFormId);
            var questions = _dbContext.Questions.ToList();
            var questionAnswers = _dbContext.QuestionAnswers.ToList();

            if (existingApplicationForm != null)
            {
                // finding and update existing application details
                existingApplicationForm.ProgramDescription = applicationReuqest.ProgramDescription;
                existingApplicationForm.PrgramTitle = applicationReuqest.PrgramTitle;
                _dbContext.ApplicationForms.Update(existingApplicationForm);

                // looping all questions and answers and edit or add to table
                applicationReuqest.QuestionRequests.ForEach(x =>
                {
                    if (x.QuestionId > 0)
                    {// update existing question details
                        var existingQuestion = questions.Find(y => y.Id == x.QuestionId);
                        existingQuestion!.IsMultiple = x.IsMultiple;
                        existingQuestion.QuestionText = x.QuestionText;
                        _dbContext.Questions.Update(existingQuestion);

                        x.Answers?.ForEach(ans =>
                        {// update question existing answers
                            if (ans.AnswerId != null && ans.AnswerId > 0)
                            {
                                var existingAnswer = questionAnswers.Find(y => y.Id == ans.AnswerId);
                                existingAnswer.AnswerText = ans.AnswerText;
                                existingAnswer.IsCorrect = ans.IsCorrect;
                                existingAnswer.QuestionId = (int)x.QuestionId;
                                _dbContext.QuestionAnswers.Update(existingAnswer);
                            }
                            else
                            {// add new answer option if added
                                var maxObj = questionAnswers?.OrderByDescending(a => a.Id).FirstOrDefault();
                                var maxQAId = maxObj != null ? maxObj.Id : 0;

                                var newAnswer = new QuestionAnswer()
                                {
                                    Id = (int)maxQAId + 1,
                                    QuestionId = (int)x.QuestionId,
                                    AnswerText = ans.AnswerText,
                                    IsCorrect = ans.IsCorrect,
                                };
                                _dbContext.QuestionAnswers.Add(newAnswer);
                            }
                        });
                    }
                    else
                    {// add new questions from newly updated application form
                        var maxObj = questions?.OrderByDescending(a => a.Id).FirstOrDefault();
                        var maxQId = maxObj != null ? maxObj.Id : 0;
                        var question = new Question()
                        {
                            Id = (int)maxQId + 1,
                            QuestionCategryId = x.QuestionCategryId,
                            IsMultiple = x.IsMultiple,
                            QuestionText = x.QuestionText,
                            ApplicationProgramId = existingApplicationForm.Id,
                        };

                        _dbContext.Questions?.Add(question);

                        var answers = new List<QuestionAnswer>();
                        // add new questions related answers from newly updated application form
                        if (x.Answers!.Any() && x.Answers!.Count > 0)
                        {

                            x.Answers.ForEach(ans =>
                            {
                                var maxObj = questionAnswers.OrderByDescending(a => a.Id).FirstOrDefault();
                                var maxQAId = maxObj != null ? maxObj.Id : 0;
                                var y = new QuestionAnswer()
                                {
                                    Id = (int)maxQAId + 1,
                                    AnswerText = ans.AnswerText,
                                    IsCorrect = ans.IsCorrect,
                                    QuestionId = question.Id
                                };
                                _dbContext.QuestionAnswers?.Add(y!);
                                _dbContext.SaveChanges();
                            });
                        }
                    }
                });
                 _dbContext.SaveChanges();
            }
            return "Success";
        }
    }
}
