using Microsoft.AspNetCore.Mvc;
using Supplier_backend.Models;
using Supplier_backend.Services;

namespace Supplier_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        // POST: api/quote/createQuote
        [HttpPost("{rfqId}/createQuote")]
        public async Task<IActionResult> CreateQuote(int rfqId, [FromBody] Quote quote)
        {
            if (rfqId != quote.RFQId)
            {
                return BadRequest("RFQ ID mismatch");
            }

            try
            {
                // Save the quote in Supplier-backend
                var createdQuote = await _quoteService.CreateQuoteAsync(quote);

                // Send the quote to TechFix-backend
                var isSentToTechFix = await _quoteService.SendQuoteToTechFixAsync(createdQuote);

                if (!isSentToTechFix)
                {
                    return StatusCode(500, "Failed to send quote to TechFix");
                }

                return Ok(createdQuote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
