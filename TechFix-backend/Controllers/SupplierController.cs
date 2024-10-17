using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechFix_backend.Services;
using TechFix_backend.Models;
using TechFix_backend.Dtos;

namespace TechFix_backend.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetSuppliersAsync();

            if (suppliers == null || !suppliers.Any())
            {
                return NotFound("No suppliers found.");
            }

            return Ok(suppliers);
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSupplier = await _supplierService.AddSupplierAsync(supplierDto);
            return Ok("Product reference created successfully.");
        }

        // PUT: api/Suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplierDto);

            if (updatedSupplier == null)
            {
                return NotFound($"Supplier with ID {id} not found.");
            }

            return Ok(updatedSupplier);
        }

        // DELETE: api/Suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);

            if (!result)
            {
                return NotFound($"Supplier with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
