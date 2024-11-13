using Persistence.Models;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.TaxBands.Any()) return;

            var taxBands = new List<TaxBand>
            {
                new TaxBand
            {
                Id = Guid.NewGuid(),
                Name = "Tax Band A",
                LowerLimit = 0,
                UpperLimit = 5000,
                TaxRate = 0
            },
            new TaxBand
            {
                Id = Guid.NewGuid(),
                Name = "Tax Band B",
                LowerLimit = 5000,
                UpperLimit = 20000,
                TaxRate = 20
            },
            new TaxBand
            {
                Id = Guid.NewGuid(),
                Name = "Tax Band C",
                LowerLimit = 20000,
                UpperLimit = null,
                TaxRate = 40
            }
            };
            await context.TaxBands.AddRangeAsync(taxBands);
            await context.SaveChangesAsync();
        }
    }
}
