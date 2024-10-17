namespace Supplier_backend.Dtos
{
    public class RFQItemResponseDto
    {
        public int RFQItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int RequestedQuantity { get; set; }
    }
}
