using IncomeTaxCalculator.Repositories;
using IncomeTaxCalculator.Services;
using Moq;
using Persistence.Models;

namespace IncomeTaxCalculator.Tests
{
    public class TaxBandServiceTests
    {
        private readonly Mock<ITaxBandSqlRepository> _repositoryMock;
        private readonly TaxBandService _taxBandService;

        public TaxBandServiceTests()
        {
            _repositoryMock = new Mock<ITaxBandSqlRepository>();
            _taxBandService = new TaxBandService(_repositoryMock.Object);
        }

        [Fact]
        public async Task AddTaxBandsAsync_CallsRepositoryWithValidData()
        {
            // Arrange
            var taxBands = new List<TaxBand>
            {
                new TaxBand { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
                new TaxBand { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 }
            };

            // Act
            await _taxBandService.AddTaxBandsAsync(taxBands);

            // Assert
            _repositoryMock.Verify(r => r.InsertTaxBandsAsync(taxBands), Times.Once);
        }
    }
}
