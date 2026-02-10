using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrickyTrayAPI.Models
{
    public class Order
    {
        public enum OrderStatus
        {
            Draft,
            Confirmed
        }
        [Key]
        public int Id { get; set; }
        public Buyer? Buyer { get; set; }

        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }= OrderStatus.Draft;
        public ICollection<OrderGift> OrderGifts { get; set; } = new List<OrderGift>();


    }
}
