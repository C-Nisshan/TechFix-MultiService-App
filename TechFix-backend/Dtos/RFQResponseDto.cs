using TechFix_backend.Enums;

namespace TechFix_backend.Dtos
{
    public class RFQResponseDto
    {
        public int RFQId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public RFQStatus Status { get; set; }
        public string SupplierName { get; set; } 
        public List<RFQItemResponseDto> RFQItems { get; set; }
    }

}
