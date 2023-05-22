using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(int? categoryId,int currentPage=1,int pageSize=5)
        {
            var articlesResult = categoryId == null ? await _articleService.GetAllByPagingAsync(null, currentPage, pageSize) : await _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize);
            return View(articlesResult.Data);
           

        }
    }
}
