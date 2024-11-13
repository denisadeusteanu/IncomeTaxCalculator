using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<TaxBand> TaxBands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxBand>(entity =>
            {
                entity.Property(t => t.Name)
                    .IsRequired();

                entity.Property(t => t.LowerLimit)
                    .IsRequired();

                entity.Property(t => t.TaxRate)
                    .IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
