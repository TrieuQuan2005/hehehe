using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hehehe.Migrations
{
    /// <inheritdoc />
    public partial class UserDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    MaNhapHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.MaNhapHoc);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauDinhChinhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhapHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TruongCanDinhChinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaTriMoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileMinhChung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    BiTuChoi = table.Column<bool>(type: "bit", nullable: false),
                    GhiChuAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauDinhChinhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentForms",
                columns: table => new
                {
                    MaNhapHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiSinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DanToc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiThuongTru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoOHienNay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoiTuongUuTien = table.Column<bool>(type: "bit", nullable: false),
                    KhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamNhapHoc = table.Column<int>(type: "int", nullable: false),
                    NamTotNghiepTHPT = table.Column<int>(type: "int", nullable: false),
                    NamNhapNgu = table.Column<int>(type: "int", nullable: true),
                    NamXuatNgu = table.Column<int>(type: "int", nullable: true),
                    NgayVaoDoan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayVaoDang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NganhDaoTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTenBo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuoiBo = table.Column<int>(type: "int", nullable: false),
                    NgheNghiepBo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoaiBo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTenMe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuoiMe = table.Column<int>(type: "int", nullable: false),
                    NgheNghiepMe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoaiMe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaoTinChoAi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiLienHe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedFiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentForms", x => x.MaNhapHoc);
                    table.ForeignKey(
                        name: "FK_StudentForms_Users_MaNhapHoc",
                        column: x => x.MaNhapHoc,
                        principalTable: "Users",
                        principalColumn: "MaNhapHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EduMinisUsers",
                columns: table => new
                {
                    MaNhapHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiSinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DanToc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiThuongTru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoOHienNay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NganhDaoTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduMinisUsers", x => x.MaNhapHoc);
                    table.ForeignKey(
                        name: "FK_EduMinisUsers_StudentForms_MaNhapHoc",
                        column: x => x.MaNhapHoc,
                        principalTable: "StudentForms",
                        principalColumn: "MaNhapHoc");
                    table.ForeignKey(
                        name: "FK_EduMinisUsers_Users_MaNhapHoc",
                        column: x => x.MaNhapHoc,
                        principalTable: "Users",
                        principalColumn: "MaNhapHoc");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EduMinisUsers");

            migrationBuilder.DropTable(
                name: "YeuCauDinhChinhs");

            migrationBuilder.DropTable(
                name: "StudentForms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
