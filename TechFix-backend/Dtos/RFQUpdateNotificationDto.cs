using TechFix_backend.Enums;

namespace TechFix_backend.Dtos
{
    public class RFQUpdateNotificationDto
    {
        public int RFQId { get; set; }
        public int SupplierId { get; set; }  // Include SupplierId
        public RFQStatus Status { get; set; }  // Include Status
        public string SpecialConditions { get; set; } = string.Empty;
    }
}
