
using Microsoft.EntityFrameworkCore;
using ResumeDB.Data;
using ResumeDB.Models;

namespace ResumeDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ResumeDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            builder.Services.AddHttpClient();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/all", async (ResumeDBContext context) =>
            {
                var users = await context.Users
                    .Include(u => u.Educations)
                    .Include(u => u.WorkExperiences)
                    .ToListAsync();
                return Results.Ok(users);
            });

            app.MapGet("/user/{id:int}", async (int id, ResumeDBContext context) =>
            {
                var user = await context.Users
                    .Include(u => u.Educations)
                    .Include(u => u.WorkExperiences)
                    .FirstOrDefaultAsync(u => u.Id == id);

                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            app.MapPost("/education", async (Education edu, ResumeDBContext context) =>
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(edu.School) || string.IsNullOrWhiteSpace(edu.Degree))
                    return Results.BadRequest("School and Degree are required.");

                // Verify user exists
                var userExists = await context.Users.AnyAsync(u => u.Id == edu.UserId_FK);
                if (!userExists)
                    return Results.BadRequest("UserId_FK does not exist.");

                context.Educations.Add(edu);
                await context.SaveChangesAsync();

                return Results.Created($"/education/{edu.Id}", edu);
            });

            app.MapPost("/workexperience", async (WorkExperience workExp, ResumeDBContext context) =>
            {
                if (string.IsNullOrWhiteSpace(workExp.JobTitle) || string.IsNullOrWhiteSpace(workExp.Company))
                    return Results.BadRequest("JobTitle and Company are required.");

                var userExists = await context.Users.AnyAsync(u => u.Id == workExp.UserId_FK);
                if (!userExists)
                    return Results.BadRequest("UserId_FK does not exist.");

                context.WorkExperiences.Add(workExp);
                await context.SaveChangesAsync();

                return Results.Created($"/workexperience/{workExp.Id}", workExp);
            });

            app.MapPut("/education/{id:int}", async (int id, Education eduUpdate, ResumeDBContext context) =>
            {
                var edu = await context.Educations.FindAsync(id);
                if (edu == null) return Results.NotFound();

                // Update fields with validation
                if (!string.IsNullOrWhiteSpace(eduUpdate.School)) edu.School = eduUpdate.School;
                if (!string.IsNullOrWhiteSpace(eduUpdate.Degree)) edu.Degree = eduUpdate.Degree;
                if (eduUpdate.StartDate != default) edu.StartDate = eduUpdate.StartDate;
                if (eduUpdate.EndDate != default) edu.EndDate = eduUpdate.EndDate;

                await context.SaveChangesAsync();
                return Results.Ok(edu);
            });

            app.MapPut("/workexperience/{id:int}", async (int id, WorkExperience workUpdate, ResumeDBContext context) =>
            {
                var work = await context.WorkExperiences.FindAsync(id);
                if (work == null) return Results.NotFound();

                if (!string.IsNullOrWhiteSpace(workUpdate.JobTitle)) work.JobTitle = workUpdate.JobTitle;
                if (!string.IsNullOrWhiteSpace(workUpdate.Company)) work.Company = workUpdate.Company;
                if (!string.IsNullOrWhiteSpace(workUpdate.Description)) work.Description = workUpdate.Description;
                if (workUpdate.Year != 0) work.Year = workUpdate.Year;

                await context.SaveChangesAsync();
                return Results.Ok(work);
            });

            app.MapDelete("/education/{id:int}", async (int id, ResumeDBContext context) =>
            {
                var edu = await context.Educations.FindAsync(id);
                if (edu == null) return Results.NotFound();

                context.Educations.Remove(edu);
                await context.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapDelete("/workexperience/{id:int}", async (int id, ResumeDBContext context) =>
            {
                var work = await context.WorkExperiences.FindAsync(id);
                if (work == null) return Results.NotFound();

                context.WorkExperiences.Remove(work);
                await context.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapGet("/github/{username}", async (string username, IHttpClientFactory httpClientFactory) =>
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MinimalAPIApp");

                var response = await client.GetAsync($"https://api.github.com/users/{username}/repos");

                if (!response.IsSuccessStatusCode)
                    return Results.StatusCode((int)response.StatusCode);

                var json = await response.Content.ReadFromJsonAsync<List<GitHubRepo>>();

                var result = json?.Select(repo => new
                {
                    Name = repo.Name,
                    Language = string.IsNullOrWhiteSpace(repo.Language) ? "okänt" : repo.Language,
                    Description = string.IsNullOrWhiteSpace(repo.Description) ? "saknas" : repo.Description,
                    Url = repo.HtmlUrl
                });

                return Results.Ok(result);
            });


            app.Run();
        }
    }
}
