using IncomeTaxCalculator.Repositories;
using Persistence.Models;

namespace IncomeTaxCalculator.Services
{
    public class TaxBandService : ITaxBandService
    {
        private readonly ITaxBandSqlRepository _taxBandSqlRepository;

        public TaxBandService(ITaxBandSqlRepository taxBandSqlRepository)
        {
            _taxBandSqlRepository = taxBandSqlRepository;
        }

        public async Task AddTaxBandsAsync(List<TaxBand> taxBands)
        {
            await _taxBandSqlRepository.InsertTaxBandsAsync(taxBands);
        }
    }
}
