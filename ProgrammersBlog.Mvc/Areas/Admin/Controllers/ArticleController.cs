﻿using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
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
        public IActionResult Add()
        {
            return View(); 
        }
    }
}
