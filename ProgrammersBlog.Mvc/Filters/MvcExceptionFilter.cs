using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using ProgrammersBlog.Shared.Entities.Concrete;

namespace ProgrammersBlog.Mvc.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public MvcExceptionFilter(IModelMetadataProvider modelMetadataProvider, IHostEnvironment environment)
        {
            _modelMetadataProvider = modelMetadataProvider;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            if (_environment.IsDevelopment()) {
            context.ExceptionHandled = true;
                var mvcErrorModel = new MvcErrorModel()
                {
                    Message = $"Üzgünüz, işleminiz sırasında beklenmedik bir hata oluştu. Sorunu en kısa sürede çözeceğiz."
                };
                var result = new ViewResult() { ViewName = "Error" };
                result.StatusCode = 500;
                result.ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;
            }
        }
    }
}
