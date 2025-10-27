# 📋 PHÂN CHIA CÔNG VIỆC CHI TIẾT THEO THÀNH VIÊN

> **Dự án:** Good Management System  
> **Nhóm:** 3 thành viên  
> **Timeline:** 6 tuần  
> **Mục tiêu:** Điểm 9.5-10/10

---

## 🎯 QUY TẮC PHÂN CHIA

- **Thành viên 1:** Files có suffix `_T1` (User Management & Settings)
- **Thành viên 2:** Files có suffix `_T2` (Suppliers, Customers & Stock)
- **Thành viên 3:** Files có suffix `_T3` (Reports & Export)

---

# 👤 THÀNH VIÊN 1: User Management & System Settings

## 📂 FILES/FOLDERS ĐẢM NHẬN

### ✅ Files đã tạo sẵn (8 files)

#### Models (4 files):
```
Models/
├── User_T1.cs                    ✅ Created
├── Role_T1.cs                    ✅ Created
├── AuditLog_T1.cs                ✅ Created
└── SystemSettings_T1.cs          ✅ Created
```

#### ViewModels (2 files):
```
ViewModels/
├── UsersViewModel_T1.cs          ✅ Created (with TODO comments)
└── SettingsViewModel_T1.cs       ✅ Created (with TODO comments)
```

#### Views (1 file template):
```
Views/
├── UsersView_T1.xaml             ✅ Template created
└── UsersView_T1.xaml.cs          ✅ Created
```

#### Services (1 file):
```
Services/
└── AuditService_T1.cs            ✅ Created (with usage examples)
```

### 🔄 Files cần cập nhật/modify

```
Views/
├── LoginWindow.xaml              🔄 Update to use User_T1
└── LoginWindow.xaml.cs

ViewModels/
└── LoginViewModel.cs             🔄 Update authentication logic

Services/
└── AppDbContext.cs               🔄 Add DbSet for T1 models

MainWindow.xaml                   🔄 Add menu items + role-based hiding
```

---

## 🎯 CÁC BƯỚC CẦN LÀM (TUẦN 1-4)

### 📅 TUẦN 1: Database & Core Setup

#### Bước 1: Cập nhật AppDbContext (30 phút)
**File:** `Services/AppDbContext.cs`

**Thêm vào class AppDbContext:**
```csharp
// THÀNH VIÊN 1: User Management Models
public DbSet<User_T1> Users_T1 { get; set; }
public DbSet<Role_T1> Roles_T1 { get; set; }
public DbSet<AuditLog_T1> AuditLogs_T1 { get; set; }
public DbSet<SystemSettings_T1> SystemSettings_T1 { get; set; }
```

**Thêm vào OnModelCreating:**
```csharp
// Configure relationships for T1 models
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

// Indexes for performance
modelBuilder.Entity<AuditLog_T1>()
    .HasIndex(a => a.Timestamp);
modelBuilder.Entity<AuditLog_T1>()
    .HasIndex(a => a.UserId);
```

---

#### Bước 2: Chạy Migration (10 phút)
**Terminal (Package Manager Console):**
```powershell
Add-Migration AddUserManagementTables_T1
Update-Database
```

**Verify:** Check database có 4 tables mới:
- User_T1
- Role_T1
- AuditLog_T1
- SystemSettings_T1

---

#### Bước 3: Seed Initial Data (1 giờ)
**File:** `Services/AppDbContext.cs` hoặc tạo `Services/DataSeeder_T1.cs`

**Tạo method:**
```csharp
public static async Task SeedDataAsync(AppDbContext context)
{
    // Seed Roles
    if (!await context.Roles_T1.AnyAsync())
    {
        var roles = new List<Role_T1>
        {
            new Role_T1
            {
                RoleName = "Admin",
                Description = "Full system access",
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
                Description = "Manager level access",
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
                Description = "Basic staff access",
                CanManageUsers = false,
                CanManageProducts = false,
                CanManageInventory = true,
                CanViewReports = false,
                CanExportData = false,
                CanManageSettings = false
            }
        };
        context.Roles_T1.AddRange(roles);
        await context.SaveChangesAsync();
    }

    // Seed Default Admin User
    if (!await context.Users_T1.AnyAsync())
    {
        var adminRole = await context.Roles_T1.FirstAsync(r => r.RoleName == "Admin");
        var adminUser = new User_T1
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FullName = "System Administrator",
            Email = "admin@goodmanagement.com",
            PhoneNumber = "0123456789",
            RoleId = adminRole.Id,
            IsActive = true,
            CreatedDate = DateTime.Now
        };
        context.Users_T1.Add(adminUser);
        await context.SaveChangesAsync();
    }

    // Seed System Settings
    if (!await context.SystemSettings_T1.AnyAsync())
    {
        var settings = new List<SystemSettings_T1>
        {
            new SystemSettings_T1 { SettingKey = "CompanyName", SettingValue = "Good Management Co., Ltd", Category = "Company" },
            new SystemSettings_T1 { SettingKey = "CompanyAddress", SettingValue = "123 Main St, Hanoi", Category = "Company" },
            new SystemSettings_T1 { SettingKey = "CompanyPhone", SettingValue = "024-1234-5678", Category = "Company" },
            new SystemSettings_T1 { SettingKey = "LowStockThreshold", SettingValue = "10", Category = "Inventory" },
            new SystemSettings_T1 { SettingKey = "MinPasswordLength", SettingValue = "6", Category = "Security" },
            new SystemSettings_T1 { SettingKey = "SessionTimeout", SettingValue = "30", Category = "Security" }
        };
        context.SystemSettings_T1.AddRange(settings);
        await context.SaveChangesAsync();
    }
}
```

**Call trong App.xaml.cs hoặc MainWindow constructor:**
```csharp
using (var context = new AppDbContext())
{
    await DataSeeder_T1.SeedDataAsync(context);
}
```

---

### 📅 TUẦN 2: User Management Module

#### Bước 4: Hoàn thiện UsersView UI (3-4 giờ)
**File:** `Views/UsersView_T1.xaml`

