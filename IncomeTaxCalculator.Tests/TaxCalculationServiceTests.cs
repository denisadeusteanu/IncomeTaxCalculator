using IncomeTaxCalculator.Repositories;
using IncomeTaxCalculator.Services;
using Moq;
using Persistence.Models;

namespace IncomeTaxCalculator.Tests
{
    public class TaxCalculationServiceTests
    {
        private readonly Mock<ITaxBandSqlRepository> _repositoryMock;
        private readonly TaxCalculationService _taxCalculationService;

        public TaxCalculationServiceTests()
        {
            _repositoryMock = new Mock<ITaxBandSqlRepository>();

            var taxBands = new List<TaxBand>
        {
            new TaxBand { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
            new TaxBand { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
            new TaxBand { Name = "Tax Band C", LowerLimit = 20000, UpperLimit = null, TaxRate = 40 }
        };

            _repositoryMock.Setup(repo => repo.GetAllTaxBands()).ReturnsAsync(taxBands);

            _taxCalculationService = new TaxCalculationService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CalculateTax_ReturnsCorrectResult_ForSalary40000()
        {
            // Arrange
            var grossAnnualSalary = 40000m;

            // Act
            var result = await _taxCalculationService.CalculateTax(grossAnnualSalary);

            // Assert
            Assert.Equal(40000m, result.GrossAnnualSalary);
            Assert.Equal(3333.33m, result.GrossMonthlySalary);
            Assert.Equal(29000m, result.NetAnnualSalary);
            Assert.Equal(2416.67m, result.NetMonthlySalary);
            Assert.Equal(11000.00m, result.AnnualTaxPaid);
            Assert.Equal(916.67m, result.MonthlyTaxPaid);
        }

        [Fact]
        public async Task CalculateTax_ReturnsCorrectResult_ForSalary10000()
        {
            // Arrange
            var grossAnnualSalary = 10000m;

            // Act
            var result = await _taxCalculationService.CalculateTax(grossAnnualSalary);

            // Assert
            Assert.Equal(10000m, result.GrossAnnualSalary);
            Assert.Equal(833.33m, result.GrossMonthlySalary);
            Assert.Equal(9000m, result.NetAnnualSalary);
            Assert.Equal(750m, result.NetMonthlySalary);
            Assert.Equal(1000m, result.AnnualTaxPaid);
            Assert.Equal(83.33m, result.MonthlyTaxPaid);
        }

        [Fact]
        public async Task CalculateTax_ThrowsArgumentException_WhenGrossAnnualSalaryIsZeroOrNegative()
        {
            // Arrange
            decimal grossAnnualSalary = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _taxCalculationService.CalculateTax(grossAnnualSalary)
            );

            Assert.Equal("Gross annual salary must be positive. (Parameter 'grossAnnualSalary')", exception.Message);
        }
    }
}
