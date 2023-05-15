using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int articleId)
        {
            var articleResult=await _articleService.GetAsync(articleId);
            if (articleResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                return View(articleResult.Data);
            }
            return NotFound();
        }
    }
}