**Cần thêm:**
1. **Add User Dialog:**
   - TextBox: Username (required)
   - PasswordBox: Password (required, min 6 chars)
   - TextBox: Full Name (required)
   - TextBox: Email (optional, email validation)
   - TextBox: Phone (optional)
   - ComboBox: Role (bind to Roles list)
   - Buttons: Save, Cancel

2. **Edit User Dialog:**
   - Same fields as Add
   - Pre-populate with selected user data
   - Disable username field (cannot change)

3. **Change Password Dialog:**
   - PasswordBox: Current Password
   - PasswordBox: New Password
   - PasswordBox: Confirm Password
   - Password strength indicator

4. **Confirmation Dialogs:**
   - Delete confirmation
   - Deactivate user confirmation

**Tip:** Copy template từ ProductsView.xaml và customize

---

#### Bước 5: Implement User CRUD Logic (4-5 giờ)
**File:** `ViewModels/UsersViewModel_T1.cs`

**TODO items đã có trong file, cần implement:**

1. **AddUser():**
```csharp
private void AddUser()
{
    var dialog = new AddUserDialog(); // Create dialog
    if (dialog.ShowDialog() == true)
    {
        var newUser = new User_T1
        {
            Username = dialog.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dialog.Password),
            FullName = dialog.FullName,
            Email = dialog.Email,
            PhoneNumber = dialog.PhoneNumber,
            RoleId = dialog.SelectedRoleId,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = CurrentUser.Username // Get from session
        };
        
        _context.Set<User_T1>().Add(newUser);
        await _context.SaveChangesAsync();
        
        // Log audit
        await AuditService_T1.LogCreateAsync(
            CurrentUser.Id, 
            "User_T1", 
            newUser.Id, 
            newUser
        );
        
        await LoadUsersAsync();
        MessageBox.Show("User created successfully!");
    }
}
```

2. **EditUser():** Similar logic with update
3. **DeleteUser():** Check dependencies first
4. **ChangePassword():** Verify current password, hash new password

---

#### Bước 6: Update Login Window (2-3 giờ)
**File:** `ViewModels/LoginViewModel.cs`

**Thay đổi authentication logic:**
```csharp
private async Task<bool> AuthenticateAsync(string username, string password)
{
    using var context = new AppDbContext();
    
    var user = await context.Users_T1
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
    
    if (user == null)
        return false;
    
    // Verify password
    if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        return false;
    
    // Update last login
    user.LastLoginDate = DateTime.Now;
    await context.SaveChangesAsync();
    
    // Log audit
    await AuditService_T1.LogLoginAsync(user.Id, true);
    
    // Store current user in session
    Application.Current.Properties["CurrentUser"] = user;
    
    return true;
}
```

---

### 📅 TUẦN 3: Settings Module

#### Bước 7: Tạo SettingsView UI (3-4 giờ)
**File:** `Views/SettingsView_T1.xaml` (NEW)

**Layout:**
```xml
<UserControl>
    <TabControl>
        <TabItem Header="Company Info">
            <!-- Company name, address, phone, email -->
        </TabItem>
        <TabItem Header="Inventory Settings">
            <!-- Low stock threshold, reorder point -->
        </TabItem>
        <TabItem Header="Security">
            <!-- Password policy, session timeout -->
        </TabItem>
        <TabItem Header="Backup">
            <!-- Backup/Restore buttons -->
        </TabItem>
    </TabControl>
</UserControl>
```

Copy style từ UsersView_T1.xaml

---

#### Bước 8: Implement Settings Logic (2-3 giờ)
**File:** `ViewModels/SettingsViewModel_T1.cs`

**Logic đã có template, cần:**
1. Test load/save settings
2. Implement backup database:
```csharp
private void BackupDatabase()
{
    var saveDialog = new SaveFileDialog
    {
        Filter = "SQLite Database|*.db",
        DefaultExt = "db",
        FileName = $"GoodManagement_Backup_{DateTime.Now:yyyyMMdd}.db"
    };
    
    if (saveDialog.ShowDialog() == true)
    {
        File.Copy("GoodManagement.db", saveDialog.FileName, true);
        MessageBox.Show("Backup successful!");
    }
}
```

---

### 📅 TUẦN 4: Integration & Testing

#### Bước 9: Integrate Audit Logs (3-4 giờ)
**Cần thêm audit vào các ViewModels:**

1. **ProductsViewModel.cs:**
```csharp
// After add product
await AuditService_T1.LogCreateAsync(currentUserId, "Products", product.Id, product);

// After edit product
await AuditService_T1.LogUpdateAsync(currentUserId, "Products", product.Id, oldProduct, newProduct);

// After delete product
await AuditService_T1.LogDeleteAsync(currentUserId, "Products", product.Id, product);
```

2. Tương tự cho: InventoryViewModel, InboundViewModel, OutboundViewModel

---

#### Bước 10: Update MainWindow Menu (1-2 giờ)
**File:** `MainWindow.xaml`

**Thêm menu items:**
```xml
<MenuItem Header="Admin" Visibility="{Binding IsAdminVisible}">
    <MenuItem Header="User Management" 
              Command="{Binding NavigateCommand}"
              CommandParameter="UsersView_T1"/>
    <MenuItem Header="System Settings" 
              Command="{Binding NavigateCommand}"
              CommandParameter="SettingsView_T1"/>
</MenuItem>
```

**File:** `ViewModels/MainViewModel.cs`

**Thêm role-based visibility:**
```csharp
public Visibility IsAdminVisible => 
    CurrentUser?.Role?.CanManageUsers == true 
        ? Visibility.Visible 
        : Visibility.Collapsed;
```

---

#### Bước 11: Testing (2-3 giờ)
- [ ] Test login với admin user (admin/admin123)
- [ ] Test create/edit/delete users
- [ ] Test role permissions
- [ ] Test change password
- [ ] Test settings save/load
- [ ] Test audit logs được tạo
- [ ] Test backup/restore

---

## ✅ CHECKLIST HOÀN THÀNH

