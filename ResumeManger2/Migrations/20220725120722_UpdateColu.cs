using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeManger2.Migrations
{
    public partial class UpdateColu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalExperiences",
                table: "Applicant",
                newName: "TotalExperience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalExperience",
                table: "Applicant",
                newName: "TotalExperiences");
        }
    }
}
