using IncomeTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxCalculatorController :ControllerBase
    {
        private readonly ITaxCalculationService _taxCalculationService;
        private readonly ILogger<TaxCalculatorController> _logger;

        public TaxCalculatorController(ITaxCalculationService taxCalculationService, ILogger<TaxCalculatorController> logger)
        {
            _taxCalculationService = taxCalculationService;
            _logger = logger;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateTax(decimal grossAnnualSalary)
        {
            try
            {
                _logger.LogInformation("Received request to calculate tax for gross annual salary: {GrossAnnualSalary}", grossAnnualSalary);

                var result = await _taxCalculationService.CalculateTax(grossAnnualSalary);

                if (result is null)
                {
                    return NotFound("Tax calculation could not be performed.");
                }

                _logger.LogInformation("Tax calculation successful for salary {GrossAnnualSalary}", grossAnnualSalary);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating tax for salary {GrossAnnualSalary}", grossAnnualSalary);
                return StatusCode(500, "An unexpected error occurred while processing the request.");
            }
        }
    }
}