### Database:
- [ ] Migration chạy thành công
- [ ] 4 tables được tạo (User_T1, Role_T1, AuditLog_T1, SystemSettings_T1)
- [ ] Seed data thành công (3 roles, 1 admin user)

### User Management:
- [ ] UsersView_T1.xaml hoàn chỉnh
- [ ] Add User dialog works
- [ ] Edit User dialog works
- [ ] Delete User works
- [ ] Change Password works
- [ ] Search & filter works
- [ ] Audit logs được tạo cho mọi thao tác

### Settings:
- [ ] SettingsView_T1.xaml được tạo
- [ ] Company settings load/save works
- [ ] Security settings works
- [ ] Backup database works
- [ ] Restore database works

### Integration:
- [ ] Login uses User_T1 table
- [ ] Current user stored in session
- [ ] Role-based menu visibility works
- [ ] Audit logs integrated vào Products
- [ ] Audit logs integrated vào Inventory
- [ ] Audit logs integrated vào Inbound/Outbound

---

## 📚 TÀI LIỆU THAM KHẢO

- **DATABASE_MIGRATION_GUIDE.md** - Chi tiết migration
- **PACKAGES_INSTALLATION_GUIDE.md** - BCrypt usage
- Code có TODO comments chi tiết
- Template UsersView_T1.xaml để tham khảo

---

## 🆘 KHI GẶP VẤN ĐỀ

1. **Migration lỗi:** Xóa Migrations folder, recreate từ đầu
2. **Password hash lỗi:** Check BCrypt.Net-Next installed
3. **Audit logs không tạo:** Check AuditService_T1 được call
4. **Menu không ẩn:** Check CurrentUser trong session

---

# 👤 THÀNH VIÊN 2: Suppliers, Customers & Stock Adjustment

## 📂 FILES/FOLDERS ĐẢM NHẬN

### ✅ Files đã tạo sẵn (6 files)

#### Models (3 files):
```
Models/
├── Supplier_T2.cs                ✅ Created
├── Customer_T2.cs                ✅ Created
└── StockAdjustment_T2.cs         ✅ Created
```

#### ViewModels (3 files):
```
ViewModels/
├── SuppliersViewModel_T2.cs      ✅ Created (with TODO comments)
├── CustomersViewModel_T2.cs      ✅ Created (with TODO comments)
└── StockAdjustmentViewModel_T2.cs ✅ Created (with TODO comments)
```

### 🔨 Files cần tạo mới (6 files)

#### Views (6 files):
```
Views/
├── SuppliersView_T2.xaml         🔨 TODO - Create from template
├── SuppliersView_T2.xaml.cs      🔨 TODO
├── CustomersView_T2.xaml         🔨 TODO - Create from template
├── CustomersView_T2.xaml.cs      🔨 TODO
├── StockAdjustmentView_T2.xaml   🔨 TODO - Create from template
└── StockAdjustmentView_T2.xaml.cs 🔨 TODO
```

### 🔄 Files cần cập nhật/modify

```
Views/
├── InboundView.xaml              🔄 Add Supplier ComboBox
├── OutboundView.xaml             🔄 Add Customer ComboBox

ViewModels/
├── InboundViewModel.cs           🔄 Load suppliers, link FK
├── OutboundViewModel.cs          🔄 Load customers, link FK

Services/
└── AppDbContext.cs               🔄 Add DbSet for T2 models

MainWindow.xaml                   🔄 Add menu items
```

---

## 🎯 CÁC BƯỚC CẦN LÀM (TUẦN 1-4)

### 📅 TUẦN 1: Database Setup & Supplier Module

#### Bước 1: Cập nhật AppDbContext (30 phút)
**File:** `Services/AppDbContext.cs`

**Thêm vào class:**
```csharp
// THÀNH VIÊN 2: Suppliers & Customers Models
public DbSet<Supplier_T2> Suppliers_T2 { get; set; }
public DbSet<Customer_T2> Customers_T2 { get; set; }
public DbSet<StockAdjustment_T2> StockAdjustments_T2 { get; set; }
```

**Thêm vào OnModelCreating:**
```csharp
// Configure relationships
modelBuilder.Entity<Supplier_T2>()
    .HasMany(s => s.InboundLogs)
    .WithOne()
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Customer_T2>()
    .HasMany(c => c.OutboundLogs)
    .WithOne()
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<StockAdjustment_T2>()
    .HasOne(sa => sa.Product)
    .WithMany()
    .HasForeignKey(sa => sa.ProductId)
    .OnDelete(DeleteBehavior.Restrict);
```

---

#### Bước 2: Chạy Migration (10 phút)
```powershell
Add-Migration AddSuppliersCustomersStockTables_T2
Update-Database
```

**Verify:** 3 tables mới
- Supplier_T2
- Customer_T2
- StockAdjustment_T2

---

#### Bước 3: Tạo SuppliersView UI (3-4 giờ)
**File:** `Views/SuppliersView_T2.xaml` (NEW)

**Copy template từ UsersView_T1.xaml và modify:**

