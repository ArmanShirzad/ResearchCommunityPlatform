using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ResearchCommunityPlatform.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using ResearchCommunityPlatform.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using ResearchCommunityPlatform.Utilities;
using ResearchCommunityPlatform.Services.AuthorizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ResearchCommunityPlatform.Services.UserSevice;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<SetClaimsForExistingUsers>();
builder.Services.AddLogging();

// Configure Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie( "CookieAuthScheme",options =>
{
    options.LoginPath = "/Account/socialmedialogin";
    options.AccessDeniedPath = "/Account/socialmedialogin";
    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = TimeSpan.FromHours(1);
    options.Cookie.SameSite = SameSiteMode.Lax;  // Necessary for OAuth2 redirection
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Enforce HTTPS
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
    googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;  // Necessary for OAuth2
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Enforce HTTPS
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Set a reasonable timeout
    options.SlidingExpiration = true;  // Enable slid
});

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EditPolicy", policy =>
        policy.Requirements.Add(new OperationAuthorizationRequirement { Name = AuthorizationConstants.UpdateOperationName }));
    options.AddPolicy("ViewPolicy", policy =>
        policy.Requirements.Add(new OperationAuthorizationRequirement { Name = AuthorizationConstants.ReadOperationName }));
});
builder.Services.AddScoped<IAuthorizationHandler, PublicationAuthorizationHandler>();
builder.Services.AddScoped<AuthorizationService>();

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedData.Initialize(services, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline
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

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
