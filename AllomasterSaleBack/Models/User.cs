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

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Используем UTC вместо Local


        // Связь с Details (один пользователь -> много деталей)
        public List<Detail> Details { get; set; }
    }
}