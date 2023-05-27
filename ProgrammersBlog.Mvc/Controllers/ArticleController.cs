using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Models;
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

        public async Task<IActionResult> Search(string keyword,int currentPage=1,int pageSize=5,bool IsAscending=false)
        {
            var searchResult=await _articleService.SearchAsync(keyword, currentPage, pageSize, IsAscending);
            if (searchResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success) {
                return View(new ArticleSearchViewModel
                {
                    ArticleListDto = searchResult.Data,
                    Keyword = keyword,
                });
            }
            return NotFound();
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
