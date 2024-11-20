using AutoMapper;
using IncomeTaxCalculator.DTOs;
using IncomeTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;

namespace IncomeTaxCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxBandsController : ControllerBase
    {
        private readonly ITaxBandService _taxBandService;
        private readonly ILogger<TaxBandsController> _logger;
        private readonly IMapper _mapper;

        public TaxBandsController(ITaxBandService taxBandService, ILogger<TaxBandsController> logger, IMapper mapper)
        {
            _taxBandService = taxBandService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTaxBandsAsync(List<TaxBandDto> taxBands)
        {
            if (taxBands == null || !taxBands.Any())
            {
                _logger.LogWarning("Attempt to add empty or null tax bands list.");
                return BadRequest("Tax bands list cannot be null or empty.");
            }

            try
            {
                var taxBandEntities = _mapper.Map<List<TaxBand>>(taxBands);
                await _taxBandService.AddTaxBandsAsync(taxBandEntities);
                _logger.LogInformation("Successfully added tax bands.");
                return Ok("Tax bands added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding tax bands.");
                return StatusCode(500, "An unexpected error occurred while processing the request.");
            }
        }
    }
}
