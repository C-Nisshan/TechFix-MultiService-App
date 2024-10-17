using Microsoft.AspNetCore.Mvc;
using Supplier_backend.Dtos;
using Supplier_backend.Services;

namespace Supplier_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFQController : ControllerBase
    {
        private readonly RFQService _rfqService;

        public RFQController(RFQService rfqService)
        {
            _rfqService = rfqService;
        }

        // POST: api/rfq
        [HttpPost]
        public async Task<IActionResult> CreateRFQFromTechFix([FromBody] RFQNotificationDto rfqNotificationDto)
        {
            if (rfqNotificationDto == null)
            {
                return BadRequest("RFQ data is required.");
            }

            await _rfqService.CreateRFQFromTechFixAsync(rfqNotificationDto);
            return CreatedAtAction(nameof(GetRFQById), new { id = rfqNotificationDto.RFQId }, rfqNotificationDto);
        }

        // PUT: api/rfq/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRFQFromTechFix(int id, [FromBody] RFQUpdateNotificationDto rfqUpdateDto)
        {
            if (rfqUpdateDto == null)
            {
                return BadRequest("RFQ update data is required.");
            }

            try
            {
                await _rfqService.UpdateRFQFromTechFixAsync(id, rfqUpdateDto);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
        }

        // GET: api/rfq
        [HttpGet]
        public async Task<IActionResult> GetAllRFQs()
        {
            var rfqs = await _rfqService.GetAllRFQsAsync();
            return Ok(rfqs); // 200 OK
        }

        // GET: api/rfq/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRFQById(int id)
        {
            var rfq = await _rfqService.GetRFQByIdAsync(id);

            if (rfq == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(rfq); // 200 OK
        }

        // DELETE: api/rfq/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRFQ(int id)
        {
            try
            {
                await _rfqService.DeleteRFQAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
        }


        // update RFQ Status
        [HttpPut("{rfqId}/respond")]
        public async Task<IActionResult> RespondToRFQ(int rfqId, [FromBody] RespondToRFQRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Status))
            {
                return BadRequest("Invalid request payload.");
            }

            try
            {
                await _rfqService.RespondToRFQAsync(rfqId, request.Status);
                return Ok(new { message = "RFQ status updated successfully." });
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
                return StatusCode(500, new { message = "An error occurred while updating the RFQ status." });
            }
        }
    }
}
