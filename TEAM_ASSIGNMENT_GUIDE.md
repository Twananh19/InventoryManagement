# 👥 Phân Chia Công Việc Theo Thành Viên

## 📋 Tổng Quan

Dự án được chia thành 3 modules độc lập để 3 thành viên có thể làm việc song song mà không conflict code.

**Quy ước đặt tên:**
- Files của **Thành viên 1** có suffix `_T1`
- Files của **Thành viên 2** có suffix `_T2`  
- Files của **Thành viên 3** có suffix `_T3`

---

## 👤 THÀNH VIÊN 1: User Management & Settings Module

### 📂 Files cần làm việc:

#### Models (✅ Đã tạo):
- `Models/User_T1.cs`
- `Models/Role_T1.cs`
- `Models/AuditLog_T1.cs`
- `Models/SystemSettings_T1.cs`

#### ViewModels (✅ Đã tạo):
- `ViewModels/UsersViewModel_T1.cs`
- `ViewModels/SettingsViewModel_T1.cs`

#### Views (🔧 Cần hoàn thiện):
- `Views/UsersView_T1.xaml` (✅ đã tạo template)
- `Views/UsersView_T1.xaml.cs`
- `Views/SettingsView_T1.xaml` (❌ cần tạo)
- `Views/SettingsView_T1.xaml.cs` (❌ cần tạo)

#### Services (✅ Đã tạo):
- `Services/AuditService_T1.cs`

### 🎯 Nhiệm vụ cụ thể:

#### 1️⃣ User Management (Ưu tiên cao)
- [ ] Hoàn thiện UI `UsersView_T1.xaml`
- [ ] Implement Add User Dialog
  - Form validation (required fields)
  - Password hashing với BCrypt
  - Assign role
- [ ] Implement Edit User Dialog
  - Load existing user data
  - Update user info
  - Change role
- [ ] Implement Delete User
  - Confirmation dialog
  - Check if user has related data
- [ ] Implement Change Password Dialog
  - Current password verification
  - New password confirmation
  - Password strength indicator
- [ ] Implement Search & Filter
  - Search by username/name/email
  - Filter by role
  - Filter by active status

#### 2️⃣ Role Management
- [ ] Tạo `Views/RolesView_T1.xaml`
- [ ] Tạo `ViewModels/RolesViewModel_T1.cs`
- [ ] CRUD Operations cho Roles
- [ ] Permissions matrix UI
- [ ] Assign permissions to roles

#### 3️⃣ Settings Module
- [ ] Tạo `Views/SettingsView_T1.xaml`
- [ ] Implement Settings categories:
  - Company Information
  - Inventory Settings (Low stock threshold)
  - Security Settings (Password policy)
  - Notification Settings
- [ ] Implement Save/Load Settings
- [ ] Implement Backup/Restore Database

#### 4️⃣ Integration
- [ ] Integrate AuditService vào tất cả CRUD operations
- [ ] Test user login với new User table
- [ ] Update MainWindow menu để thêm User Management
- [ ] Implement role-based access control (show/hide menu items)

### 📚 Kiến thức cần có:
- WPF XAML & Data Binding
- Entity Framework Core
- BCrypt password hashing
- Material Design
- Async/Await patterns

### ⏱️ Thời gian ước tính: 3-4 tuần

---

## 👤 THÀNH VIÊN 2: Suppliers, Customers & Stock Adjustment Module

### 📂 Files cần làm việc:

#### Models (✅ Đã tạo):
- `Models/Supplier_T2.cs`
- `Models/Customer_T2.cs`
- `Models/StockAdjustment_T2.cs`

#### ViewModels (✅ Đã tạo):
- `ViewModels/SuppliersViewModel_T2.cs`
- `ViewModels/CustomersViewModel_T2.cs`
- `ViewModels/StockAdjustmentViewModel_T2.cs`

#### Views (❌ Cần tạo):
- `Views/SuppliersView_T2.xaml`
- `Views/SuppliersView_T2.xaml.cs`
- `Views/CustomersView_T2.xaml`
- `Views/CustomersView_T2.xaml.cs`
- `Views/StockAdjustmentView_T2.xaml`
- `Views/StockAdjustmentView_T2.xaml.cs`

### 🎯 Nhiệm vụ cụ thể:

#### 1️⃣ Supplier Management (Ưu tiên cao)
- [ ] Tạo `Views/SuppliersView_T2.xaml`
- [ ] Implement CRUD Operations
  - Add Supplier Dialog (form với validation)
  - Edit Supplier Dialog
  - Delete Supplier (with confirmation)
