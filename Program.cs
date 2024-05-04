using Microsoft.EntityFrameworkCore;
using T =Microsoft.Extensions.Configuration;
using ResearchCommunityPlatform.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using ResearchCommunityPlatform.Services.UserSevice;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

.AddCookie(options =>
{
    //options.LoginPath = "/Account/ExternalLogin";
    options.LoginPath = "/Account/socialmedialogin"; // Point to your login action
    options.AccessDeniedPath = "/Account/socialmedialogin";
    options.Cookie.HttpOnly = true;  // Make the cookie inaccessible to client-side scripts
    options.Cookie.SameSite = SameSiteMode.None;  // Necessary for OAuth2 redirection
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Enforce HTTPS
})
.AddGoogle(googleOptions =>

{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CorrelationCookie.SameSite = SameSiteMode.None;
    googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;  // Necessary for OAuth2 redirection
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Enforce HTTPS
});
builder.Services.AddControllersWithViews(); 
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
var app = builder.Build();  

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500; // or use the actual status code if different
            context.Response.ContentType = "text/plain";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            await context.Response.WriteAsync($"Exception: {exception?.Message}");
        });
    });
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseDeveloperExceptionPage();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
