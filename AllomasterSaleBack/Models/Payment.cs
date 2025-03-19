using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlloMasterSale.Data
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentGatewayId { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}