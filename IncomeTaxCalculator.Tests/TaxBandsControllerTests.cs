using AutoMapper;
using IncomeTaxCalculator.Controllers;
using IncomeTaxCalculator.DTOs;
using IncomeTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Models;

namespace IncomeTaxCalculator.Tests
{
    public class TaxBandsControllerTests
    {
        private readonly Mock<ITaxBandService> _taxBandServiceMock;
        private readonly Mock<ILogger<TaxBandsController>> _loggerMock;
        private readonly TaxBandsController _controllerMock;
        private readonly Mock<IMapper> _mapperMock;

        public TaxBandsControllerTests()
        {
            _taxBandServiceMock = new Mock<ITaxBandService>();
            _loggerMock = new Mock<ILogger<TaxBandsController>>();
            _mapperMock = new Mock<IMapper>();
            _controllerMock = new TaxBandsController(_taxBandServiceMock.Object, _loggerMock.Object, _mapperMock.Object);

        }

        [Fact]
        public async Task AddTaxBands_ReturnsOk_WithValidResult()
        {
            // Arrange
            var taxBandDtos = new List<TaxBandDto>
            {
                new TaxBandDto { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 10 },
                new TaxBandDto { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 }
            };
            var taxBands = new List<TaxBand>
            {
                new TaxBand { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 10 },
                new TaxBand { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 }
            };

            _mapperMock.Setup(m => m.Map<List<TaxBand>>(It.IsAny<List<TaxBandDto>>())).Returns(taxBands);


            // Act
            await _controllerMock.AddTaxBandsAsync(taxBandDtos);

            // Assert
            _taxBandServiceMock.Verify(r => r.AddTaxBandsAsync(taxBands), Times.Once);

        }

        [Fact]
        public async Task AddTaxBandsAsync_ReturnsBadRequest_WhenTaxBandsAreNull()
        {
            // Act
            var result = await _controllerMock.AddTaxBandsAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Tax bands list cannot be null or empty.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddTaxBandsAsync_ReturnsBadRequest_WhenTaxBandsIsEmpty()
        {
            // Arrange
            var emptyTaxBands = new List<TaxBandDto>();

            // Act
            var result = await _controllerMock.AddTaxBandsAsync(emptyTaxBands);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Tax bands list cannot be null or empty.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddTaxBandsAsync_ReturnsServerError_WhenAddingTaxBandsFails()
        {
            // Arrange
            var taxBandDtos = new List<TaxBandDto>
            {
                new TaxBandDto { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 10 },
                new TaxBandDto { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 }
            };
            var taxBands = new List<TaxBand>
            {
                new TaxBand { Name = "Tax Band A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 10 },
                new TaxBand { Name = "Tax Band B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 }
            };
            _mapperMock.Setup(m => m.Map<List<TaxBand>>(It.IsAny<List<TaxBandDto>>())).Returns(taxBands);
            _taxBandServiceMock.Setup(service => service.AddTaxBandsAsync(taxBands)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controllerMock.AddTaxBandsAsync(taxBandDtos);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            Assert.Equal("An unexpected error occurred while processing the request.", serverErrorResult.Value);
        }

    }
}
