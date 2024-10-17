using TechFix_backend.Data;
using TechFix_backend.Models;
using TechFix_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this namespace

namespace TechFix_backend.Services
{
    public class ProductRefService
    {
        private readonly TechFixDbContext _context;
        private readonly ILogger<ProductRefService> _logger; // Add logger dependency

        // Constructor with injected logger
        public ProductRefService(TechFixDbContext context, ILogger<ProductRefService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Create or update product reference
        public async Task CreateProductRefAsync(ProductRefDto productRefDto)
        {
            try
            {
                // Check if Supplier exists before adding ProductRef
                var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == productRefDto.SupplierId);
                if (!supplierExists)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} does not exist.", productRefDto.SupplierId); // Log warning
                    throw new ArgumentException("Invalid SupplierId.");
                }

                var newProductRef = new ProductRef
                {
                    ProductId = productRefDto.ProductId,
                    ProductName = productRefDto.ProductName,
                    SupplierId = productRefDto.SupplierId
                };

                _context.ProductRefs.Add(newProductRef);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created product reference with ProductId {ProductId}", productRefDto.ProductId); // Log success
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product reference for ProductId {ProductId}", productRefDto.ProductId); // Log error
                throw;
            }
        }

        // Update an existing product reference
        public async Task UpdateProductRefAsync(ProductRefUpdateDto productRefUpdateDto, int productId)
        {
            try
            {
                var existingProductRef = await _context.ProductRefs
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (existingProductRef == null)
                {
                    _logger.LogWarning("Product reference with ID {ProductId} not found.", productId); // Log warning
                    throw new KeyNotFoundException("Product reference not found.");
                }

                // Update only the fields provided in the DTO
                if (!string.IsNullOrEmpty(productRefUpdateDto.ProductName))
                {
                    existingProductRef.ProductName = productRefUpdateDto.ProductName;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated product reference with ProductId {ProductId}", productId); // Log success
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product reference with ProductId {ProductId}", productId); // Log error
                throw;
            }
        }

        // Delete product reference by ProductId
        public async Task DeleteProductRefAsync(int productId)
        {
            try
            {
                var productRef = await _context.ProductRefs.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (productRef == null)
                {
                    _logger.LogWarning("Product reference with ID {ProductId} not found.", productId); // Log warning
                    throw new KeyNotFoundException("Product reference not found.");
                }

                _context.ProductRefs.Remove(productRef);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted product reference with ProductId {ProductId}", productId); // Log success
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product reference with ProductId {ProductId}", productId); // Log error
                throw;
            }
        }

        // Get a product reference by ProductId
        public async Task<ProductRef?> GetProductRefByIdAsync(int productId)
        {
            try
            {
                var productRef = await _context.ProductRefs.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (productRef == null)
                {
                    _logger.LogWarning("Product reference with ID {ProductId} not found.", productId); // Log warning
                }
                return productRef;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product reference with ProductId {ProductId}", productId); // Log error
                throw;
            }
        }

        // Get all Ids and name from ProductRefs
        public async Task<List<ProductRefDto>> GetAllProductRefsAsync()
        {
            try
            {
                var productRefs = await _context.ProductRefs
                    .Select(p => new ProductRefDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName
                    })
                    .ToListAsync();

                if (productRefs == null || !productRefs.Any())
                {
                    _logger.LogWarning("No product references found.");
                    return new List<ProductRefDto>(); // Return empty list if no products found
                }

                _logger.LogInformation("Retrieved {Count} product references.", productRefs.Count);
                return productRefs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all product references.");
                throw;
            }
        }

    }
}
