using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrickyTrayAPI.Models
{
    public class OrderGift
    {
        [Key]
        public int Id { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public Gift Gift { get; set; }

        [ForeignKey("Gift")]
        public int GiftId { get; set; }

        public bool IsWinner { get; set; } = false;
    }
}
