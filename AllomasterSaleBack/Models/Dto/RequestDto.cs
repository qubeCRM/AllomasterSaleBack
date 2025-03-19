namespace AlloMasterSale.Models
{
    public class RequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int SubscriptionId { get; set; }
        public int SubscriptionMonths { get; set; }
        public int PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}