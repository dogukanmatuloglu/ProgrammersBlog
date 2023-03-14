using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager, ICommentService commentService , IArticleService articleService , ICategoryService categoryService)
        {
            _userManager = userManager;
            _commentService = commentService;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByNonDeleted();
            var articlesCountResult = await _articleService.CountByNonDeleted();
            var commentsCountResult=await _commentService.CountByNonDeleted();
            var usersCount = await _userManager.Users.CountAsync();
            var artcilesResult = await _articleService.GetAll();

            if (categoriesCountResult.ResultStatus == ResultStatus.Success && articlesCountResult.ResultStatus == ResultStatus.Success && commentsCountResult.ResultStatus == ResultStatus.Success && usersCount > -1 && artcilesResult.ResultStatus == ResultStatus.Success)
            {


                return View(new DashboardViewModel
                {
                    CategoriesCount = categoriesCountResult.Data,
                    ArticlesCount = articlesCountResult.Data,
                    CommentCount = commentsCountResult.Data,
                    UserCount = usersCount,
                    Articles = artcilesResult.Data
                });
            }
            return NotFound();
        }
    }
}
