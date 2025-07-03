using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hehehe.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EduMinisUsers_StudentForms_MaNhapHoc",
                table: "EduMinisUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentForms_EduMinisUsers_MaNhapHoc",
                table: "StudentForms",
                column: "MaNhapHoc",
                principalTable: "EduMinisUsers",
                principalColumn: "MaNhapHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentForms_EduMinisUsers_MaNhapHoc",
                table: "StudentForms");

            migrationBuilder.AddForeignKey(
                name: "FK_EduMinisUsers_StudentForms_MaNhapHoc",
                table: "EduMinisUsers",
                column: "MaNhapHoc",
                principalTable: "StudentForms",
                principalColumn: "MaNhapHoc");
        }
    }
}
