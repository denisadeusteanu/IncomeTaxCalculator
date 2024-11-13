using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

namespace IncomeTaxCalculator.Repositories
{
    public class TaxBandSqlRepository : ITaxBandSqlRepository
    {
        private readonly DataContext _context;
        public TaxBandSqlRepository(DataContext context)
        {
            _context = context;
        }

        public Task<List<TaxBand>> GetAllTaxBands()
        {
            return _context.TaxBands.OrderBy(taxBand => taxBand.LowerLimit).ToListAsync();
        }
    }
}
