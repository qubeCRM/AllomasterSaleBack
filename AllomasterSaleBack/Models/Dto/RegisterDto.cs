using System.ComponentModel.DataAnnotations;

namespace AlloMasterSale.Models
{
    public class RegisterDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
        public string Password { get; set; }

        public int? CompanyId { get; set; } // Можно указать ID существующей компании

        public string? CompanyName { get; set; } // Или создать новую компанию
    }
}