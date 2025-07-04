﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hehehe.Data;

#nullable disable

namespace hehehe.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250701010911_userandstudent")]
    partial class userandstudent
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("hehehe.Models.InitUserForm", b =>
                {
                    b.Property<string>("MaNhapHoc")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChoOHienNay")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DanToc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KhuVuc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NganhDaoTao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiSinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoiThuongTru")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhapHoc");

                    b.ToTable("EduMinisUsers");
                });

            modelBuilder.Entity("hehehe.Models.User", b =>
                {
                    b.Property<string>("MaNhapHoc")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhapHoc");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("hehehe.Models.UserForm", b =>
                {
                    b.Property<string>("MaNhapHoc")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BaoTinChoAi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChoOHienNay")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DanToc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiaChiLienHe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DoiTuongUuTien")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTenBo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTenMe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("KhuVuc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NamNhapHoc")
                        .HasColumnType("int");

                    b.Property<int?>("NamNhapNgu")
                        .HasColumnType("int");

                    b.Property<int>("NamTotNghiepTHPT")
                        .HasColumnType("int");

                    b.Property<int?>("NamXuatNgu")
                        .HasColumnType("int");

                    b.Property<string>("NganhDaoTao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayVaoDang")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayVaoDoan")
                        .HasColumnType("datetime2");

                    b.Property<string>("NgheNghiepBo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NgheNghiepMe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoiSinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoiThuongTru")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoaiBo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoaiMe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TuoiBo")
                        .HasColumnType("int");

                    b.Property<int>("TuoiMe")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("UploadedFiles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhapHoc");

                    b.ToTable("StudentForms");
                });

            modelBuilder.Entity("hehehe.Models.UserStdInfo", b =>
                {
                    b.Property<string>("MaNhapHoc")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSinhVien")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MsAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MsPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QlPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhapHoc");

                    b.ToTable("UserStdInfos");
                });

            modelBuilder.Entity("hehehe.Models.YeuCauDinhChinh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("BiTuChoi")
                        .HasColumnType("bit");

                    b.Property<bool>("DaDuyet")
                        .HasColumnType("bit");

                    b.Property<string>("FileMinhChung")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GhiChuAdmin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GiaTriMoi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaNhapHoc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayGui")
                        .HasColumnType("datetime2");

                    b.Property<string>("TruongCanDinhChinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("YeuCauDinhChinhs");
                });

            modelBuilder.Entity("hehehe.Models.InitUserForm", b =>
                {
                    b.HasOne("hehehe.Models.User", "User")
                        .WithOne("InitUserForm")
                        .HasForeignKey("hehehe.Models.InitUserForm", "MaNhapHoc")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("hehehe.Models.UserForm", b =>
                {
                    b.HasOne("hehehe.Models.InitUserForm", "InitUserForm")
                        .WithOne("UserForm")
                        .HasForeignKey("hehehe.Models.UserForm", "MaNhapHoc")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("hehehe.Models.User", "User")
                        .WithOne("UserForm")
                        .HasForeignKey("hehehe.Models.UserForm", "MaNhapHoc")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InitUserForm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("hehehe.Models.UserStdInfo", b =>
                {
                    b.HasOne("hehehe.Models.User", "User")
                        .WithOne("UserStdInfo")
                        .HasForeignKey("hehehe.Models.UserStdInfo", "MaNhapHoc")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("hehehe.Models.InitUserForm", b =>
                {
                    b.Navigation("UserForm");
                });

            modelBuilder.Entity("hehehe.Models.User", b =>
                {
                    b.Navigation("InitUserForm");

                    b.Navigation("UserForm");

                    b.Navigation("UserStdInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
