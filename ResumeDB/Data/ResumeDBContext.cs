using Microsoft.EntityFrameworkCore;
using ResumeDB.Models;

namespace ResumeDB.Data
{
    public class ResumeDBContext : DbContext
    {
        public ResumeDBContext(DbContextOptions<ResumeDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Education> Educations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Alice Andersson",
                    Description = "Frontend developer with a passion for UI.",
                    ContactInfo = "alice@example.com"
                },
                new User
                {
                    Id = 2,
                    Name = "Bob Bergström",
                    Description = "Backend developer who loves APIs.",
                    ContactInfo = "bob@example.com"
                }
            );

            // Seed Education
            modelBuilder.Entity<Education>().HasData(
                new Education
                {
                    Id = 1,
                    School = "Lund University",
                    Degree = "BSc Computer Science",
                    StartDate = new DateTime(2018, 9, 1),
                    EndDate = new DateTime(2021, 6, 15),
                    UserId_FK = 1
                },
                new Education
                {
                    Id = 2,
                    School = "KTH Royal Institute of Technology",
                    Degree = "MSc Software Engineering",
                    StartDate = new DateTime(2017, 9, 1),
                    EndDate = new DateTime(2020, 6, 15),
                    UserId_FK = 2
                }
            );

            // Seed WorkExperience
            modelBuilder.Entity<WorkExperience>().HasData(
                new WorkExperience
                {
                    Id = 1,
                    JobTitle = "Frontend Developer",
                    Company = "Creative Tech AB",
                    Description = "Worked with React and Angular.",
                    Year = 2022,
                    UserId_FK = 1
                },
                new WorkExperience
                {
                    Id = 2,
                    JobTitle = "Backend Developer",
                    Company = "API Masters AB",
                    Description = "Developed REST APIs with .NET Core.",
                    Year = 2023,
                    UserId_FK = 2
                }
            );
        }
    }
}
