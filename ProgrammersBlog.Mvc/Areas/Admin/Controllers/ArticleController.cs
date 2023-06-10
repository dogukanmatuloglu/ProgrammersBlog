using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using ProgrammersBlog.Entities.Complex_Types;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;
     
        public ArticleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, IToastNotification toastNotification = null) : base(userManager, mapper, imageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }
        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllByNonDeletedAndActiveAsync();
            if (articles.ResultStatus == Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                return View(articles.Data);
            }
            return NotFound();
        }
        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (result.ResultStatus == Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)

            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories,
                });
            }
            return NotFound();

        }
        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
        {
            if (ModelState.IsValid)
            {
                var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var imageResult = await ImageHelper.Upload(articleAddViewModel.Title, articleAddViewModel.ThumbnailFile, Entities.Complex_Types.PictureType.Post);
                articleAddDto.Thumbnail = imageResult.Data.FullName;
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName, LoggedInUser.Id);
                if (result.ResultStatus == Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message,new ToastrOptions
                    {
                        Title="Başarılı İşlem!"
                    });
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);

                }
            }
            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleAddViewModel.Categories = categories.Data.Categories;
            return View(articleAddViewModel);

        }
        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int articleId)
        {
            var articleResult = await _articleService.GetArticleUpdateDtoAsync(articleId);
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (articleResult.ResultStatus == Shared.Utilities.Results.ComplexTypes.ResultStatus.Success && categoriesResult.ResultStatus == Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModel.Categories = categoriesResult.Data.Categories;
                return View(articleUpdateViewModel);
            }
            else
            {
                return NotFound();
            }

        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNewThumbnailUploaded = false;
                var oldThumbnail=articleUpdateViewModel.Thumbnail;
                if (articleUpdateViewModel.ThumbnailFile!=null)
                {
                    var uploadedImageResult = await ImageHelper.Upload(articleUpdateViewModel.Title, articleUpdateViewModel.ThumbnailFile, PictureType.Post);
                    articleUpdateViewModel.Thumbnail = uploadedImageResult.ResultStatus == ResultStatus.Success ? uploadedImageResult.Data.FullName : "postImages/defaultThumbnail.jpg";
                    if (oldThumbnail!="postImages/defaultThumbnail.jpg")
                    {
                        isNewThumbnailUploaded = true;
                    }
                   
                }
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
                var result = await _articleService.UpdateAsync(articleUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    if (isNewThumbnailUploaded)
                    {
                        ImageHelper.Delete(oldThumbnail);
                    }
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
                    {
                        Title = "Başarılı İşlem!"
                    });
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleUpdateViewModel.Categories = categories.Data.Categories;
            return View(articleUpdateViewModel);
          

        }
        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result=await _articleService.DeleteAsync(id,LoggedInUser.UserName);
            var articleResult = JsonConvert.SerializeObject(result);
            return Json(articleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllArticles()
        {
            var articles = await _articleService.GetAllByNonDeletedAndActiveAsync();
            var articleResult = System.Text.Json.JsonSerializer.Serialize(articles,new JsonSerializerOptions
            {
                ReferenceHandler=ReferenceHandler.Preserve
            }); 

            return Json(articleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> DeletedArticles()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            return View(result.Data);

        }
        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllDeletedArticles()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            var articles = System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articles);
        }
        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> UndoDelete(int articleId)
        {
            var result = await _articleService.UndoDeleteAsync(articleId, LoggedInUser.UserName);
            var undoDeleteArticleResult = System.Text.Json.JsonSerializer.Serialize(result);
            return Json(undoDeleteArticleResult);
        }
        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> HardDelete(int articleId)
        {
            var result = await _articleService.HardDeleteAsync(articleId);
            var hardDeletedArticleResult = System.Text.Json.JsonSerializer.Serialize(result);
            return Json(hardDeletedArticleResult);
        }
    }

}
