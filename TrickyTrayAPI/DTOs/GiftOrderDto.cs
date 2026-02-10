namespace TrickyTrayAPI.DTOs
{
    public class CartItemDto
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
