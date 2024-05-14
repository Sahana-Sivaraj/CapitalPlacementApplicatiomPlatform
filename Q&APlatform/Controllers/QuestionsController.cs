using Microsoft.AspNetCore.Mvc;
using Q_APlatform.IServices;
using Q_APlatform.Models;
using Q_APlatform.ViewRequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q_APlatform.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ILogger<ApplicationFormController> _logger;

        public QuestionsController(IQuestionService questionService, ILogger<ApplicationFormController> logger)
        {
            _questionService = questionService;
            _logger = logger;
        }
        // GET: api/<QuestionsController>
        // get endpoint for retrival of questions based on question type id
        [HttpGet("category/{categoryId}")]
        public List<QuestionRequest> GetQuestionsByCategoryId(int categoryId)
        {
            return _questionService.GetQuestionsByCategoryId(categoryId);
        }

        // POST api/<QuestionsController>
        [HttpPost("create")]
        // create bulk questions with dofferent types
        public void CreateQuestionWithCategory([FromBody] List<QuestionRequest> questionRequests)
        {
           _questionService.CreateQuestionWithCategory(questionRequests);
        }
        // update particular question and answers
        [HttpPut("updatequestion")]
        public async Task<object> UpdateAplicationQuestionAnswer([FromBody] QuestionRequest applicationReuqest)
        {
            var result = _questionService.UpdateAplicationQuestionAnswer(applicationReuqest);
            return  Ok(result);
        }
        // delete the question and its answers
        [HttpDelete("deletequestion/{questionId}")]
        public async Task<object> DeleteQuestionAndAnswer([FromBody] int questionId)
        {
            var result = _questionService.DeleteQuestionAndAnswer(questionId);
            return Ok(result);
        }
    }
}
