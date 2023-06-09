using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Shared.Utilities.Helpers.Abstract;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly AboutUsPageInfo _aboutPageInfo;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
        private readonly IToastNotification _toastNotification;
        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter, IToastNotification toastNotification)
        {
            _aboutPageInfo = aboutPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutPageInfo);
        }

        [HttpPost]
        public IActionResult About(AboutUsPageInfo aboutUsPageInfo)
        {
            if (ModelState.IsValid)
            {
                _aboutUsPageInfoWriter.Update(x =>
                {
                    x.Header = aboutUsPageInfo.Header;
                    x.SeoAuthor = aboutUsPageInfo.SeoAuthor;
                    x.SeoTags = aboutUsPageInfo.SeoTags;
                    x.Content = aboutUsPageInfo.Content;
                    x.SeoDescription = aboutUsPageInfo.SeoDescription;

                });
                _toastNotification.AddSuccessToastMessage("Hakkımızda Sayfa İçerikleri Başarıyla Güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"
                });
            }
            return View(_aboutPageInfo);
        }
    }
}
