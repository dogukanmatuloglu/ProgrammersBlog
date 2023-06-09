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
        private readonly WebSiteInfo _webSiteInfo;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWritableOptions<SmtpSettings> _smtSettingsWriter;
        private readonly IWritableOptions<WebSiteInfo> _webSiteInfoWriter;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
        private readonly IToastNotification _toastNotification;
        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter, IToastNotification toastNotification, IOptionsSnapshot<WebSiteInfo> webSiteInfo, IWritableOptions<WebSiteInfo> webSiteInfoWriter, IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtSettingsWriter)
        {
            _aboutPageInfo = aboutPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
            _toastNotification = toastNotification;
            _webSiteInfo = webSiteInfo.Value;
            _webSiteInfoWriter = webSiteInfoWriter;
            _smtpSettings = smtpSettings.Value;
            _smtSettingsWriter = smtSettingsWriter;
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
            return View(aboutUsPageInfo);
        }


        [HttpGet]
        public IActionResult GeneralSettings()
        {
            return View(_webSiteInfo);
        }

        [HttpPost]
        public IActionResult GeneralSettings(WebSiteInfo webSiteInfo)
        {
            if (ModelState.IsValid)
            {
                _webSiteInfoWriter.Update(x =>
                {
                    x.Title = webSiteInfo.Title;
                    x.MenuTitle = webSiteInfo.MenuTitle;
                    x.SeoAuthor= webSiteInfo.SeoAuthor;
                    x.SeoTags= webSiteInfo.SeoTags;
                    x.SeoDescription= webSiteInfo.SeoDescription;
                    
                });
                _toastNotification.AddSuccessToastMessage("Sitenizin Genel Ayarları Başarıyla Güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"
                });
            }
            return View(webSiteInfo);
        }

        [HttpGet]
        public IActionResult EmailSettings()
        {
            return View(_smtpSettings);
        }

        [HttpPost]
        public IActionResult EmailSettings(SmtpSettings smtpSettings)
        {
            if (ModelState.IsValid)
            {
                _smtSettingsWriter.Update(x =>
                {
                    x.SenderEmail = smtpSettings.SenderEmail;
                    x.SenderName = smtpSettings.SenderName;
                    x.Server=smtpSettings.Server;
                    x.Port= smtpSettings.Port;
                    x.Password= smtpSettings.Password;
                    x.Username = smtpSettings.Username;

                });
                _toastNotification.AddSuccessToastMessage("Sitenizin E-Posta Ayarları Başarıyla Güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"
                });
            }
            return View(smtpSettings);
        }


    }
}
