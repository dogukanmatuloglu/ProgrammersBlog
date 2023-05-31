﻿using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Complex_Types;
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
                var userArticles = await _articleService.GetAllByUserIdOnFiler(articleResult.Data.Article.UserId, FilterBy.Category, OrderBy.Date, false, 10, articleResult.Data.Article.CategoryId, DateTime.Now, DateTime.Now,0,99999,0,99999);
                await _articleService.IncreaseViewCountAsync(articleId);
                return View(new ArticleDetailViewModel
                {
                    ArticleDto = articleResult.Data,
                    ArticleDetailRightSideBarViewModel=new ArticleDetailRightSideBarViewModel
                    {
                        ArticleListDto=userArticles.Data,
                        Header="Kullanıcının Aynı Kategori İçindeki En Çok Okunan Makaleleri",
                        User=articleResult.Data.Article.User
                    }
                });
            }
            return NotFound();
        }
    }
}
