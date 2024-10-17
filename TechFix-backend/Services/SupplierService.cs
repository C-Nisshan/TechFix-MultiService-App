using TechFix_backend.Dtos;
using TechFix_backend.Models;
using TechFix_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace TechFix_backend.Services
{
    public class SupplierService
    {
        private readonly TechFixDbContext _context;

        public SupplierService(TechFixDbContext context)
        {
            _context = context;
        }

        // Service method to get all suppliers
        public async Task<List<SupplierDto>> GetSuppliersAsync()
        {
            var suppliers = await _context.Suppliers
                                          .Select(s => new SupplierDto
                                          {
                                              Id = s.SupplierId,
                                              Name = s.Name,
                                              ContactInfo = s.ContactInfo,
                                          })
                                          .ToListAsync();

            return suppliers;
        }

        // Service method to add a new supplier
        public async Task<SupplierDto> AddSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = new Supplier
            {
                Name = supplierDto.Name,
                ContactInfo = supplierDto.ContactInfo
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return new SupplierDto
            {
                Id = supplier.SupplierId,
                Name = supplier.Name,
                ContactInfo = supplier.ContactInfo
            };
        }

        // Service method to update an existing supplier
        public async Task<SupplierDto?> UpdateSupplierAsync(int id, SupplierDto supplierDto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return null;
            }

            supplier.Name = supplierDto.Name;
            supplier.ContactInfo = supplierDto.ContactInfo;

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();

            return new SupplierDto
            {
                Id = supplier.SupplierId,
                Name = supplier.Name,
                ContactInfo = supplier.ContactInfo
            };
        }

        // Service method to delete a supplier
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
