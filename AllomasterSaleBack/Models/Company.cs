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

        // Связь с пользователями и подписками
        public List<User> Users { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Request> Requests { get; set; }
    }
}