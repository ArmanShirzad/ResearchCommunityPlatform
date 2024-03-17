using Microsoft.EntityFrameworkCore;
using T =Microsoft.Extensions.Configuration;
using ResearchCommunityPlatform.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")));
builder.Services.AddAuthentication()
    .AddGoogle(opts =>

    {
        opts.ClientId = "google-client-id";
        opts.ClientSecret = "google-client-secret";
    })
    .AddOAuth("GitHub", opts =>
    {
        opts.ClientId = "github-client-id";
        opts.ClientSecret = "github-client-secret";
        opts.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        opts.TokenEndpoint = "https://github.com/login/oauth/access_token";
        opts.UserInformationEndpoint = "https://api.github.com/user";
    })
    .AddOAuth("ORCID", opts =>
    {
        opts.ClientId = "orcid-client-id";
        opts.ClientSecret = "orcid-client-secret";
        opts.AuthorizationEndpoint = "https://orcid.org/oauth/authorize";
        opts.TokenEndpoint = "https://pub.orcid.org/oauth/token";
        opts.UserInformationEndpoint = "https://pub.orcid.org/v3.0/[ORCID_ID]/person";
    });     
// Add other providers as needed
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
