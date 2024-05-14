using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q_APlatform.IServices;
using Q_APlatform.Models;
using Q_APlatform.ViewRequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q_APlatform.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationFormController : ControllerBase
    {
        private readonly IApplicationFormService _applicationFormService;
        private readonly ILogger<ApplicationFormController> _logger;

        public ApplicationFormController(IApplicationFormService applicationFormService, ILogger<ApplicationFormController> logger)
        {
            _applicationFormService = applicationFormService;
            _logger = logger;
        }
        // POST api/<ApplicationFormController>
        // Create Application endpoint with questions and its multi-optional answers
        [HttpPost("create")]
        public async Task<object> CreateApplication([FromBody] ApplicationQuestionAnswerFormRequest applicationReuqest)
        {
            var result = _applicationFormService.CreateApplication(applicationReuqest);
            return Ok(result);
        }

        // update application details and its related question details
        [HttpPut("updateapplication")]
        public async Task<object> UpdateAplicationQuestionAnswer([FromBody] ApplicationQuestionAnswerFormRequest applicationReuqest)
        {
            var result = _applicationFormService.UpdateAplicationQuestionAnswer(applicationReuqest);
            return Ok(result);
        }
    }
}
