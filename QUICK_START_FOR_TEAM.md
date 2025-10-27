# 🚀 Quick Start Guide for Team Members

## 📌 Bạn là thành viên nào?

### 👤 THÀNH VIÊN 1: User Management & Settings
**Files của bạn có suffix `_T1`**

📂 **Your Files:**
- Models: `User_T1.cs`, `Role_T1.cs`, `AuditLog_T1.cs`, `SystemSettings_T1.cs`
- ViewModels: `UsersViewModel_T1.cs`, `SettingsViewModel_T1.cs`
- Views: `UsersView_T1.xaml`, `SettingsView_T1.xaml` (TODO)
- Services: `AuditService_T1.cs`

🎯 **Your Tasks:**
1. User CRUD (Add, Edit, Delete, Change Password)
2. Role & Permissions Management
3. System Settings (Company info, Security, Notifications)
4. Integrate Audit Logs into all operations

📦 **Packages to Install:**
```powershell
Install-Package BCrypt.Net-Next
Install-Package Newtonsoft.Json
```

---

### 👤 THÀNH VIÊN 2: Suppliers, Customers & Stock
**Files của bạn có suffix `_T2`**

📂 **Your Files:**
- Models: `Supplier_T2.cs`, `Customer_T2.cs`, `StockAdjustment_T2.cs`
- ViewModels: `SuppliersViewModel_T2.cs`, `CustomersViewModel_T2.cs`, `StockAdjustmentViewModel_T2.cs`
- Views: `SuppliersView_T2.xaml` (TODO), `CustomersView_T2.xaml` (TODO), `StockAdjustmentView_T2.xaml` (TODO)

🎯 **Your Tasks:**
1. Supplier CRUD + Purchase History
2. Customer CRUD + Loyalty Points
3. Stock Adjustment (Damaged, Lost, Expired, etc.)
4. Stock Taking Feature
5. Excel Import/Export

📦 **Packages to Install:**
```powershell
Install-Package EPPlus -Version 7.0.0
Install-Package Newtonsoft.Json
```

---

### 👤 THÀNH VIÊN 3: Reports, Analytics & Export
**Files của bạn có suffix `_T3`**

📂 **Your Files:**
- ViewModels: `AdvancedReportsViewModel_T3.cs`, `AuditLogsViewModel_T3.cs`
- Views: `AdvancedReportsView_T3.xaml` (TODO), `AuditLogsView_T3.xaml` (TODO)
- Services: `ExportService_T3.cs`

🎯 **Your Tasks:**
1. Excel Export (Products, Inventory, Reports)
2. PDF Export (Invoices, Reports)
3. Charts & Graphs (Line, Bar, Pie charts)
4. Advanced Reports (Profit/Loss, ABC Analysis)
5. Audit Logs Viewer (with filters & search)

📦 **Packages to Install:**
```powershell
Install-Package EPPlus -Version 7.0.0
Install-Package iTextSharp.LGPLv2.Core
Install-Package LiveChartsCore.SkiaSharpView.WPF -Version 2.0.0-rc2
Install-Package Newtonsoft.Json
```

---

## 🔧 Setup Steps (TẤT CẢ THÀNH VIÊN)

### 1. Pull latest code
```bash
git pull origin main
```

### 2. Install your packages
See above for your specific packages

### 3. Update Database
```powershell
# Open Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
Add-Migration AddNewModules_T1_T2_T3
Update-Database
```

### 4. Create your branch
```bash
git checkout -b feature/your-module-name-T1  # Replace T1 with your number
```

### 5. Start coding!
Focus on your files only (the ones with your suffix)

---

## 📚 Important Documents

1. **TEAM_ASSIGNMENT_GUIDE.md** - Chi tiết nhiệm vụ từng người
2. **DATABASE_MIGRATION_GUIDE.md** - Hướng dẫn database
3. **PACKAGES_INSTALLATION_GUIDE.md** - Hướng dẫn packages
4. **PROJECT_ENHANCEMENT_SUMMARY.md** - Tổng quan dự án

---

## ⚠️ Rules

1. **ONLY edit files with YOUR suffix** (_T1, _T2, or _T3)
2. **Commit often** with clear messages
3. **Test your code** before pushing
4. **Don't merge to main** without team review
5. **Ask questions** if stuck

---

## 🆘 Help

- **Slack/Discord**: Team chat
- **Daily Standup**: 9:00 AM every day
- **Code Review**: Before merging

---

## ✅ Weekly Goals

**Week 1**: Complete Models & ViewModels  
**Week 2**: Complete Views & Basic CRUD  
**Week 3**: Complete Advanced Features  
**Week 4**: Testing & Integration  
**Week 5**: Bug Fixing & Polish  
**Week 6**: Demo Preparation

---

**Let's build something amazing! 🚀**
