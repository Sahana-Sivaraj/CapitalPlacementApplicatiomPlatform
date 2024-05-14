using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Q_APlatform.Models;
using Q_APlatform.ViewRequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q_APlatform.Controllers
{
    [Route("api/userapplication")]
    [ApiController]
    public class UserApplicationSubmissionController : ControllerBase
    {
        private readonly QuestionAnswerDbContext _dbContext;
        private readonly ILogger<ApplicationFormController> _logger;

        public UserApplicationSubmissionController(QuestionAnswerDbContext dbContext, ILogger<ApplicationFormController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // POST api/<UserApplicationSubmissionController>
        [HttpPost("submission")]
        public async Task<object> Post([FromBody] List<UserSubmissionRequest> value)
        {
            value.ForEach(x =>
            {
                var maxObj = _dbContext.UserAnswers?.OrderByDescending(a => a.Id).FirstOrDefault();
                var maxQId = maxObj!=null? maxObj.Id:0;
                var newUserAnswer = new UserAnswer()
                {
                    Id= maxQId+1,
                    AnswerId = x.AnswerId,
                    AnswerText = x.AnswerText,
                    QuestionId = x.QuestionId,
                    UserId = x.UserId
                };
                _dbContext.UserAnswers?.Add(newUserAnswer);
                _dbContext.SaveChanges();
            });
            var result = await _dbContext.SaveChangesAsync();
            return Ok(result);

        }
    }
}
