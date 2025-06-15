using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hehehe.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFormSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentForms_Users_UserId",
                table: "StudentForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentForms",
                table: "StudentForms");

            migrationBuilder.DropIndex(
                name: "IX_StudentForms_UserId",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentForms");

            migrationBuilder.RenameColumn(
                name: "StudentCode",
                table: "StudentForms",
                newName: "HoTen");

            migrationBuilder.AddColumn<string>(
                name: "MaNhapHoc",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaNhapHoc",
                table: "StudentForms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "DiemThi",
                table: "StudentForms",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "StudentForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "MaNhapHoc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentForms",
                table: "StudentForms",
                column: "MaNhapHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentForms_Users_MaNhapHoc",
                table: "StudentForms",
                column: "MaNhapHoc",
                principalTable: "Users",
                principalColumn: "MaNhapHoc",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentForms_Users_MaNhapHoc",
                table: "StudentForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentForms",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "MaNhapHoc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaNhapHoc",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "DiemThi",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "StudentForms");

            migrationBuilder.RenameColumn(
                name: "HoTen",
                table: "StudentForms",
                newName: "StudentCode");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentForms",
                table: "StudentForms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentForms_UserId",
                table: "StudentForms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentForms_Users_UserId",
                table: "StudentForms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
