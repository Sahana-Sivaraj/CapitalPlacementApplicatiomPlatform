using Microsoft.AspNetCore.Mvc;
using Q_APlatform.ViewRequestModels;

namespace Q_APlatform.IServices
{
    public interface IApplicationFormService
    {
        Task<object> CreateApplication(ApplicationQuestionAnswerFormRequest applicationReuqest);
        Task<object> UpdateAplicationQuestionAnswer(ApplicationQuestionAnswerFormRequest applicationReuqest);
    }
}
