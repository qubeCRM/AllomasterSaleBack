using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlloMasterSale.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User"; // Добавляем поле Role, по умолчанию "User"

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Связь с заявками
        public List<Request> Requests { get; set; }
    }
}