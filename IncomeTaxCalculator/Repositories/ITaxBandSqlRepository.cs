using Persistence.Models;

namespace IncomeTaxCalculator.Repositories
{
    public interface ITaxBandSqlRepository
    {
        Task<List<TaxBand>> GetAllTaxBands();
        Task InsertTaxBandsAsync(List<TaxBand> taxBands);
    }
}
