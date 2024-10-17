using Microsoft.AspNetCore.Mvc;
using TechFix_backend.Models;
using TechFix_backend.Services;

namespace TechFix_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierQuoteController : ControllerBase
    {
        private readonly ISupplierQuoteService _supplierQuoteService;

        public SupplierQuoteController(ISupplierQuoteService supplierQuoteService)
        {
            _supplierQuoteService = supplierQuoteService;
        }

        // POST: api/supplier-quote
        [HttpPost]
        public async Task<IActionResult> CreateSupplierQuote([FromBody] SupplierQuote supplierQuote)
        {
            try
            {
                var createdSupplierQuote = await _supplierQuoteService.CreateSupplierQuoteAsync(supplierQuote);
                return Ok(createdSupplierQuote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
