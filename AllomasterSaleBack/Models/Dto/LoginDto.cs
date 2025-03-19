using System.ComponentModel.DataAnnotations;

namespace AlloMasterSale.Models
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}