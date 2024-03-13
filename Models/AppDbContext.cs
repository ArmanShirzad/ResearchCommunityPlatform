using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ResearchCommunityPlatform.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options )
        {

        }
       
        public DbSet<Publication> Publications;
        public DbSet<PublicationCreator> PublicationCreators;
        public DbSet<File> Files;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicationCreator>()
                .HasKey(pc => new { pc.PublicationId, pc.UserID });
        }
    }

}