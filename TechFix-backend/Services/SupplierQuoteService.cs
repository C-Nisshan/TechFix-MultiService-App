using TechFix_backend.Models;
using System.Threading.Tasks;
using TechFix_backend.Data;

namespace TechFix_backend.Services
{
    public interface ISupplierQuoteService
    {
        Task<SupplierQuote> CreateSupplierQuoteAsync(SupplierQuote supplierQuote);
    }

    public class SupplierQuoteService : ISupplierQuoteService
    {
        private readonly TechFixDbContext _context;

        public SupplierQuoteService(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierQuote> CreateSupplierQuoteAsync(SupplierQuote supplierQuote)
        {
            _context.SupplierQuotes.Add(supplierQuote);
            await _context.SaveChangesAsync();
            return supplierQuote;
        }
    }
}
