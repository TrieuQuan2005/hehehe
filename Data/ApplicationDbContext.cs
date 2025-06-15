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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.MaNhapHoc);

            modelBuilder.Entity<UserForm>()
                .HasKey(f => f.MaNhapHoc);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Form)
                .WithOne(f => f.User)
                .HasForeignKey<UserForm>(f => f.MaNhapHoc);
        }
    }

}