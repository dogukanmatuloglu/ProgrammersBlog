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

        public async Task<IActionResult> Index(int? categoryId,int currentPage=1,int pageSize=5,bool isAscending=false)
        {
            var articlesResult = categoryId == null ? await _articleService.GetAllByPagingAsync(null, currentPage, pageSize,isAscending) : await _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize,isAscending);
            return View(articlesResult.Data);
           

        }
    }
}
