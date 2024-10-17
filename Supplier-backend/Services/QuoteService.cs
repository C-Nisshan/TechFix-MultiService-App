using Supplier_backend.Data;
using Supplier_backend.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Supplier_backend.Services
{
    public interface IQuoteService
    {
        Task<Quote> CreateQuoteAsync(Quote quote);
        Task<bool> SendQuoteToTechFixAsync(Quote quote);
    }

    public class QuoteService : IQuoteService
    {
        private readonly SupplierDbContext _context;
        private readonly HttpClient _httpClient;

        public QuoteService(SupplierDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<Quote> CreateQuoteAsync(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
        }

        public async Task<bool> SendQuoteToTechFixAsync(Quote quote)
        {
            // Prepare the payload for TechFix-backend
            var supplierQuote = new
            {
                RFQId = quote.RFQId,
                SupplierId = quote.ClientId, // Assuming ClientId corresponds to SupplierId in TechFix
                TotalPrice = quote.TotalPrice,
                DeliveryTime = quote.DeliveryTime,
                QuoteReceivedDate = quote.ResponseDate,
                SupplierQuoteItems = quote.QuoteItems.Select(qi => new
                {
                    ProductId = qi.ProductId,
                    QuotedPrice = qi.QuotedPrice,
                    AvailableQuantity = qi.AvailableQuantity,
                    Discount = qi.Discount
                }).ToList()
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(supplierQuote), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://techfix-backend/api/supplier-quote", jsonContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending quote to TechFix: {ex.Message}");
                return false;
            }
        }
    }
}
