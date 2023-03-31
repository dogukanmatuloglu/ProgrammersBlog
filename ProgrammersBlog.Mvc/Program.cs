using ProgrammersBlog.Mvc.AutoMapper.Profiles;
using ProgrammersBlog.Mvc.Helpers;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}).AddNToastNotifyNoty().AddRazorRuntimeCompilation(); 
builder.Services.AddSession();
builder.Services.LoadMyServices(connectionString:builder.Configuration.GetValue<string>("ConnectionStrings:LocalDB"));
builder.Services.AddScoped<IImageHelper, ImageHelper>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = new PathString("/Admin/User/Login");
    opt.LogoutPath = new PathString("/Admin/User/Logout");
    opt.Cookie = new CookieBuilder
    {
        Name = "ProgrammersBlog",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy=CookieSecurePolicy.SameAsRequest

    };
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
    opt.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");

});

builder.Services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile),typeof(ViewModelProfiles));


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