```xml
<UserControl x:Class="GoodManagement.Views.SuppliersView_T2"
             xmlns:materialDesign="...">
    
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <!-- Header -->
        <TextBlock Text="Supplier Management" 
                   FontSize="24" FontWeight="Bold"/>

        <!-- Toolbar -->
        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="0,20,0,20">
            <Button Content="Add Supplier" 
                    Command="{Binding AddSupplierCommand}"/>
            <Button Content="Edit" 
                    Command="{Binding EditSupplierCommand}"/>
            <Button Content="Delete" 
                    Command="{Binding DeleteSupplierCommand}"/>
            <Button Content="View Purchase History" 
                    Command="{Binding ViewPurchaseHistoryCommand}"/>
            <Button Content="Export Excel" 
                    Command="{Binding ExportToExcelCommand}"/>
            <TextBox Width="250"
                     Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}"
                     materialDesign:HintAssist.Hint="Search suppliers..."/>
            <CheckBox Content="Active only" 
                      IsChecked="{Binding ShowActiveOnly}"/>
        </StackPanel>

        <!-- DataGrid -->
        <DataGrid Grid.Row="2"
                  ItemsSource="{Binding Suppliers}"
                  SelectedItem="{Binding SelectedSupplier}"
                  AutoGenerateColumns="False">
            <DataGrid.Columns>
                <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="50"/>
                <DataGridTextColumn Header="Supplier Name" Binding="{Binding SupplierName}" Width="*"/>
                <DataGridTextColumn Header="Contact Person" Binding="{Binding ContactPerson}" Width="150"/>
                <DataGridTextColumn Header="Phone" Binding="{Binding PhoneNumber}" Width="120"/>
                <DataGridTextColumn Header="Email" Binding="{Binding Email}" Width="*"/>
                <DataGridTextColumn Header="Tax Code" Binding="{Binding TaxCode}" Width="100"/>
                <DataGridCheckBoxColumn Header="Active" Binding="{Binding IsActive}" Width="80"/>
            </DataGrid.Columns>
        </DataGrid>
    </Grid>
</UserControl>
```

**Code-behind:**
```csharp
public partial class SuppliersView_T2 : UserControl
{
    public SuppliersView_T2()
    {
        InitializeComponent();
        DataContext = new SuppliersViewModel_T2();
    }
}
```

---

#### Bước 4: Implement Supplier CRUD (3-4 giờ)
**File:** `ViewModels/SuppliersViewModel_T2.cs`

**Logic template đã có, implement dialogs:**

1. **AddSupplier():** Create AddSupplierDialog.xaml
2. **EditSupplier():** EditSupplierDialog.xaml
3. **DeleteSupplier():** Check có inbound history không
4. **ViewPurchaseHistory():** Show list of inbound logs

---

#### Bước 5: Excel Export (2-3 giờ)
**Cần install EPPlus:**
```powershell
Install-Package EPPlus -Version 7.0.0
```

**File:** `ViewModels/SuppliersViewModel_T2.cs`

```csharp
private void ExportToExcel()
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    
    using (var package = new ExcelPackage())
    {
        var worksheet = package.Workbook.Worksheets.Add("Suppliers");
        
        // Headers
        worksheet.Cells["A1"].Value = "Supplier Name";
        worksheet.Cells["B1"].Value = "Contact Person";
        worksheet.Cells["C1"].Value = "Phone";
        worksheet.Cells["D1"].Value = "Email";
        worksheet.Cells["E1"].Value = "Address";
        worksheet.Cells["F1"].Value = "Tax Code";
        
        // Data
        int row = 2;
        foreach (var supplier in Suppliers)
        {
            worksheet.Cells[row, 1].Value = supplier.SupplierName;
            worksheet.Cells[row, 2].Value = supplier.ContactPerson;
            worksheet.Cells[row, 3].Value = supplier.PhoneNumber;
            worksheet.Cells[row, 4].Value = supplier.Email;
            worksheet.Cells[row, 5].Value = supplier.Address;
            worksheet.Cells[row, 6].Value = supplier.TaxCode;
            row++;
        }
        
        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        
        // Save
        var saveDialog = new SaveFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            FileName = $"Suppliers_{DateTime.Now:yyyyMMdd}.xlsx"
        };
        
        if (saveDialog.ShowDialog() == true)
        {
            package.SaveAs(new FileInfo(saveDialog.FileName));
            MessageBox.Show("Export successful!");
        }
    }
}
```

---

### 📅 TUẦN 2: Customer Module

#### Bước 6: Tạo CustomersView UI (3-4 giờ)
**File:** `Views/CustomersView_T2.xaml` (NEW)

**Similar to SuppliersView, thêm:**
- Customer Type dropdown (Retail, Wholesale, VIP)
- Loyalty Points column
- Total Purchase Amount column
- Add Points button

---

#### Bước 7: Implement Customer CRUD (3-4 giờ)
**File:** `ViewModels/CustomersViewModel_T2.cs`

**Additional features:**
1. **Loyalty Points Management:**
```csharp
private void AddLoyaltyPoints()
{
    if (SelectedCustomer == null) return;
    
    var dialog = new AddPointsDialog();
    if (dialog.ShowDialog() == true)
    {
        SelectedCustomer.LoyaltyPoints += dialog.Points;
        _context.SaveChanges();
        MessageBox.Show($"Added {dialog.Points} points!");
    }
}
```

2. **Auto-calculate points from purchases:**
```csharp
// In OutboundViewModel, after create transaction
var customer = await _context.Customers_T2.FindAsync(customerId);
if (customer != null)
{
    customer.TotalPurchaseAmount += totalPrice;
    customer.LoyaltyPoints += (int)(totalPrice / 10000); // 1 point per 10,000 VND
    await _context.SaveChangesAsync();
}
```

---

### 📅 TUẦN 3: Stock Adjustment Module

#### Bước 8: Tạo StockAdjustmentView UI (4-5 giờ)
**File:** `Views/StockAdjustmentView_T2.xaml` (NEW)

**Layout:**
```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="400"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
    
    <!-- Left: Adjustment Form -->
    <StackPanel Grid.Column="0" Margin="20">
        <TextBlock Text="Stock Adjustment" FontSize="20"/>
        
        <ComboBox ItemsSource="{Binding Products}"
                  SelectedItem="{Binding SelectedProduct}"
                  DisplayMemberPath="ProductName"
                  materialDesign:HintAssist.Hint="Select Product"/>
        
        <TextBlock Text="{Binding CurrentStock, StringFormat='Current Stock: {0}'}"/>
        
        <ComboBox ItemsSource="{Binding AdjustmentTypes}"
                  SelectedItem="{Binding SelectedAdjustmentType}"
                  materialDesign:HintAssist.Hint="Type (IN/OUT)"/>
        
        <ComboBox ItemsSource="{Binding AdjustmentReasons}"
                  SelectedItem="{Binding SelectedReason}"
                  materialDesign:HintAssist.Hint="Reason"/>
        
        <TextBox Text="{Binding AdjustmentQuantity}"
                 materialDesign:HintAssist.Hint="Quantity"/>
        
        <TextBlock Text="{Binding QuantityAfter, StringFormat='After: {0}'}"/>
        
        <TextBox Text="{Binding Notes}"
                 materialDesign:HintAssist.Hint="Notes"
                 AcceptsReturn="True"
                 Height="100"/>
        
        <Button Content="Create Adjustment" 
                Command="{Binding CreateAdjustmentCommand}"/>
    </StackPanel>
    
    <!-- Right: History -->
    <DataGrid Grid.Column="1"
              ItemsSource="{Binding Adjustments}">
        <!-- Columns... -->
    </DataGrid>
</Grid>
```

