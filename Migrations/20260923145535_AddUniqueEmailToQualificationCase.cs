using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CredentialCheck.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmailToQualificationCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QualificationCases_ApplicantEmail",
                table: "QualificationCases",
                column: "ApplicantEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QualificationCases_ApplicantEmail",
                table: "QualificationCases");
        }
    }
}
