using Microsoft.AspNetCore.Mvc;
using Supplier_backend.Services;
using Supplier_backend.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Supplier_backend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Create a new product with an image
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productDto, [FromForm] IFormFile? image)
        {
            byte[]? imageData = null;

            if (image != null)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            await _productService.AddProductAsync(productDto, imageData);
            return Ok("Product created successfully.");
        }

        // Update an existing product with an optional image
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto productDto, [FromForm] IFormFile? image)
        {
            byte[]? imageData = null;

            if (image != null)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            try
            {
                await _productService.UpdateProductAsync(id, productDto, imageData);
                return Ok("Product updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product not found.");
            }
        }

        // Get All Products (For client access)
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int supplierId)
        {
            var products = await _productService.GetAllProductsAsync(supplierId);
            return Ok(products);
        }

        // Get a single product by id (For client access)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Delete a product and notify client system
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok("Product deleted successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Product not found.");
            }
        }
    }
}
