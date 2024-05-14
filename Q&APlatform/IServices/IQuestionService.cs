using Microsoft.AspNetCore.Mvc;
using Q_APlatform.ViewRequestModels;

namespace Q_APlatform.IServices
{
    public interface IQuestionService
    {
        List<QuestionRequest> GetQuestionsByCategoryId(int categoryId);
        void CreateQuestionWithCategory(List<QuestionRequest> questionRequests);
        Task<object> UpdateAplicationQuestionAnswer(QuestionRequest applicationReuqest);
        Task<object> DeleteQuestionAndAnswer(int questionId);
    }
}
