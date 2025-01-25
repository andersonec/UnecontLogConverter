using Microsoft.EntityFrameworkCore;
using UnecontLogConverter.Entities;

namespace UnecontLogConverter.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogTransformed> LogsTransformed { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>()
                .HasOne(l => l.TransformedLog)
                .WithOne(t => t.OriginalLog)
                .HasForeignKey<LogTransformed>(t => t.LogId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}