---

#### Bước 9: Implement Stock Adjustment Logic (3-4 giờ)
**File:** `ViewModels/StockAdjustmentViewModel_T2.cs`

**Logic template đã có, test:**
1. Select product
2. Enter quantity
3. Create adjustment
4. Verify inventory updated

---

### 📅 TUẦN 4: Integration

#### Bước 10: Update Inbound/Outbound Forms (2-3 giờ)

**File:** `Views/InboundView.xaml`

**Replace TextBox bằng ComboBox:**
```xml
<!-- OLD: -->
<TextBox Text="{Binding SupplierName}" 
         materialDesign:HintAssist.Hint="Supplier"/>

<!-- NEW: -->
<ComboBox ItemsSource="{Binding Suppliers}"
          SelectedItem="{Binding SelectedSupplier}"
          DisplayMemberPath="SupplierName"
          materialDesign:HintAssist.Hint="Select Supplier"/>
```

**File:** `ViewModels/InboundViewModel.cs`

```csharp
// Add property
public ObservableCollection<Supplier_T2> Suppliers { get; set; }
private Supplier_T2? _selectedSupplier;
public Supplier_T2? SelectedSupplier
{
    get => _selectedSupplier;
    set { _selectedSupplier = value; OnPropertyChanged(); }
}

// Load suppliers
private async Task LoadSuppliersAsync()
{
    var suppliers = await _context.Suppliers_T2
        .Where(s => s.IsActive)
        .ToListAsync();
    Suppliers = new ObservableCollection<Supplier_T2>(suppliers);
}

// When creating inbound, use supplier ID
var inbound = new InboundLog
{
    // ... other fields
    Supplier = SelectedSupplier?.SupplierName,
    SupplierId = SelectedSupplier?.Id // Add this FK to InboundLog model
};
```

**Tương tự cho OutboundView & OutboundViewModel với Customers**

---

#### Bước 11: Update MainWindow Menu (1 giờ)
```xml
<MenuItem Header="Business Relations">
    <MenuItem Header="Suppliers" 
              Command="{Binding NavigateCommand}"
              CommandParameter="SuppliersView_T2"/>
    <MenuItem Header="Customers" 
              Command="{Binding NavigateCommand}"
              CommandParameter="CustomersView_T2"/>
    <MenuItem Header="Stock Adjustment" 
              Command="{Binding NavigateCommand}"
              CommandParameter="StockAdjustmentView_T2"/>
</MenuItem>
```

---

## ✅ CHECKLIST HOÀN THÀNH

### Database:
- [ ] Migration chạy thành công
- [ ] 3 tables được tạo
- [ ] Foreign keys vào InboundLog, OutboundLog

### Supplier Management:
- [ ] SuppliersView_T2.xaml hoàn chỉnh
- [ ] Add/Edit/Delete suppliers works
- [ ] Search & filter works
- [ ] Excel export works
- [ ] Excel import works (bonus)
- [ ] Purchase history view

### Customer Management:
- [ ] CustomersView_T2.xaml hoàn chỉnh
- [ ] Add/Edit/Delete customers works
- [ ] Loyalty points calculation works
- [ ] Customer type classification works
- [ ] Excel export works
- [ ] Sales history view

### Stock Adjustment:
- [ ] StockAdjustmentView_T2.xaml hoàn chỉnh
- [ ] Create adjustment works
- [ ] Inventory auto-updates
- [ ] Adjustment history view
- [ ] Filter by date/product/reason
- [ ] Stock taking feature (bonus)

### Integration:
- [ ] InboundView uses Supplier dropdown
- [ ] OutboundView uses Customer dropdown
- [ ] Foreign keys populated correctly
- [ ] Menu items added
- [ ] Audit logs integrated (by T1)

---

## 📚 TÀI LIỆU THAM KHẢO

- Template từ UsersView_T1.xaml
- EPPlus documentation
- ViewModels đã có TODO comments

---

# 👤 THÀNH VIÊN 3: Reports, Analytics & Export

## 📂 FILES/FOLDERS ĐẢM NHẬN

### ✅ Files đã tạo sẵn (3 files)

#### ViewModels (2 files):
```
ViewModels/
├── AdvancedReportsViewModel_T3.cs ✅ Created (with TODO)
└── AuditLogsViewModel_T3.cs       ✅ Created (with TODO)
```

#### Services (1 file):
```
Services/
└── ExportService_T3.cs            ✅ Template (need implement)
```

### 🔨 Files cần tạo mới (4 files)

#### Views (4 files):
```
Views/
├── AdvancedReportsView_T3.xaml    🔨 TODO
├── AdvancedReportsView_T3.xaml.cs 🔨 TODO
├── AuditLogsView_T3.xaml          🔨 TODO
└── AuditLogsView_T3.xaml.cs       🔨 TODO
```

### 🔄 Files cần cập nhật/modify

```
Views/
├── DashboardView.xaml             🔄 Add charts
├── ReportsView.xaml               🔄 Add export buttons

ViewModels/
├── DashboardViewModel.cs          🔄 Data for charts
├── ReportsViewModel.cs            🔄 Link ExportService

Services/
└── ExportService_T3.cs            🔄 Implement all methods

MainWindow.xaml                    🔄 Add menu items
```

---

## 🎯 CÁC BƯỚC CẦN LÀM (TUẦN 1-4)

### 📅 TUẦN 1: Excel Export (PRIORITY!)

