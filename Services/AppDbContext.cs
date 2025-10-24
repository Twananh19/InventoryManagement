using Microsoft.EntityFrameworkCore;
using GoodManagement.Models;

namespace GoodManagement.Services
{
    /// <summary>
    /// DbContext để quản lý kết nối và thao tác với database
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InboundLog> InboundLogs { get; set; }
        public DbSet<OutboundLog> OutboundLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Sử dụng SQLite cho đơn giản, file database sẽ nằm trong thư mục bin
            optionsBuilder.UseSqlite("Data Source=goodmanagement.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thiết lập quan hệ giữa các bảng
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InboundLog>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OutboundLog>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed dữ liệu admin mặc định
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                }
            );

            // Seed một số sản phẩm mẫu
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Gạo ST25",
                    Packaging = "Bao 50kg",
                    UnitOfMeasurement = "kg",
                    Price = 25000
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Đường trắng",
                    Packaging = "Bao 25kg",
                    UnitOfMeasurement = "kg",
                    Price = 18000
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Dầu ăn Neptune",
                    Packaging = "Chai 1L",
                    UnitOfMeasurement = "lít",
                    Price = 45000
                }
            );
        }
    }
}
