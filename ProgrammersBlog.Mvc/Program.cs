using NLog.Web;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.AutoMapper.Profiles;
using ProgrammersBlog.Mvc.Filters;
using ProgrammersBlog.Mvc.Helpers;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using ProgrammersBlog.Shared.Utilities.Extensions;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews(opt =>
{
    opt.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value=>"Bu alan boþ geçilmemelidir");
    opt.Filters.Add<MvcExceptionFilter>();

}).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}).AddNToastNotifyNoty().AddRazorRuntimeCompilation(); 
builder.Services.AddSession();
builder.Services.LoadMyServices(connectionString:builder.Configuration.GetValue<string>("ConnectionStrings:LocalDB"));
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.Configure<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.Configure<WebSiteInfo>(builder.Configuration.GetSection("WebSiteInfo"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.ConfigureWritable<AboutUsPageInfo>(builder.Configuration.GetSection("AboutUsPageInfo"));
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = new PathString("/Admin/Auth/Login");
    opt.LogoutPath = new PathString("/Admin/Auth/Logout");
    opt.Cookie = new CookieBuilder
    {
        Name = "ProgrammersBlog",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy=CookieSecurePolicy.SameAsRequest

    };
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
    opt.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");

});

builder.Services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile),typeof(ViewModelProfiles),typeof(CommentProfile));
builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    //config.Sources.Clear();
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile("appsettings.json", true, true);
    config.AddEnvironmentVariables();
   
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseStatusCodePages();

    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseNToastNotify();
app.UseEndpoints(endpoints => { endpoints.MapAreaControllerRoute(name: "Admin", areaName: "Admin", pattern: "Admin/{controller=Home}/{action=Index}/{id?}"); endpoints.MapDefaultControllerRoute(); } );
app.Run();