#### Bước 1: Install Packages (10 phút)
```powershell
Install-Package EPPlus -Version 7.0.0
Install-Package iTextSharp.LGPLv2.Core -Version 3.4.0
Install-Package LiveChartsCore.SkiaSharpView.WPF -Version 2.0.0-rc2
Install-Package Newtonsoft.Json
```

---

#### Bước 2: Implement Excel Export (4-5 giờ)
**File:** `Services/ExportService_T3.cs`

**Replace TODO với implementation:**

```csharp
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

public static async Task<string?> ExportToExcelAsync<T>(
    IEnumerable<T> data,
    string fileName,
    string sheetName = "Sheet1")
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    
    using (var package = new ExcelPackage())
    {
        var worksheet = package.Workbook.Worksheets.Add(sheetName);
        
        // Get properties
        var properties = typeof(T).GetProperties();
        
        // Add headers with formatting
        for (int i = 0; i < properties.Length; i++)
        {
            var cell = worksheet.Cells[1, i + 1];
            cell.Value = properties[i].Name;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        
        // Add data
        int row = 2;
        foreach (var item in data)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var cell = worksheet.Cells[row, col + 1];
                var value = properties[col].GetValue(item);
                
                // Format dates
                if (value is DateTime dateValue)
                {
                    cell.Value = dateValue;
                    cell.Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                }
                else
                {
                    cell.Value = value?.ToString() ?? "";
                }
                
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            row++;
        }
        
        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        
        // Add filters
        worksheet.Cells[1, 1, 1, properties.Length].AutoFilter = true;
        
        // Save
        var filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            fileName
        );
        
        await package.SaveAsAsync(new FileInfo(filePath));
        return filePath;
    }
}
```

---

#### Bước 3: Test Excel Export (1-2 giờ)
**Tạo test cases trong các ViewModels:**

1. **Products Export:**
```csharp
// In ProductsViewModel
public ICommand ExportToExcelCommand => new RelayCommand(async _ =>
{
    var products = await _context.Products.ToListAsync();
    var filePath = await ExportService_T3.ExportToExcelAsync(
        products,
        $"Products_{DateTime.Now:yyyyMMdd}.xlsx",
        "Products"
    );
    MessageBox.Show($"Exported to: {filePath}");
});
```

2. **Inventory Export**
3. **Inbound/Outbound Export**
4. **Suppliers/Customers Export** (by T2)

---

### 📅 TUẦN 2: Charts & Dashboard Enhancement

#### Bước 4: Setup LiveCharts (1 giờ)
**File:** `App.xaml`

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- Existing -->
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml"/>
            
            <!-- Add LiveCharts -->
            <ResourceDictionary Source="pack://application:,,,/LiveChartsCore.SkiaSharpView.WPF;component/Themes/Generic.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

---

#### Bước 5: Add Chart to Dashboard (3-4 giờ)
**File:** `Views/DashboardView.xaml`

**Thêm vào Grid:**
```xml
<Grid Grid.Row="2">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
    
    <!-- Line Chart: Stock Levels -->
    <GroupBox Grid.Column="0" Header="Stock Levels Trend" Margin="10">
        <lvc:CartesianChart Series="{Binding StockSeries}"
                           XAxes="{Binding XAxes}"
                           YAxes="{Binding YAxes}">
        </lvc:CartesianChart>
    </GroupBox>
    
    <!-- Pie Chart: Category Distribution -->
    <GroupBox Grid.Column="1" Header="Products by Category" Margin="10">
        <lvc:PieChart Series="{Binding CategorySeries}"
                     LegendPosition="Right">
        </lvc:PieChart>
    </GroupBox>
</Grid>
```

**Thêm namespace:**
```xml
xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF"
```

---

#### Bước 6: Implement Chart Data (2-3 giờ)
**File:** `ViewModels/DashboardViewModel.cs`

```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

public ISeries[] StockSeries { get; set; }
public Axis[] XAxes { get; set; }
public Axis[] YAxes { get; set; }
public ISeries[] CategorySeries { get; set; }

private async Task LoadChartsDataAsync()
{
    // Line Chart: Last 7 days stock
    var last7Days = Enumerable.Range(0, 7)
        .Select(i => DateTime.Now.AddDays(-6 + i).Date)
        .ToList();
    
    var stockData = new List<double>();
    foreach (var date in last7Days)
    {
        var total = await _context.Inventories
            .Where(i => i.LastUpdated.Date <= date)
            .SumAsync(i => i.Quantity);
        stockData.Add(total);
    }
    
    StockSeries = new ISeries[]
    {
        new LineSeries<double>
        {
            Values = stockData,
            Name = "Total Stock",
            Fill = null,
            GeometrySize = 10
        }
    };
    
    XAxes = new Axis[]
    {
        new Axis
        {
            Labels = last7Days.Select(d => d.ToString("dd/MM")).ToArray()
        }
    };
    
    YAxes = new Axis[]
    {
        new Axis { Name = "Quantity" }
    };
    
    // Pie Chart: Products by category
    var categories = await _context.Products
        .GroupBy(p => p.Category)
        .Select(g => new { Category = g.Key, Count = g.Count() })
        .ToListAsync();
    
    CategorySeries = categories.Select(c => new PieSeries<double>
    {
        Values = new[] { (double)c.Count },
        Name = c.Category ?? "Unknown"
    }).ToArray();
    
    OnPropertyChanged(nameof(StockSeries));
    OnPropertyChanged(nameof(CategorySeries));
}
```

---

### 📅 TUẦN 3: Advanced Reports & PDF Export

#### Bước 7: Tạo AdvancedReportsView UI (3-4 giờ)
**File:** `Views/AdvancedReportsView_T3.xaml` (NEW)

