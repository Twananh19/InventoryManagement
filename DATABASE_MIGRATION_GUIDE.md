# Database Migration Guide

## 📋 Overview
File này hướng dẫn cách cập nhật database với các tables mới cho các module được chia cho 3 thành viên.

## 🔧 Bước 1: Cập nhật AppDbContext.cs

Mở file `Services/AppDbContext.cs` và thêm các DbSet sau:

```csharp
// THÀNH VIÊN 1: User Management & Settings
public DbSet<User_T1> Users_T1 { get; set; }
public DbSet<Role_T1> Roles_T1 { get; set; }
public DbSet<AuditLog_T1> AuditLogs_T1 { get; set; }
public DbSet<SystemSettings_T1> SystemSettings_T1 { get; set; }

// THÀNH VIÊN 2: Suppliers & Customers
public DbSet<Supplier_T2> Suppliers_T2 { get; set; }
public DbSet<Customer_T2> Customers_T2 { get; set; }
public DbSet<StockAdjustment_T2> StockAdjustments_T2 { get; set; }
```

## 🔧 Bước 2: Cấu hình Relationships (trong OnModelCreating)

Thêm vào method `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // THÀNH VIÊN 1: Configure relationships
    modelBuilder.Entity<User_T1>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<AuditLog_T1>()
        .HasOne(a => a.User)
        .WithMany(u => u.AuditLogs)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    // THÀNH VIÊN 2: Configure relationships
    modelBuilder.Entity<Supplier_T2>()
        .HasMany(s => s.InboundLogs)
        .WithOne()
        .HasForeignKey("SupplierId")
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Customer_T2>()
        .HasMany(c => c.OutboundLogs)
        .WithOne()
        .HasForeignKey("CustomerId")
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<StockAdjustment_T2>()
        .HasOne(sa => sa.Product)
        .WithMany()
        .HasForeignKey(sa => sa.ProductId)
        .OnDelete(DeleteBehavior.Restrict);

    // Indexes for performance
    modelBuilder.Entity<AuditLog_T1>()
        .HasIndex(a => a.Timestamp);
    
    modelBuilder.Entity<AuditLog_T1>()
        .HasIndex(a => a.UserId);
}
```

## 🔧 Bước 3: Thêm Initial Data (Seed Data)

Thêm method mới trong AppDbContext để seed data ban đầu:

```csharp
public static async Task SeedInitialDataAsync(AppDbContext context)
{
    // Seed Roles (THÀNH VIÊN 1)
    if (!context.Roles_T1.Any())
    {
        context.Roles_T1.AddRange(
            new Role_T1
            {
                RoleName = "Admin",
                Description = "System Administrator with full access",
                CanManageUsers = true,
                CanManageProducts = true,
                CanManageInventory = true,
                CanViewReports = true,
                CanExportData = true,
                CanManageSettings = true
            },
            new Role_T1
            {
                RoleName = "Manager",
                Description = "Manager with limited admin access",
                CanManageUsers = false,
                CanManageProducts = true,
                CanManageInventory = true,
                CanViewReports = true,
                CanExportData = true,
                CanManageSettings = false
            },
            new Role_T1
            {
                RoleName = "Staff",
                Description = "Staff with basic access",
                CanManageUsers = false,
                CanManageProducts = false,
                CanManageInventory = true,
                CanViewReports = false,
                CanExportData = false,
                CanManageSettings = false
            }
        );
        await context.SaveChangesAsync();
    }

    // Seed default admin user (THÀNH VIÊN 1)
    if (!context.Users_T1.Any())
    {
        var adminRole = context.Roles_T1.First(r => r.RoleName == "Admin");
        context.Users_T1.Add(new User_T1
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FullName = "System Administrator",
            Email = "admin@goodmanagement.com",
            RoleId = adminRole.Id,
            IsActive = true,
            CreatedDate = DateTime.Now
        });
        await context.SaveChangesAsync();
    }

    // Seed System Settings (THÀNH VIÊN 1)
    if (!context.SystemSettings_T1.Any())
    {
        context.SystemSettings_T1.AddRange(
            new SystemSettings_T1 { SettingKey = "CompanyName", SettingValue = "Good Management Co., Ltd", Category = "Company" },
            new SystemSettings_T1 { SettingKey = "LowStockThreshold", SettingValue = "10", Category = "Inventory" },
            new SystemSettings_T1 { SettingKey = "MinPasswordLength", SettingValue = "6", Category = "Security" }
        );
        await context.SaveChangesAsync();
    }
}
```

## 🔧 Bước 4: Tạo Migration

Chạy lệnh sau trong Package Manager Console:

```powershell
Add-Migration AddNewModules_T1_T2_T3
```

## 🔧 Bước 5: Update Database

```powershell
Update-Database
```

## 🔧 Bước 6: Verify Migration

Kiểm tra database đã tạo đủ tables:

**Tables mới (THÀNH VIÊN 1):**
- User_T1
- Role_T1
- AuditLog_T1
- SystemSettings_T1

**Tables mới (THÀNH VIÊN 2):**
- Supplier_T2
- Customer_T2
- StockAdjustment_T2

## ⚠️ Troubleshooting

### Lỗi: Foreign Key Constraint

Nếu gặp lỗi foreign key, hãy:
1. Xóa database cũ: `Drop-Database` (trong Package Manager Console)
2. Tạo lại migration: `Add-Migration InitialCreate`
3. Update database: `Update-Database`

### Lỗi: Table Already Exists

```powershell
# Remove last migration
Remove-Migration

# Create new migration with different name
Add-Migration AddNewModules_V2
Update-Database
```

## 📝 Testing

Sau khi migration thành công, test bằng cách:

1. Chạy ứng dụng
2. Login với user mặc định:
   - Username: `admin`
   - Password: `admin123`
3. Kiểm tra các module mới hoạt động

## 🎯 Next Steps

1. **Thành viên 1**: Implement UI cho User Management, Settings
2. **Thành viên 2**: Implement UI cho Suppliers, Customers, Stock Adjustment
3. **Thành viên 3**: Implement Excel/PDF Export, Charts, Audit Logs View

## 📚 References

- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Fluent API](https://docs.microsoft.com/en-us/ef/core/modeling/)
- [Data Seeding](https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding)
