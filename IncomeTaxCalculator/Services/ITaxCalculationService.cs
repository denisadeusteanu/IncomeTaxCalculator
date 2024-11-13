using IncomeTaxCalculator.DTOs;

namespace IncomeTaxCalculator.Services
{
    public interface ITaxCalculationService
    {
        Task<SalaryCalculationResultDto> CalculateTax(decimal grossAnnualSalary);
    }
}
