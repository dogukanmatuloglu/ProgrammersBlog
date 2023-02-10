using ProgrammersBlog.Entities.Dtos;

namespace ProgrammersBlog.Mvc.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryAddDto  CategoryAddDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
