namespace AlloMasterSale.Models
{
    public class RegisterDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}