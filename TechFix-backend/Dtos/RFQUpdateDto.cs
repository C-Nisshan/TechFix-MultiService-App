using TechFix_backend.Enums;

namespace TechFix_backend.Dtos
{
    public class RFQUpdateDto
    {
        public RFQStatus? Status { get; set; }
        public int? SupplierId { get; set; }
        public List<RFQItemUpdateDto>? RFQItems { get; set; }
    }
}

