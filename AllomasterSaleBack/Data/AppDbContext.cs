using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace AlloMasterSale.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Detail> Details { get; set; }

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

            // Связь Detail -> User
            modelBuilder.Entity<Detail>()
                .HasOne(d => d.User)
                .WithMany(u => u.Details)
                .HasForeignKey(d => d.UserId);
        }
    }
}