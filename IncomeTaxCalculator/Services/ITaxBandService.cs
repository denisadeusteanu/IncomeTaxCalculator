using Persistence.Models;

namespace IncomeTaxCalculator.Services
{
    public interface ITaxBandService
    {
        Task AddTaxBandsAsync(List<TaxBand> taxBands);
    }
}
