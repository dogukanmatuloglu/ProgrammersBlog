using ProgrammersBlog.Entities.Dtos;

namespace ProgrammersBlog.Mvc.Models
{
    public class CommentAddAjaxViewModel
    {
        public CommentAddDto CommentAddDto{ get; set; }
        public String CommentAddPartial { get; set; }
        public CommentDto CommentDto { get; set; }
    }
}
