using Supplier_backend.Data;
using Supplier_backend.Models;
using Supplier_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Supplier_backend.Enums;

namespace Supplier_backend.Services
{
    public class RFQService
    {
        private readonly SupplierDbContext _context;
        private readonly ILogger<RFQService> _logger;
        private readonly HttpClient _httpClient;
        public RFQService(SupplierDbContext context, HttpClient httpClient, ILogger<RFQService> logger)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
        }

        // Create RFQ (from TechFix)
        public async Task CreateRFQFromTechFixAsync(RFQNotificationDto rfqNotificationDto)
        {
            try
            {
                // Log the received data
                _logger.LogInformation("Received RFQ from TechFix: {@RFQNotificationDto}", rfqNotificationDto);

                var newRFQ = new RFQ
                {
                    RFQId = rfqNotificationDto.RFQId,
                    CreationDate = rfqNotificationDto.CreationDate,
                    Status = rfqNotificationDto.Status
                };

                newRFQ.RFQItems = rfqNotificationDto.RFQItems.Select(i => new RFQItem
                {
                    ProductId = i.ProductId,
                    RequestedQuantity = i.RequestedQuantity
                }).ToList();

                _context.RFQs.Add(newRFQ);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RFQ from TechFix created with ID {RFQId}", newRFQ.RFQId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating RFQ from TechFix.");
                throw;
            }
        }

        // Update RFQ (from TechFix)
        public async Task UpdateRFQFromTechFixAsync(int rfqId, RFQUpdateNotificationDto rfqUpdateDto)
        {
            try
            {
                // Log the RFQ ID and the update data
                _logger.LogInformation("Received update for RFQ ID {RFQId} from TechFix: {@RFQUpdateDto}", rfqId, rfqUpdateDto);

                var existingRFQ = await _context.RFQs.Include(r => r.RFQItems).FirstOrDefaultAsync(r => r.RFQId == rfqId);

                if (existingRFQ == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    throw new KeyNotFoundException("RFQ not found.");
                }

                existingRFQ.Status = rfqUpdateDto.Status ?? existingRFQ.Status;

                await _context.SaveChangesAsync();

                _logger.LogInformation("RFQ ID {RFQId} updated successfully from TechFix.", rfqId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating RFQ ID {RFQId} from TechFix.", rfqId);
                throw;
            }
        }

        // Get all RFQs with Supplier and Product details
        public async Task<List<RFQResponseDto>> GetAllRFQsAsync()
        {
            try
            {
                var rfqs = await _context.RFQs
                    .Include(r => r.RFQItems)
                        .ThenInclude(i => i.Product) // Include Product details
                    .Select(r => new RFQResponseDto
                    {
                        RFQId = r.RFQId,
                        CreationDate = r.CreationDate,
                        Status = r.Status.ToString(),
                        RFQItems = r.RFQItems.Select(i => new RFQItemResponseDto
                        {
                            RFQItemId = i.RFQItemId,
                            ProductId = i.ProductId,
                            ProductName = i.Product.Name, 
                            RequestedQuantity = i.RequestedQuantity
                        }).ToList()
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} RFQs.", rfqs.Count);
                return rfqs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving RFQs.");
                throw;
            }
        }


        // Get RFQ by ID
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
                        CreationDate = r.CreationDate,
                        Status = r.Status.ToString(),
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

        // Delete RFQ
        public async Task DeleteRFQAsync(int rfqId)
        {
            try
            {
                // Log the RFQ ID to be deleted
                _logger.LogInformation("Received request to delete RFQ ID {RFQId} from TechFix", rfqId);

                var rfq = await _context.RFQs.FindAsync(rfqId);

                if (rfq == null)
                {
                    _logger.LogWarning("RFQ with ID {RFQId} not found.", rfqId);
                    throw new KeyNotFoundException("RFQ not found.");
                }

                _context.RFQs.Remove(rfq);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RFQ ID {RFQId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting RFQ ID {RFQId}.", rfqId);
                throw;
            }
        }


        public async Task RespondToRFQAsync(int rfqId, string status)
        {
            try
            {
                var rfq = await _context.RFQs.FindAsync(rfqId);
                if (rfq == null)
                {
                    throw new KeyNotFoundException("RFQ not found.");
                }

                // Convert string status to enum RFQStatus
                if (Enum.TryParse<RFQStatus>(status, true, out var rfqStatus))
                {
                    rfq.Status = rfqStatus; // Assign the enum value
                }
                else
                {
                    throw new ArgumentException($"Invalid status: {status}");
                }

                await _context.SaveChangesAsync();

                // Sync status with TechFix
                await SyncRFQStatusWithTechFix(rfqId, rfqStatus.ToString()); // Convert enum to string
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while responding to RFQ.");
                throw;
            }
        }

        private async Task SyncRFQStatusWithTechFix(int rfqId, string status)
        {
            var techFixResponse = new { RFQId = rfqId, Status = status };
            try
            {
                var response = await _httpClient.PutAsJsonAsync("http://localhost:5000/api/rfq/status/{rfqId}", techFixResponse);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to update status in TechFix.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while syncing RFQ status with TechFix.");
            }
        }
    }
}
