using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hehehe.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadedFilesJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedFilePath",
                table: "StudentForms");

            migrationBuilder.AddColumn<string>(
                name: "UploadedFiles",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedFiles",
                table: "StudentForms");

            migrationBuilder.AddColumn<string>(
                name: "UploadedFilePath",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
