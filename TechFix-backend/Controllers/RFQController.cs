using Microsoft.AspNetCore.Mvc;
using TechFix_backend.Dtos;
using TechFix_backend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TechFix_backend.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RFQController : ControllerBase
    {
        private readonly RFQService _rfqService;
        private readonly ILogger<RFQController> _logger;

        public RFQController(RFQService rfqService, ILogger<RFQController> logger)
        {
            _rfqService = rfqService;
            _logger = logger;
        }

        // POST: api/RFQ
        [HttpPost]
        public async Task<IActionResult> AddRFQ([FromBody] RFQCreateDto rfqCreateDto)
        {
            try
            {
                await _rfqService.AddRFQAsync(rfqCreateDto);
                return Ok("RFQ created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating RFQ.");
                return StatusCode(500, "An error occurred while creating the RFQ.");
            }
        }

        // PUT: api/RFQ/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRFQ(int id, [FromBody] RFQUpdateDto rfqUpdateDto)
        {
            try
            {
                await _rfqService.UpdateRFQAsync(id, rfqUpdateDto);
                return Ok("RFQ updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"RFQ with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating RFQ ID {id}.");
                return StatusCode(500, "An error occurred while updating the RFQ.");
            }
        }

        // GET: api/RFQ
        [HttpGet]
        public async Task<IActionResult> GetAllRFQs()
        {
            try
            {
                var rfqs = await _rfqService.GetAllRFQsAsync();
                return Ok(rfqs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving RFQs.");
                return StatusCode(500, "An error occurred while retrieving RFQs.");
            }
        }

        // GET: api/RFQ/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRFQById(int id)
        {
            try
            {
                var rfq = await _rfqService.GetRFQByIdAsync(id);
                if (rfq == null)
                {
                    return NotFound($"RFQ with ID {id} not found.");
                }
                return Ok(rfq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving RFQ ID {id}.");
                return StatusCode(500, "An error occurred while retrieving the RFQ.");
            }
        }

        // DELETE: api/RFQ/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRFQ(int id)
        {
            try
            {
                await _rfqService.DeleteRFQAsync(id);
                return Ok("RFQ deleted successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"RFQ with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting RFQ ID {id}.");
                return StatusCode(500, "An error occurred while deleting the RFQ.");
            }
        }

        // Update RFQ status by RFQId
        [HttpPut("/status/{rfqId}")]
        public async Task<IActionResult> UpdateRFQStatus(int rfqId, [FromBody] UpdateRFQStatusDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Status))
            {
                return BadRequest("Invalid request payload.");
            }

            try
            {
                await _rfqService.UpdateRFQStatusAsync(rfqId, request.Status);
                return Ok(new { message = "RFQ status updated successfully in TechFix." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log exception, return generic error message
                return StatusCode(500, new { message = "An error occurred while updating the RFQ status in TechFix." });
            }
        }
    }
}
