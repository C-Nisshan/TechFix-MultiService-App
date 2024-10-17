using TechFix_backend.Data;
using TechFix_backend.Models;
using TechFix_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TechFix_backend.Enums;

namespace TechFix_backend.Services
{
    public class RFQService
    {
        private readonly TechFixDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RFQService> _logger;

        public RFQService(TechFixDbContext context, HttpClient httpClient, ILogger<RFQService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // Add a new RFQ
        public async Task AddRFQAsync(RFQCreateDto rfqCreateDto)
        {
            try
            {
                var newRFQ = new RFQ
                {
                    CreatedBy = rfqCreateDto.CreatedBy,
                    CreationDate = DateTime.Now,
                    Status = RFQStatus.PENDING,
                    SupplierId = rfqCreateDto.SupplierId
                };

                // Add the RFQ items
                newRFQ.RFQItems = rfqCreateDto.RFQItems.Select(i => new RFQItem
                {
                    ProductId = i.ProductId,
                    RequestedQuantity = i.RequestedQuantity
                }).ToList();

                _context.RFQs.Add(newRFQ);
                await _context.SaveChangesAsync();

                // Log the RFQ creation
                _logger.LogInformation("RFQ ID {RFQId} created successfully.", newRFQ.RFQId);

                // Notify Supplier-backend about the new RFQ
                await NotifySupplierBackendOnCreateAsync(newRFQ, rfqCreateDto.RFQItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the RFQ.");
                throw;
            }
        }

        // Update an existing RFQ
        public async Task UpdateRFQAsync(int rfqId, RFQUpdateDto rfqUpdateDto)
        {
            try
            {
                var existingRFQ = await _context.RFQs.Include(r => r.RFQItems).FirstOrDefaultAsync(r => r.RFQId == rfqId);

                if (existingRFQ == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    throw new KeyNotFoundException("RFQ not found.");
                }

                // Update status, supplier, and any other updatable fields
                existingRFQ.Status = rfqUpdateDto.Status ?? existingRFQ.Status;
                existingRFQ.SupplierId = rfqUpdateDto.SupplierId;

                // Update RFQ items
                foreach (var updatedItem in rfqUpdateDto.RFQItems)
                {
                    var existingItem = existingRFQ.RFQItems.FirstOrDefault(i => i.RFQItemId == updatedItem.RFQItemId);
                    if (existingItem != null)
                    {
                        existingItem.RequestedQuantity = updatedItem.RequestedQuantity;
                    }
                }

                await _context.SaveChangesAsync();

                // Log the RFQ update
                _logger.LogInformation("RFQ ID {RFQId} updated successfully.", rfqId);

                // Notify Supplier-backend about the RFQ update
                await NotifySupplierBackendOnUpdateAsync(existingRFQ);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating RFQ ID {RFQId}.", rfqId);
                throw;
            }
        }

        // Retrieve all RFQs
        public async Task<List<RFQResponseDto>> GetAllRFQsAsync()
        {
            try
            {
                var rfqs = await _context.RFQs
                    .Include(r => r.Supplier) // Include Supplier details
                    .Include(r => r.RFQItems)
                        .ThenInclude(i => i.Product) // Use Product instead of ProductRefs
                    .Select(r => new RFQResponseDto
                    {
                        RFQId = r.RFQId,
                        CreatedBy = r.CreatedBy,
                        CreationDate = r.CreationDate,
                        Status = r.Status,
                        SupplierName = r.Supplier.Name, // Assuming the Supplier has a Name property
                        RFQItems = r.RFQItems.Select(i => new RFQItemResponseDto
                        {
                            RFQItemId = i.RFQItemId,
                            ProductId = i.ProductId,
                            ProductName = i.Product.ProductName, // Use Product instead of ProductRef
                            RequestedQuantity = i.RequestedQuantity
                        }).ToList()
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} RFQs.", rfqs.Count);
                return rfqs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving RFQs."); // Make sure this is correct
                throw;
            }
        }


        // Retrieve a specific RFQ by ID
        public async Task<RFQResponseDto?> GetRFQByIdAsync(int rfqId)
        {
            try
            {
                var rfq = await _context.RFQs
                    .Include(r => r.RFQItems)
                    .Where(r => r.RFQId == rfqId)
                    .Select(r => new RFQResponseDto
                    {
                        RFQId = r.RFQId,
                        CreatedBy = r.CreatedBy,
                        CreationDate = r.CreationDate,
                        Status = r.Status,
                        RFQItems = r.RFQItems.Select(i => new RFQItemResponseDto
                        {
                            RFQItemId = i.RFQItemId,
                            ProductId = i.ProductId,
                            RequestedQuantity = i.RequestedQuantity
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (rfq == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    return null;
                }

                _logger.LogInformation("RFQ ID {RFQId} retrieved successfully.", rfqId);
                return rfq;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving RFQ ID {RFQId}.", rfqId);
                throw;
            }
        }

        // Delete an RFQ
        public async Task DeleteRFQAsync(int rfqId)
        {
            try
            {
                var rfq = await _context.RFQs.FindAsync(rfqId);

                if (rfq == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    throw new KeyNotFoundException("RFQ not found.");
                }

                _context.RFQs.Remove(rfq);
                await _context.SaveChangesAsync();

                // Log the RFQ deletion
                _logger.LogInformation("RFQ ID {RFQId} deleted successfully.", rfqId);

                // Notify Supplier-backend about the RFQ deletion
                await NotifySupplierBackendToDeleteRFQ(rfqId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting RFQ ID {RFQId}.", rfqId);
                throw;
            }
        }

        // Private method to notify Supplier-backend about RFQ creation
        private async Task NotifySupplierBackendOnCreateAsync(RFQ rfq, List<RFQItemCreateDto> rfqItems)
        {
            try
            {
                var rfqNotification = new RFQNotificationDto
                {
                    RFQId = rfq.RFQId,
                    Status = rfq.Status,
                    CreationDate = rfq.CreationDate,
                    RFQItems = rfqItems.Select(i => new RFQItemNotificationDto
                    {
                        ProductId = i.ProductId,
                        RequestedQuantity = i.RequestedQuantity
                    }).ToList()
                };

                var response = await _httpClient.PostAsJsonAsync("http://localhost:5001/api/rfq", rfqNotification);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify Supplier-backend about RFQ creation.");
                    throw new Exception("Failed to notify Supplier-backend about RFQ creation.");
                }

                _logger.LogInformation("Notified Supplier-backend about RFQ ID {RFQId}.", rfq.RFQId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying Supplier-backend.");
                throw;
            }
        }

        // Private method to notify Supplier-backend about RFQ update
        private async Task NotifySupplierBackendOnUpdateAsync(RFQ rfq)
        {
            try
            {
                var rfqUpdateNotification = new RFQUpdateNotificationDto
                {
                    RFQId = rfq.RFQId,
                    SupplierId = rfq.SupplierId ?? 0,
                    Status = rfq.Status
                };

                var response = await _httpClient.PutAsJsonAsync($"http://localhost:5001/api/rfq/{rfq.RFQId}", rfqUpdateNotification);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify Supplier-backend about RFQ update.");
                    throw new Exception("Failed to notify Supplier-backend about RFQ update.");
                }

                _logger.LogInformation("Notified Supplier-backend about RFQ ID {RFQId} update.", rfq.RFQId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying Supplier-backend.");
                throw;
            }
        }

        // Private method to notify Supplier-backend about RFQ deletion
        private async Task NotifySupplierBackendToDeleteRFQ(int rfqId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:5001/api/rfq/status/{rfqId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to notify Supplier-backend about RFQ deletion.");
                    throw new Exception("Failed to notify Supplier-backend about RFQ deletion.");
                }

                _logger.LogInformation("Notified Supplier-backend about RFQ ID {RFQId} deletion.", rfqId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while notifying Supplier-backend.");
                throw;
            }
        }

        public async Task UpdateRFQStatusAsync(int rfqId, string status)
        {
            try
            {
                var rfq = await _context.RFQs.FindAsync(rfqId);
                if (rfq == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    throw new KeyNotFoundException("RFQ not found.");
                }

                // Convert string status to enum RFQStatus (if applicable)
                if (Enum.TryParse<RFQStatus>(status, true, out var rfqStatus))
                {
                    rfq.Status = rfqStatus; // Update status with enum
                }
                else
                {
                    throw new ArgumentException($"Invalid status: {status}");
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated RFQ status for RFQ ID {RFQId} to {Status}", rfqId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating RFQ status.");
                throw;
            }
        }
    }
}
