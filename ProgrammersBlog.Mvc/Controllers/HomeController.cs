using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly AboutUsPageInfo _aboutUsPageInfo;

        public HomeController(IArticleService articleService,IOptions<AboutUsPageInfo> aboutUsPageInfo)
        {
            _articleService = articleService;
            _aboutUsPageInfo=aboutUsPageInfo.Value;
        }


        public async Task<IActionResult> Index(int? categoryId,int currentPage=1,int pageSize=5,bool isAscending=false)
        {
            var articlesResult = categoryId == null ? await _articleService.GetAllByPagingAsync(null, currentPage, pageSize,isAscending) : await _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize,isAscending);
            return View(articlesResult.Data);          

        }

        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            return View();
        }
    }
}
