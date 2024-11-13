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
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);

            _context.TaxBands.AddRange(
                     new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
                     new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
                     new TaxBand { Id = Guid.NewGuid(), Name = "Tax Band C", LowerLimit = 20000, UpperLimit = null, TaxRate = 40 }
                                      );
            _context.SaveChanges();
            _repository = new TaxBandSqlRepository( _context );
        }

        [Fact]
        public async Task GetAllTaxBands_ReturnsAllTaxBands()
        {
            //Act
            var result = await _repository.GetAllTaxBands();

            //Assert
            Assert.Equal(3, result.Count);
        }
    }
}
