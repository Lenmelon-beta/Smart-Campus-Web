using Microsoft.EntityFrameworkCore;
using CustomAuth.Models;

namespace CustomAuth.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
        public DbSet<ChatMessage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("Chat", schema: "dbo");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).HasColumnName("ID").UseIdentityColumn();
                entity.Property(e => e.From).HasColumnName("From").IsRequired();
                entity.Property(e => e.To).HasColumnName("To").IsRequired();
                entity.Property(e => e.Message).HasColumnName("Message").IsRequired();
            });
        }
    }
}