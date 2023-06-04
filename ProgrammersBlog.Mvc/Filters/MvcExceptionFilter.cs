using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using ProgrammersBlog.Shared.Entities.Concrete;
using System.Data.SqlTypes;

namespace ProgrammersBlog.Mvc.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger _logger;

        public MvcExceptionFilter(IModelMetadataProvider modelMetadataProvider, IHostEnvironment environment, ILogger<MvcExceptionFilter> logger)
        {
            _modelMetadataProvider = modelMetadataProvider;
            _environment = environment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (_environment.IsDevelopment()) {
                context.ExceptionHandled = true;
                var mvcErrorModel = new MvcErrorModel();
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = "Üzgünüz, işleminiz sırasında beklenmedik bir veritabanı hatası oluştu. Sorunu en kısa sürede çözeceğiz.";
                        mvcErrorModel.Detail=context.Exception.Message;
                        _logger.LogError(context.Exception, context.Exception.Message);
                            break;
                    case NullReferenceException:
                        mvcErrorModel.Message = "Üzgünüz, işleminiz sırasında beklenmedik bir null veri hatası oluştu. Sorunu en kısa sürede çözeceğiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message = $"Üzgünüz, işleminiz sırasında beklenmedik bir hata oluştu. Sorunu en kısa sürede çözeceğiz.";
                        _logger.LogError(context.Exception, "Bu benim log hata mesajım");
                        break;
                }
               
                var result = new ViewResult() { ViewName = "Error" };
                result.StatusCode = 500;
                result.ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;
            }
        }
    }
}
