using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ResearchCommunityPlatform.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        using (var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
        {
            // Seed roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Seed users if they don't exist
            if (await userManager.FindByEmailAsync("testuser@example.com") == null)
            {
                var user = new User { UserName = "testuser@example.com", Email = "testuser@example.com" };
                await userManager.CreateAsync(user, "Password123!");
            }

            if (await userManager.FindByEmailAsync("adminuser@example.com") == null)
            {
                var uploader = new User { UserName = "adminuser@example.com", Email = "adminuser@example.com" };
                await userManager.CreateAsync(uploader, "Password123!");
                await userManager.AddToRoleAsync(uploader, "Admin");
            }

            var testUser = await userManager.FindByEmailAsync("testuser@example.com");
            var adminUser = await userManager.FindByEmailAsync("adminuser@example.com");

            // Seed specific test publications if they don't exist
            if (!context.Publications.Any(p => p.Title == "First Publication" && p.UserId == testUser.Id))
            {
                context.Publications.Add(new Publication
                {
                    Title = "First Publication",
                    DOI = "10.1234/example",
                    Description = "This is the first publication",
                    UserId = testUser.Id,
                    DateOfPublish = DateTime.Now.ToString("yyyy-MM-dd"),
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    PubType = "Journal",
                    AuthorsSerialized = "[\"Author1\", \"Author2\"]"
                });
            }

            if (!context.Publications.Any(p => p.Title == "Second Publication" && p.UserId == adminUser.Id))
            {
                context.Publications.Add(new Publication
                {
                    Title = "Second Publication",
                    DOI = "10.5678/example",
                    Description = "This is the second publication",
                    UserId = adminUser.Id,
                    DateOfPublish = DateTime.Now.ToString("yyyy-MM-dd"),
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    PubType = "Conference",
                    AuthorsSerialized = "[\"Author1\", \"Author2\"]"
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
