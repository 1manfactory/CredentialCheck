using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CredentialCheck.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "QualificationDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSizeBytes",
                table: "QualificationDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "QualificationDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoragePath",
                table: "QualificationDocuments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoredFileName",
                table: "QualificationDocuments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "QualificationDocuments");

            migrationBuilder.DropColumn(
                name: "FileSizeBytes",
                table: "QualificationDocuments");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "QualificationDocuments");

            migrationBuilder.DropColumn(
                name: "StoragePath",
                table: "QualificationDocuments");

            migrationBuilder.DropColumn(
                name: "StoredFileName",
                table: "QualificationDocuments");
        }
    }
}
