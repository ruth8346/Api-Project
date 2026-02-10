namespace TrickyTrayAPI.DTOs
{
    public class OrderViewDto
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<CartItemDto> Items { get; set; } = new();
        public int TotalPrice { get; set; }
    }
}
