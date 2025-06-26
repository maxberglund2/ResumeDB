using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResumeDB.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    School = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId_FK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Users_UserId_FK",
                        column: x => x.UserId_FK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    UserId_FK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Users_UserId_FK",
                        column: x => x.UserId_FK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactInfo", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Frontend developer with a passion for UI.", "Alice Andersson" },
                    { 2, "bob@example.com", "Backend developer who loves APIs.", "Bob Bergström" }
                });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "Id", "Degree", "EndDate", "School", "StartDate", "UserId_FK" },
                values: new object[,]
                {
                    { 1, "BSc Computer Science", new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lund University", new DateTime(2018, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "MSc Software Engineering", new DateTime(2020, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "KTH Royal Institute of Technology", new DateTime(2017, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "WorkExperiences",
                columns: new[] { "Id", "Company", "Description", "JobTitle", "UserId_FK", "Year" },
                values: new object[,]
                {
                    { 1, "Creative Tech AB", "Worked with React and Angular.", "Frontend Developer", 1, 2022 },
                    { 2, "API Masters AB", "Developed REST APIs with .NET Core.", "Backend Developer", 2, 2023 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UserId_FK",
                table: "Educations",
                column: "UserId_FK");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_UserId_FK",
                table: "WorkExperiences",
                column: "UserId_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
