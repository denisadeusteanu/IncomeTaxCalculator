using IncomeTaxCalculator.Controllers;
using IncomeTaxCalculator.DTOs;
using IncomeTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace IncomeTaxCalculator.Tests
{
    public class TaxCalculatorControllerTests
    {
        private readonly Mock<ITaxCalculationService> _taxCalculationServiceMock;
        private readonly Mock<ILogger<TaxCalculatorController>> _loggerMock;
        private readonly TaxCalculatorController _controllerMock;

        public TaxCalculatorControllerTests()
        {
            _taxCalculationServiceMock = new Mock<ITaxCalculationService>();
            _loggerMock = new Mock<ILogger<TaxCalculatorController>>();
            _controllerMock = new TaxCalculatorController(_taxCalculationServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CalculateTax_ReturnsOk_WithValidResult()
        {
            // Arrange
            decimal grossAnnualSalary = 40000m;
            var expectedResult = new SalaryCalculationResultDto
            {
                GrossAnnualSalary = grossAnnualSalary,
                GrossMonthlySalary = 3333.33m,
                NetAnnualSalary = 29000m,
                NetMonthlySalary = 2416.67m,
                AnnualTaxPaid = 11000.00m,
                MonthlyTaxPaid = 916.67m
            };
            _taxCalculationServiceMock.Setup(service => service.CalculateTax(grossAnnualSalary)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controllerMock.CalculateTax(grossAnnualSalary);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task CalculateTax_ReturnsNotFound_WhenResultIsNull()
        {
            // Arrange
            decimal grossAnnualSalary = 50000m;
            var expectedResult = new SalaryCalculationResultDto();
            _taxCalculationServiceMock.Setup(service => service.CalculateTax(grossAnnualSalary)).ReturnsAsync((SalaryCalculationResultDto)null);

            // Act
            var result = await _controllerMock.CalculateTax(grossAnnualSalary);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Tax calculation could not be performed.", notFoundResult.Value);
        }

        [Fact]
        public async Task CalculateTax_ReturnsServerError_OnException()
        {
            // Arrange
            decimal grossAnnualSalary = 0;
            _taxCalculationServiceMock.Setup(service => service.CalculateTax(grossAnnualSalary)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controllerMock.CalculateTax(grossAnnualSalary);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            Assert.Equal("An unexpected error occurred while processing the request.", serverErrorResult.Value);
        }
    }
}
