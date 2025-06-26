using EzraDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzraDemo.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Id)
                .HasConversion(
                v => v.ToString(),      // Guid → string for storage
                v => Guid.Parse(v));
        }
    }
}