- [ ] Implement Search & Filter
  - Search by name/phone/email
  - Filter by active status
- [ ] View Purchase History
  - Show all inbound logs from supplier
  - Calculate total purchase amount
- [ ] Export Suppliers to Excel
  - Use EPPlus library
  - Generate Excel template for import
- [ ] Import Suppliers from Excel

#### 2️⃣ Customer Management
- [ ] Tạo `Views/CustomersView_T2.xaml`
- [ ] Implement CRUD Operations
- [ ] Implement Search & Filter
  - Filter by customer type (Retail/Wholesale/VIP)
- [ ] View Purchase History per customer
- [ ] Loyalty Points Management
  - Add/subtract points
  - Calculate points from purchases
- [ ] Top Customers Report
  - By revenue
  - By order count
  - By loyalty points

#### 3️⃣ Stock Adjustment Module (Quan trọng!)
- [ ] Tạo `Views/StockAdjustmentView_T2.xaml`
- [ ] Implement Stock Adjustment Form
  - Select product
  - Choose adjustment type (IN/OUT)
  - Select reason (Damaged, Lost, Expired, etc.)
  - Enter quantity
  - Show before/after quantity
  - Add notes
- [ ] Auto-update Inventory after adjustment
- [ ] Implement Approval workflow (for large adjustments)
- [ ] View Adjustment History
  - Filter by date range
  - Filter by product
  - Filter by reason
- [ ] Stock Taking Feature
  - Create stock take session
  - List all products
  - Enter actual quantity
  - Compare with system
  - Auto-generate adjustments for differences

#### 4️⃣ Integration
- [ ] Link Suppliers with InboundLog
- [ ] Link Customers with OutboundLog
- [ ] Update existing Inbound/Outbound forms to select from Supplier/Customer lists
- [ ] Update MainWindow menu
- [ ] Test all CRUD operations

### 📚 Kiến thức cần có:
- WPF XAML & Data Binding
- Entity Framework Core with relationships
- Material Design
- Excel Import/Export (EPPlus)
- Complex forms with validation

### ⏱️ Thời gian ước tính: 3-4 tuần

---

## 👤 THÀNH VIÊN 3: Reports, Analytics & Export Module

### 📂 Files cần làm việc:

#### ViewModels (✅ Đã tạo):
- `ViewModels/AdvancedReportsViewModel_T3.cs`
- `ViewModels/AuditLogsViewModel_T3.cs`

#### Views (❌ Cần tạo):
- `Views/AdvancedReportsView_T3.xaml`
- `Views/AdvancedReportsView_T3.xaml.cs`
- `Views/AuditLogsView_T3.xaml`
- `Views/AuditLogsView_T3.xaml.cs`

#### Services (✅ Đã tạo):
- `Services/ExportService_T3.cs`

### 🎯 Nhiệm vụ cụ thể:

#### 1️⃣ Excel Export (Ưu tiên cao!)
- [ ] Install EPPlus package
- [ ] Implement `ExportToExcelAsync` trong `ExportService_T3.cs`
- [ ] Tạo Excel templates với formatting
  - Headers với background color
  - Auto-fit columns
  - Borders
  - Data validation
- [ ] Implement export cho các modules:
  - Products list
  - Inventory report
  - Inbound/Outbound logs
  - Suppliers/Customers list
  - Audit logs
- [ ] Add summary rows (totals, averages)
- [ ] Add charts trong Excel

#### 2️⃣ PDF Export
- [ ] Install iTextSharp.LGPLv2.Core package
- [ ] Implement `ExportToPdfAsync` trong `ExportService_T3.cs`
- [ ] Tạo PDF templates
  - Company logo & header
  - Title & date
  - Data table with formatting
  - Footer with page numbers
- [ ] Implement PDF export cho:
  - Sales reports
  - Inventory reports
  - Audit trail
  - Invoices (bonus)

#### 3️⃣ Charts & Graphs (Quan trọng!)
- [ ] Install LiveChartsCore.SkiaSharpView.WPF package
- [ ] Tạo `Views/AdvancedReportsView_T3.xaml`
- [ ] Implement các loại chart:
  - **Line Chart**: Revenue over time
  - **Bar Chart**: Inbound vs Outbound comparison
  - **Pie Chart**: Revenue distribution by product category
  - **Column Chart**: Top 10 best-selling products
  - **Area Chart**: Inventory levels over time
- [ ] Make charts interactive (tooltips, legends)
- [ ] Allow exporting charts as images

