using Supplier_backend.Data;
using Supplier_backend.Models;
using Supplier_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace Supplier_backend.Services
{
    public class ProductService
    {
        private readonly SupplierDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger; 

        public ProductService(SupplierDbContext context, HttpClient httpClient, ILogger<ProductService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task AddProductAsync(ProductCreateDto productCreateDto, byte[]? imageData)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = productCreateDto.Name,
                    Description = productCreateDto.Description,
                    Category = productCreateDto.Category,
                    Price = productCreateDto.Price,
                    StockLevel = productCreateDto.StockLevel,
                    ImageData = imageData
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                // Log information after adding the product
                _logger.LogInformation("Product '{ProductName}' added successfully.", newProduct.Name);

                // Notify TechFix-backend about the new product
                await NotifyTechFixBackendOnCreateAsync(newProduct);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while adding the product.");
                throw;
            }
        }

        public async Task UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto, byte[]? imageData)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(productId);

                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                    throw new KeyNotFoundException("Product not found.");
                }

                if (!string.IsNullOrEmpty(productUpdateDto.Name))
                {
                    existingProduct.Name = productUpdateDto.Name;
                    await NotifyTechFixBackendOnUpdateAsync(existingProduct);
                }

                if (!string.IsNullOrEmpty(productUpdateDto.Description))
                {
                    existingProduct.Description = productUpdateDto.Description;
                }

                if (!string.IsNullOrEmpty(productUpdateDto.Category))
                {
                    existingProduct.Category = productUpdateDto.Category;
                }

                if (productUpdateDto.Price.HasValue)
                {
                    existingProduct.Price = productUpdateDto.Price.Value;
                }

                if (productUpdateDto.StockLevel.HasValue)
                {
                    existingProduct.StockLevel = productUpdateDto.StockLevel.Value;
                }

                if (imageData != null)
                {
                    existingProduct.ImageData = imageData;
                }

                await _context.SaveChangesAsync();

                // Log successful update
                _logger.LogInformation("Product '{ProductId}' updated successfully.", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product ID {ProductId}.", productId);
                throw;
            }
        }

        public async Task<List<ProductResponseDto>> GetAllProductsAsync(int supplierId)
        {
            try
            {
                var products = await _context.Products
                    .Select(p => new ProductResponseDto
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Category = p.Category,
                        Price = p.Price,
                        StockLevel = p.StockLevel,
                        ImageBase64 = p.ImageData != null ? Convert.ToBase64String(p.ImageData) : null
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} products.", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                throw;
            }
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                    return null;
                }

                _logger.LogInformation("Product '{ProductName}' retrieved successfully.", product.Name);
                return new ProductResponseDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Category = product.Category,
                    Price = product.Price,
                    StockLevel = product.StockLevel,
                    ImageBase64 = product.ImageData != null ? Convert.ToBase64String(product.ImageData) : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product ID {ProductId}.", productId);
                throw;
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                    throw new KeyNotFoundException("Product not found.");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                // Log product deletion
                _logger.LogInformation("Product '{ProductId}' deleted successfully.", productId);

                // Notify TechFix-backend to delete the product reference
                await NotifyTechFixBackendToDeleteProduct(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product ID {ProductId}.", productId);
                throw;
            }
        }

        private async Task NotifyTechFixBackendOnCreateAsync(Product product)
        {
            try
            {
                var productRef = new ProductRefDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    SupplierId = 1 // Hardcoded supplier ID 
                };

                var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/productref", productRef);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify TechFix-backend about product creation.");
                    throw new Exception("Failed to notify TechFix-backend about product creation.");
                }

                _logger.LogInformation("Notified TechFix-backend about the creation of product '{ProductName}'.", product.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying TechFix-backend.");
                throw;
            }
        }

        private async Task NotifyTechFixBackendOnUpdateAsync(Product product)
        {
            try
            {
                var productRefUpdate = new ProductRefUpdateDto 
                {
                    productName = product.Name
                };

                var response = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/productref/{product.ProductId}", productRefUpdate);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify TechFix-backend about product update.");
                    throw new Exception("Failed to notify TechFix-backend about product update.");
                }

                _logger.LogInformation("Notified TechFix-backend about the update of product '{ProductName}'.", product.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying TechFix-backend.");
                throw;
            }
        }

        private async Task NotifyTechFixBackendToDeleteProduct(int productId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:5000/api/productref/{productId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify TechFix-backend to delete the product reference.");
                    throw new Exception("Failed to notify TechFix-backend to delete the product reference.");
                }

                _logger.LogInformation("Notified TechFix-backend to delete product ID {ProductId}.", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying TechFix-backend.");
                throw;
            }
        }
    }
}
