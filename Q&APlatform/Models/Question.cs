using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q_APlatform.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public bool IsMultiple { get; set; }
        public int ApplicationProgramId {  get; set; }
        public int QuestionCategryId { get; set; }
        
        public QuestionCategory QuestionCategory { get; set; }
       
        public ApplicationForm ApplicationForm { get; set; }
    }
}