#### 4️⃣ Advanced Reports
- [ ] Profit/Loss Report
  - Calculate revenue from outbound
  - Calculate cost from inbound
  - Show profit margin
- [ ] Inventory Valuation Report
  - Current stock value
  - By category
- [ ] ABC Analysis
  - Classify products by revenue (A: 80%, B: 15%, C: 5%)
- [ ] Sales Trend Analysis
  - Daily/Weekly/Monthly trends
  - Forecasting (bonus)
- [ ] Stock Movement Report
  - Show all stock changes (inbound, outbound, adjustments)

#### 5️⃣ Audit Logs Viewer
- [ ] Tạo `Views/AuditLogsView_T3.xaml`
- [ ] Implement filters:
  - By user
  - By action (CREATE/UPDATE/DELETE)
  - By table
  - By date range
- [ ] Implement pagination (50 logs per page)
- [ ] View log details (before/after comparison)
- [ ] Export audit trail to Excel/PDF
- [ ] Search functionality

#### 6️⃣ Dashboard Enhancement
- [ ] Update existing Dashboard với charts
- [ ] Add real-time statistics
- [ ] Add quick export buttons
- [ ] Add notifications for low stock

### 📚 Kiến thức cần có:
- WPF XAML & Data Binding
- Entity Framework Core (LINQ queries)
- EPPlus library
- iTextSharp library
- LiveCharts2 library
- Data visualization concepts
- Statistical calculations

### ⏱️ Thời gian ước tính: 3-4 tuần

---

## 🔄 Workflow Làm Việc

### 1. Setup Git Branches
```bash
# Thành viên 1
git checkout -b feature/user-management-T1

# Thành viên 2
git checkout -b feature/suppliers-customers-T2

# Thành viên 3
git checkout -b feature/reports-export-T3
```

### 2. Daily Workflow
```bash
# Pull latest changes from main
git checkout main
git pull origin main

# Switch to your branch
git checkout feature/your-branch-name

# Merge main into your branch
git merge main

# Work on your tasks...

# Commit your changes
git add .
git commit -m "feat(T1): implement user management CRUD"

# Push to your branch
git push origin feature/your-branch-name
```

### 3. Code Review & Merge
- Tạo Pull Request khi hoàn thành feature
- Nhóm review code
- Merge vào main sau khi approve

### 4. Communication
- **Daily standup**: Báo cáo tiến độ hàng ngày
- **Weekly meeting**: Demo features đã hoàn thành
- **Slack/Discord**: Hỏi đáp khi gặp vấn đề

---

## 📦 Packages Cần Install

### Thành viên 1:
```powershell
Install-Package BCrypt.Net-Next
Install-Package Newtonsoft.Json
```

### Thành viên 2:
```powershell
Install-Package EPPlus
```

### Thành viên 3:
```powershell
Install-Package EPPlus
Install-Package iTextSharp.LGPLv2.Core
Install-Package LiveChartsCore.SkiaSharpView.WPF
```

---

## ✅ Definition of Done

Một feature được coi là hoàn thành khi:
- [ ] Code compile không lỗi
- [ ] UI hoàn chỉnh và đẹp (Material Design)
- [ ] CRUD operations hoạt động đúng
- [ ] Có validation và error handling
- [ ] Có confirmation dialogs cho delete
- [ ] Code có comments đầy đủ
- [ ] Đã test thủ công
- [ ] Đã commit code với message rõ ràng
- [ ] Không conflict với code của thành viên khác

---

## 🎯 Mục Tiêu Cuối Cùng

**Tuần 1-2**: Setup & Core Development
**Tuần 3-4**: Integration & Testing  
**Tuần 5**: Bug Fixing & Polish
**Tuần 6**: Documentation & Presentation

**Demo Day**: Tất cả features hoạt động mượt mà! 🚀

---

## 🆘 Khi Gặp Vấn Đề

1. **Check Documentation**: README, Development Guide
2. **Google/Stack Overflow**: Search error messages
3. **Ask Team**: Discord/Slack group chat
4. **Ask Teacher**: Nếu vấn đề phức tạp

---

## 🎓 Resources

- [WPF Tutorial](https://wpf-tutorial.com/)
- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Material Design in XAML](http://materialdesigninxaml.net/)
- [EPPlus Documentation](https://github.com/EPPlusSoftware/EPPlus)
- [LiveCharts Documentation](https://livecharts.dev/)

**Chúc các bạn code vui vẻ và đạt điểm cao! 💯**
