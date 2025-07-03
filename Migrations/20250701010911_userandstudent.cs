using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hehehe.Migrations
{
    /// <inheritdoc />
    public partial class userandstudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStdInfos",
                columns: table => new
                {
                    MaNhapHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSinhVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QlPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MsAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MsPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStdInfos", x => x.MaNhapHoc);
                    table.ForeignKey(
                        name: "FK_UserStdInfos_Users_MaNhapHoc",
                        column: x => x.MaNhapHoc,
                        principalTable: "Users",
                        principalColumn: "MaNhapHoc");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStdInfos");
        }
    }
}