```xml
<UserControl>
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- Header -->
        <TextBlock Text="Advanced Reports & Analytics" 
                   FontSize="24" FontWeight="Bold"/>
        
        <!-- Filters & Controls -->
        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="0,20">
            <DatePicker SelectedDate="{Binding FromDate}"
                       materialDesign:HintAssist.Hint="From Date"/>
            <DatePicker SelectedDate="{Binding ToDate}"
                       materialDesign:HintAssist.Hint="To Date"/>
            <ComboBox ItemsSource="{Binding ReportTypes}"
                     SelectedItem="{Binding SelectedReportType}"
                     Width="200"/>
            <Button Content="Generate Report" 
                   Command="{Binding GenerateReportCommand}"/>
            <Button Content="Export Excel" 
                   Command="{Binding ExportToExcelCommand}"/>
            <Button Content="Export PDF" 
                   Command="{Binding ExportToPdfCommand}"/>
        </StackPanel>
        
        <!-- Report Display Area -->
        <TabControl Grid.Row="2">
            <TabItem Header="Summary">
                <!-- Statistics cards -->
            </TabItem>
            <TabItem Header="Charts">
                <!-- Bar charts, pie charts -->
            </TabItem>
            <TabItem Header="Details">
                <!-- DataGrid with details -->
            </TabItem>
        </TabControl>
    </Grid>
</UserControl>
```

---

#### Bước 8: Implement PDF Export (3-4 giờ)
**File:** `Services/ExportService_T3.cs`

```csharp
using iTextSharp.text;
using iTextSharp.text.pdf;

public static async Task<string?> ExportToPdfAsync<T>(
    IEnumerable<T> data,
    string fileName,
    string title = "Report")
{
    var document = new Document(PageSize.A4, 25, 25, 30, 30);
    var filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        fileName
    );
    
    var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
    document.Open();
    
    // Add title
    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
    var titleParagraph = new Paragraph(title, titleFont)
    {
        Alignment = Element.ALIGN_CENTER,
        SpacingAfter = 20
    };
    document.Add(titleParagraph);
    
    // Add date
    var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
    document.Add(new Paragraph($"Generated: {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont));
    document.Add(new Paragraph(" "));
    
    // Create table
    var properties = typeof(T).GetProperties();
    var table = new PdfPTable(properties.Length)
    {
        WidthPercentage = 100
    };
    
    // Headers
    foreach (var prop in properties)
    {
        var cell = new PdfPCell(new Phrase(prop.Name, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };
        table.AddCell(cell);
    }
    
    // Data
    foreach (var item in data)
    {
        foreach (var prop in properties)
        {
            var value = prop.GetValue(item)?.ToString() ?? "";
            table.AddCell(new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
            {
                Padding = 5
            });
        }
    }
    
    document.Add(table);
    
    // Footer
    document.Add(new Paragraph(" "));
    document.Add(new Paragraph($"Total Records: {data.Count()}", dateFont));
    
    document.Close();
    
    return filePath;
}
```

---

### 📅 TUẦN 4: Audit Logs Viewer

#### Bước 9: Tạo AuditLogsView UI (3-4 giờ)
**File:** `Views/AuditLogsView_T3.xaml` (NEW)

```xml
<UserControl>
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Header -->
        <TextBlock Text="Audit Logs" FontSize="24" FontWeight="Bold"/>
        
        <!-- Filters -->
        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="0,20">
            <ComboBox ItemsSource="{Binding Users}"
                     SelectedItem="{Binding SelectedUser}"
                     DisplayMemberPath="Username"
                     Width="150"
                     materialDesign:HintAssist.Hint="User"/>
            
            <ComboBox ItemsSource="{Binding Actions}"
                     SelectedItem="{Binding SelectedAction}"
                     Width="120"
                     materialDesign:HintAssist.Hint="Action"/>
            
            <ComboBox ItemsSource="{Binding Tables}"
                     SelectedItem="{Binding SelectedTable}"
                     Width="150"
                     materialDesign:HintAssist.Hint="Table"/>
            
            <DatePicker SelectedDate="{Binding FromDate}"
                       materialDesign:HintAssist.Hint="From"/>
            
            <DatePicker SelectedDate="{Binding ToDate}"
                       materialDesign:HintAssist.Hint="To"/>
            
            <TextBox Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}"
                    Width="200"
                    materialDesign:HintAssist.Hint="Search..."/>
            
            <Button Content="Clear Filters" 
                   Command="{Binding ClearFiltersCommand}"/>
            
            <Button Content="Export" 
                   Command="{Binding ExportLogsCommand}"/>
        </StackPanel>
        
        <!-- DataGrid -->
        <DataGrid Grid.Row="2"
                  ItemsSource="{Binding AuditLogs}"
                  SelectedItem="{Binding SelectedLog}"
                  AutoGenerateColumns="False">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Time" 
                                   Binding="{Binding Timestamp, StringFormat=dd/MM/yyyy HH:mm:ss}" 
                                   Width="150"/>
                <DataGridTextColumn Header="User" 
                                   Binding="{Binding User.Username}" 
                                   Width="100"/>
                <DataGridTextColumn Header="Action" 
                                   Binding="{Binding Action}" 
                                   Width="80"/>
                <DataGridTextColumn Header="Table" 
                                   Binding="{Binding TableName}" 
                                   Width="120"/>
                <DataGridTextColumn Header="Record ID" 
                                   Binding="{Binding RecordId}" 
                                   Width="80"/>
                <DataGridTextColumn Header="Description" 
                                   Binding="{Binding Description}" 
                                   Width="*"/>
                <DataGridTemplateColumn Header="Details" Width="100">
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <Button Content="View" 
                                   Command="{Binding DataContext.ViewDetailsCommand, 
                                           RelativeSource={RelativeSource AncestorType=DataGrid}}"
                                   CommandParameter="{Binding}"/>
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
            </DataGrid.Columns>
        </DataGrid>
        
        <!-- Pagination -->
        <StackPanel Grid.Row="3" Orientation="Horizontal" 
                   HorizontalAlignment="Center" Margin="0,10">
            <Button Content="Previous" 
                   Command="{Binding PreviousPageCommand}"
                   Width="100"/>
            <TextBlock Text="{Binding CurrentPage}" 
                      VerticalAlignment="Center" 
                      Margin="20,0"/>
            <Button Content="Next" 
                   Command="{Binding NextPageCommand}"
                   Width="100"/>
            <TextBlock Text="{Binding TotalLogs, StringFormat='Total: {0} logs'}" 
                      VerticalAlignment="Center" 
                      Margin="20,0"/>
        </StackPanel>
    </Grid>
</UserControl>
```

