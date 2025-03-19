using Microsoft.EntityFrameworkCore;

namespace AlloMasterSale.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Request> Requests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальность логина
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            // Уникальность названия компании
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Уникальность названия продукта
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Связь User -> Company
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);

            // Связь Subscription -> Company
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CompanyId);

            // Связь Payment -> Subscription
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.SubscriptionId);

            // Связь Request -> User
            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.UserId);

            // Связь Request -> Company
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Company)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CompanyId);

            // Связь Request -> Subscription
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Subscription)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.SubscriptionId);

            // Связь Request -> Payment
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Payment)
                .WithMany(p => p.Requests)
                .HasForeignKey(r => r.PaymentId);
        }
    }
}