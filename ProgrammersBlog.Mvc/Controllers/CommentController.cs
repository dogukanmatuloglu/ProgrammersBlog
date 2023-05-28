using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System.Text.Json;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<JsonResult> Add(CommentAddDto commentAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.AddAsync(commentAddDto);
                if (result.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
                {
                    var commentAddAjaxViewModel = JsonSerializer.Serialize(new CommentAddAjaxViewModel
                    {
                        CommentDto = result.Data,
                        CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
                    },new JsonSerializerOptions
                    {
                        ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.Preserve
                    }) ;
                    return Json(commentAddAjaxViewModel);
                }
                ModelState.AddModelError("", result.Message);
            }
            var CommentAddAjaxErrorModel= JsonSerializer.Serialize(new CommentAddAjaxViewModel
            {
                CommentAddDto=commentAddDto,
                CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
            });
            return Json(CommentAddAjaxErrorModel);
        }
    }
}
