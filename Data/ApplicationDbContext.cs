using Microsoft.EntityFrameworkCore;
using hehehe.Models;

namespace hehehe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserForm> StudentForms { get; set; }
        
        public DbSet<InitUserForm> EduMinisUsers { get; set; }

        public DbSet<YeuCauDinhChinh> YeuCauDinhChinhs { get; set; }
        
        public DbSet<UserStdInfo> UserStdInfos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasOne(i => i.InitUserForm)
                .WithOne(u => u.User)
                .HasForeignKey<InitUserForm>(i => i.MaNhapHoc)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<UserForm>()
                .HasOne(u => u.InitUserForm)
                .WithOne(i => i.UserForm)
                .HasForeignKey<UserForm>(u => u.MaNhapHoc)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserStdInfo)
                .WithOne(i => i.User)
                .HasForeignKey<UserStdInfo>(i => i.MaNhapHoc)
                .OnDelete(DeleteBehavior.NoAction);
            
        }
        
    }
}