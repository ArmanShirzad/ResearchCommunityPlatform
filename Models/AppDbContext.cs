using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json;

namespace ResearchCommunityPlatform.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options )
        {
        }
       
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationCreator> PublicationCreators { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PublicationCreator>()
                   .HasKey(pc => new { pc.PublicationId, pc.UserID });

            modelBuilder.Entity<PublicationCreator>()
                .HasOne(pc => pc.Publication)
                .WithMany(p => p.PublicationCreators)
                .HasForeignKey(pc => pc.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PublicationCreator>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.PublicationCreators)
                .HasForeignKey(pc => pc.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            //seed model data
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = "1",
                UserName = "user@example.com",
                Email = "user@example.com",
                NormalizedUserName = "USER@EXAMPLE.COM",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEIWY...", // Use a real hash here
                SecurityStamp = Guid.NewGuid().ToString("D"),
            });
  

            // Seed data for the Publication entity
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            modelBuilder.Entity<Publication>().HasData(
                new Publication
                {
                    PublicationId = 1,
                    UserId = "1",
                    Title = "A Bibliometric Analysis of Quantum Machine Learning Research",
                    DOI = "https://doi.org/10.1080/0194262X.2023.2292049",
                    Description = "Quantum Machine Learning is an interdisciplinary field...",
                    AuthorsSerialized = JsonConvert.SerializeObject(new List<string> { "Ali Asghar Ahmadikia", "Arman Shirzad", "Ali Mohammad Saghiri" }),
                    DateOfPublish = "2024-01-03",
                    UploadDate = dateNow,
                    PubType = "journal"

                },
                new Publication
                {
                    PublicationId = 2,
                    UserId = "1",
                    Title = "Novel Speaker Recognition Method on VoxCeleb Dataset",
                    DOI = "",
                    Description = "",
                    AuthorsSerialized = JsonConvert.SerializeObject(new List<string> { "A. Shirzad", "R. Abdollahipour", "R. Darshi" }),
                    DateOfPublish = "",
                    UploadDate = dateNow,
                    PubType = "conference"
                },
                new Publication
                {
                    PublicationId = 3,
                    UserId = "1",
                    Title = "Optimizing Cloud Datacenter Profit Using Q-learning-Based Method",
                    DOI = "",
                    Description = "",
                    AuthorsSerialized = JsonConvert.SerializeObject(new List<string> { "P. Mohammadi", "R. Darshi", "A. Nasiri", "R. Abdollahipour", "A. Shirzad" }),
                    DateOfPublish = "",
                    UploadDate = dateNow,
                    PubType = "conference"
                },
                new Publication
                {
                    PublicationId = 4, // Increment the ID based on your last seeded entry
                    UserId = "1", // Assuming this publication is associated with the first user
                    Title = "Data Governance and Stewardship: Designing Data Stewardship Entities and Advancing Data Access",
                    DOI = "https://doi.org/10.1111/j.1475-6773.2010.01140.x",
                    Description = "U.S. health policy is engaged in a struggle over access to health information...",
                    AuthorsSerialized = JsonConvert.SerializeObject(new List<string> { "Sara Rosenbaum" }),
                    DateOfPublish = "2010-08-02", // Use the publication date in YYYY-MM-DD format
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd"), // Use current date as upload date
                    PubType = "journal"
                });


            // Seed PublicationCreators related to the publications
            modelBuilder.Entity<PublicationCreator>().HasData(
                new PublicationCreator { PublicationId = 1, UserID = "1" },
                new PublicationCreator { PublicationId = 2, UserID = "1" },
                new PublicationCreator { PublicationId = 3, UserID = "1" }
            );


            //file
            modelBuilder.Entity<File>().HasData(
              new File
              {
                  FileID = 1,
                  PublicationId = 1,
                  FileName = "user1pub001.pdf",
                  FilePath = "/Files/user1pub001.pdf", // Starts with '/'
                  Size = 7.98f // Example size in bytes (512 KB
              });


        }
    }

}