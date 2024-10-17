namespace TechFix_backend.Dtos
{
    public class RFQCreateDto
    {
        public string CreatedBy { get; set; }
        public int SupplierId { get; set; }
        public List<RFQItemCreateDto> RFQItems { get; set; }
    }
}
