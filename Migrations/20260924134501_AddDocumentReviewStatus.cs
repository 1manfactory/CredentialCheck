using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CredentialCheck.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentReviewStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewStatus",
                table: "QualificationDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "QualificationDocuments");
        }
    }
}
