using CustomAuth.Models;
using Microsoft.EntityFrameworkCore;
using CustomAuth.Models;

namespace CustomAuth.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().ToTable("UserAccounts", schema: "dbo");
            modelBuilder.Entity<UserAccount>()
                .HasKey(u => u.Username);
        }
    }
}