---

#### Bước 10: Implement Details Viewer (2 giờ)
**Create dialog:** `Views/AuditLogDetailsDialog.xaml`

```xml
<Window>
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- Info -->
        <StackPanel Grid.Row="0">
            <TextBlock Text="{Binding SelectedLog.User.Username, StringFormat='User: {0}'}"/>
            <TextBlock Text="{Binding SelectedLog.Action, StringFormat='Action: {0}'}"/>
            <TextBlock Text="{Binding SelectedLog.Timestamp, StringFormat='Time: {0:dd/MM/yyyy HH:mm:ss}'}"/>
        </StackPanel>
        
        <!-- Old Value -->
        <GroupBox Grid.Row="1" Header="Before (Old Value)" Margin="0,10">
            <TextBox Text="{Binding SelectedLog.OldValue}" 
                    IsReadOnly="True"
                    VerticalScrollBarVisibility="Auto"
                    TextWrapping="Wrap"/>
        </GroupBox>
        
        <!-- New Value -->
        <GroupBox Grid.Row="2" Header="After (New Value)" Margin="0,10">
            <TextBox Text="{Binding SelectedLog.NewValue}" 
                    IsReadOnly="True"
                    VerticalScrollBarVisibility="Auto"
                    TextWrapping="Wrap"/>
        </GroupBox>
    </Grid>
</Window>
```

---

#### Bước 11: Update MainWindow Menu (30 phút)
```xml
<MenuItem Header="Reports">
    <MenuItem Header="Basic Reports" 
              Command="{Binding NavigateCommand}"
              CommandParameter="ReportsView"/>
    <MenuItem Header="Advanced Reports" 
              Command="{Binding NavigateCommand}"
              CommandParameter="AdvancedReportsView_T3"/>
    <MenuItem Header="Audit Logs" 
              Command="{Binding NavigateCommand}"
              CommandParameter="AuditLogsView_T3"
              Visibility="{Binding IsAdminVisible}"/>
</MenuItem>
```

---

## ✅ CHECKLIST HOÀN THÀNH

### Excel Export:
- [ ] ExportService_T3 implemented
- [ ] Products export works
- [ ] Inventory export works
- [ ] Transactions export works
- [ ] Suppliers/Customers export works (T2 data)
- [ ] Formatting & styling applied

### PDF Export:
- [ ] PDF export implemented
- [ ] Reports export to PDF
- [ ] Invoices/receipts (bonus)
- [ ] Headers & footers

### Charts:
- [ ] LiveCharts setup
- [ ] Dashboard charts working
- [ ] Line chart (trend)
- [ ] Bar chart (comparison)
- [ ] Pie chart (distribution)
- [ ] Tooltips & legends

### Advanced Reports:
- [ ] AdvancedReportsView_T3.xaml created
- [ ] Profit/Loss report
- [ ] ABC Analysis
- [ ] Stock movement report
- [ ] Custom date ranges
- [ ] Multiple filters

### Audit Logs:
- [ ] AuditLogsView_T3.xaml created
- [ ] Load logs with pagination
- [ ] Filters work (user, action, table, date)
- [ ] Search functionality
- [ ] Details viewer (before/after)
- [ ] Export audit trail

---

## 📚 TÀI LIỆU THAM KHẢO

- EPPlus documentation: https://github.com/EPPlusSoftware/EPPlus
- iTextSharp documentation
- LiveCharts documentation: https://livecharts.dev/
- Code có TODO comments

---

# 📊 TỔNG KẾT & TIMELINE

## 🗓️ TIMELINE 6 TUẦN

### Tuần 1: Setup & Foundation
**All members:**
- [ ] Git branches created
- [ ] Packages installed
- [ ] Database migration run
- [ ] Seed data populated

**T1:** User models + migration  
**T2:** Supplier/Customer models + migration  
**T3:** Excel export implementation

### Tuần 2: Core Features
**T1:** User CRUD + UI  
**T2:** Supplier CRUD + UI  
**T3:** Charts integration

### Tuần 3: Advanced Features
**T1:** Settings module  
**T2:** Customer + Stock Adjustment  
**T3:** PDF export + Advanced Reports

### Tuần 4: Integration
**T1:** Audit integration + Login update  
**T2:** Link Inbound/Outbound with Suppliers/Customers  
**T3:** Audit Logs Viewer

### Tuần 5: Testing & Bug Fixes
**All:** Integration testing, bug fixes, UI polish

### Tuần 6: Documentation & Demo
**All:** User manual, demo video, presentation

---

## 📞 COORDINATION POINTS

### Daily Standup (15 phút mỗi ngày):
- What did you do yesterday?
- What will you do today?
- Any blockers?

### Weekly Integration (Thứ 6 hàng tuần):
- Merge branches to main
- Resolve conflicts
- Integration testing

### Communication Channels:
- Discord/Slack: Daily chat
- GitHub Issues: Track bugs
- Pull Requests: Code review

---

## 🎯 SUCCESS METRICS

- [ ] All 14 modules working
- [ ] No compilation errors
- [ ] All CRUD operations tested
- [ ] Export features working
- [ ] Charts displaying correctly
- [ ] Audit logs integrated
- [ ] Documentation complete
- [ ] Demo script ready

---

## 🏆 EXPECTED RESULT

**Với roadmap này:**
- ✅ Mỗi thành viên có scope rõ ràng
- ✅ Không conflict code (suffix riêng)
- ✅ Timeline realistic (6 tuần)
- ✅ Features đầy đủ (14 modules)
- ✅ Code quality cao
- ✅ Documentation đầy đủ

**Target Grade: 9.5-10/10** 🎯🏆

---

**Good luck to all team members! 🚀**

*Last updated: October 27, 2025*
