using System.ComponentModel.DataAnnotations;

namespace AlloMasterSale.Data
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Связь с User (одна компания -> много пользователей)
        public List<User> Users { get; set; }

        // Связь с Subscriptions (одна компания -> много подписок)
        public List<Subscription> Subscriptions { get; set; }
    }
}