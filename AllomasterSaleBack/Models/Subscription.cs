using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlloMasterSale.Data
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Required]
        public int Months { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Связь с Payments (одна подписка -> много платежей)
        public List<Payment> Payments { get; set; }
    }
}