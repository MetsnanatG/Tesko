using Microsoft.EntityFrameworkCore;
using Tesko.Models;

namespace Tesko.Data
{
    public class TeskoDbContext : DbContext
    {
        public TeskoDbContext(DbContextOptions<TeskoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin User", Email = "admin@tesko.com", Role = "Admin", PasswordHash = "admin123" },
                new User { Id = 2, Name = "Test Lead", Email = "lead@tesko.com", Role = "Approver", PasswordHash = "lead123" },
                new User { Id = 3, Name = "Tester One", Email = "tester1@tesko.com", Role = "Requester", PasswordHash = "tester123" }
            );
            
            modelBuilder.Entity<Asset>().HasData(
                new Asset { Id = 1, Name = "5G SIM Card", Type = "SIM", TotalStock = 100, AvailableStock = 100, LowStockThreshold = 10 },
                new Asset { Id = 2, Name = "iPhone 13", Type = "Device", TotalStock = 5, AvailableStock = 5, LowStockThreshold = 1 },
                new Asset { Id = 3, Name = "$10 Voucher", Type = "Voucher", TotalStock = 50, AvailableStock = 50, LowStockThreshold = 5 }
            );
        }
    }
}
