using Microsoft.AspNetCore.Mvc;
using TechFix_backend.Services;
using TechFix_backend.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechFix_backend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductRefController : ControllerBase
    {
        private readonly ProductRefService _productRefService;

        public ProductRefController(ProductRefService productRefService)
        {
            _productRefService = productRefService;
        }

        // Create product reference
        [HttpPost]
        public async Task<IActionResult> CreateProductRef([FromBody] ProductRefDto productRefDto)
        {
            try
            {
                await _productRefService.CreateProductRefAsync(productRefDto);
                return Ok("Product reference created successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                // Log detailed exception
                return StatusCode(500, $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Update product reference
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProductRef([FromBody] ProductRefUpdateDto productRefUpdateDto, int productId)
        {
            try
            {
                await _productRefService.UpdateProductRefAsync(productRefUpdateDto, productId);
                return Ok("Product reference updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product reference not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error in updating productRef: {ex.Message}");
            }
        }

        // Delete product reference by ProductId
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductRef(int productId)
        {
            try
            {
                await _productRefService.DeleteProductRefAsync(productId);
                return Ok("Product reference deleted successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product reference not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error in deleting productRef: {ex.Message}");
            }
        }

        // Optionally, you can add a Get endpoint if needed
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductRef(int productId)
        {
            var productRef = await _productRefService.GetProductRefByIdAsync(productId);
            if (productRef == null)
            {
                return NotFound();
            }
            return Ok(productRef);
        }

        // GET: api/ProductRefs
        [HttpGet]
        public async Task<IActionResult> GetAllProductRefs()
        {
            var productRefs = await _productRefService.GetAllProductRefsAsync();
            if (productRefs == null || !productRefs.Any())
            {
                return NotFound("No product references found.");
            }
            return Ok(productRefs);
        }
    }
}
