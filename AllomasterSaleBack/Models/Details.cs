using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlloMasterSale.Data
{
    public class Detail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public string CompanyName { get; set; }
        public string Login { get; set; }
        public string Pswrd { get; set; }
    }
}