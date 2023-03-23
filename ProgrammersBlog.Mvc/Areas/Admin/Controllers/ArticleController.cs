using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        public ArticleController(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllByNonDeletedAndActiveAsync();
            if (articles.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                return View(articles.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            if (result.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories=result.Data.Categories,
                });
            }
            return NotFound();
           
        }
    }
}
