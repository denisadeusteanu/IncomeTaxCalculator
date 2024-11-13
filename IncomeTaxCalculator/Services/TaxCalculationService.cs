using IncomeTaxCalculator.DTOs;
using IncomeTaxCalculator.Helpers;
using IncomeTaxCalculator.Repositories;
using Persistence.Models;

namespace IncomeTaxCalculator.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly ITaxBandSqlRepository _taxBandSqlRepository;
        public TaxCalculationService(ITaxBandSqlRepository taxBandSqlRepository)
        {
            _taxBandSqlRepository = taxBandSqlRepository;
        }
        public async Task<SalaryCalculationResultDto> CalculateTax(decimal grossAnnualSalary)
        {
            if (grossAnnualSalary <= 0)
            {
                throw new ArgumentException("Gross annual salary must be positive.", nameof(grossAnnualSalary));
            }

            var taxBands = await _taxBandSqlRepository.GetAllTaxBands();

            decimal grossMonthlySalary = NumberHelper.ConvertAnnualToMonthly(grossAnnualSalary);
            decimal remainingSalary = grossAnnualSalary;
            decimal totalTax = 0;

            foreach (var taxBand in taxBands)
            {
                if (remainingSalary == 0) 
                { 
                    return CreateSalaryCalculationResultDto(grossAnnualSalary, grossMonthlySalary, totalTax);
                }

                totalTax += CalculateTaxForBand(taxBand, ref remainingSalary);
            }

            return CreateSalaryCalculationResultDto(grossAnnualSalary, grossMonthlySalary, totalTax);
        }

        private SalaryCalculationResultDto CreateSalaryCalculationResultDto(decimal grossAnnualSalary, decimal grossMonthlySalary, decimal totalTax)
        {
            var netAnnualSalary = grossAnnualSalary - totalTax;
            var netMonthlySalary = NumberHelper.ConvertAnnualToMonthly(netAnnualSalary);
            var monthlyTaxPaid = NumberHelper.ConvertAnnualToMonthly(totalTax);

            return new SalaryCalculationResultDto
            {
                GrossAnnualSalary = grossAnnualSalary,
                GrossMonthlySalary = NumberHelper.RoundToTwoDecimalPlaces(grossMonthlySalary),
                NetAnnualSalary = NumberHelper.RoundToTwoDecimalPlaces(netAnnualSalary),
                NetMonthlySalary = NumberHelper.RoundToTwoDecimalPlaces(netMonthlySalary),
                AnnualTaxPaid = NumberHelper.RoundToTwoDecimalPlaces(totalTax),
                MonthlyTaxPaid = NumberHelper.RoundToTwoDecimalPlaces(monthlyTaxPaid)
            };
        }

        private decimal CalculateTaxForBand(TaxBand taxBand, ref decimal remainingSalary)
        {
            if (taxBand.UpperLimit.HasValue)
            {
                return CalculateTaxForBandWithUpperLimit(taxBand, ref remainingSalary);
            }
            else
            {
                return CalculateTaxAmountForBand(taxBand, remainingSalary);
            }
        }

        private decimal CalculateTaxForBandWithUpperLimit(TaxBand taxBand, ref decimal remainingSalary)
        {
            decimal salaryInBand;

            if (taxBand.UpperLimit < remainingSalary)
            {
                salaryInBand = taxBand.UpperLimit.Value - taxBand.LowerLimit;
                remainingSalary -= salaryInBand;
            }
            else
            {
                salaryInBand = remainingSalary;
                remainingSalary = 0;
            }

            return CalculateTaxAmountForBand(taxBand, salaryInBand);
        }

        private static decimal CalculateTaxAmountForBand(TaxBand taxBand, decimal salaryInBand)
        {
            return taxBand.TaxRate != 0 ? salaryInBand * NumberHelper.CalculatePercentage(taxBand.TaxRate) : 0;
        }
    }
}
