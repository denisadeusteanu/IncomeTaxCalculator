using IncomeTaxCalculator.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

namespace IncomeTaxCalculator.Tests
{
    public class TaxBandSqlRepositoryTests
    {
        private readonly ITaxBandSqlRepository _repository;
        private readonly DataContext _context;

        public TaxBandSqlRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase-{Guid.NewGuid()}")
                .Options;

            _context = new DataContext(options);
            _repository = new TaxBandSqlRepository(_context);
        }

        [Fact]
        public async Task GetAllTaxBands_ReturnsAllTaxBands()
        {
            //Arrange

            _context.TaxBands.AddRange(
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band C", LowerLimit = 20000, UpperLimit = null, TaxRate = 40 }
                          );
            _context.SaveChanges();

            //Act
            var result = await _repository.GetAllTaxBands();

            //Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task InsertTaxBandsAsync_ShouldInsertMultipleTaxBands()
        {
            // Arrange

            var taxBands = new List<TaxBand>
            {
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
             new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band C", LowerLimit = 20000, UpperLimit = null, TaxRate = 40 }
            };

            // Act
            await _repository.InsertTaxBandsAsync(taxBands);

            // Assert
            var insertedTaxBands = await _context.TaxBands.ToListAsync();
            Assert.Equal(3, insertedTaxBands.Count);

            Assert.Contains(insertedTaxBands, tb => tb.LowerLimit == 0 && tb.UpperLimit == 5000 && tb.TaxRate == 0);
            Assert.Contains(insertedTaxBands, tb => tb.LowerLimit == 5000 && tb.UpperLimit == 20000 && tb.TaxRate == 20);
            Assert.Contains(insertedTaxBands, tb => tb.LowerLimit == 20000 && tb.UpperLimit == null && tb.TaxRate == 40);
        }

    }
}
