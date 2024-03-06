using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ResearchCommunityPlatform.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options )
        {

        }
        //dbsets - onModelCreating

    }
}
