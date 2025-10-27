# 📚 Hướng dẫn Phát triển Dự án Good Management System

## 🎯 Tổng quan

Hệ thống quản lý kho hàng với kiến trúc **MVVM** (Model-View-ViewModel), sử dụng WPF và Material Design.

---

## 📁 Cấu trúc Dự án

### 1. **Models/** - Data Models
Chứa các entity class đại diện cho dữ liệu:
- `User.cs` - Thông tin người dùng
- `Product.cs` - Sản phẩm
- `Inventory.cs` - Tồn kho
- `InboundLog.cs` - Phiếu nhập hàng
- `OutboundLog.cs` - Phiếu xuất hàng

### 2. **Views/** - User Interface
Giao diện người dùng (XAML + Code-behind):
- ✅ `LoginWindow.xaml` - Màn hình đăng nhập
- ✅ `DashboardView.xaml` - Trang tổng quan
- ✅ `ProductsView.xaml` - Quản lý sản phẩm
- ✅ `InventoryView.xaml` - Quản lý tồn kho
- ✅ `InboundView.xaml` - Nhập hàng
- ✅ `OutboundView.xaml` - Xuất hàng
- ✅ `ReportsView.xaml` - Báo cáo & thống kê

### 3. **ViewModels/** - Business Logic
Logic xử lý nghiệp vụ:
- ✅ `LoginViewModel.cs` - Xử lý đăng nhập
- ✅ `MainViewModel.cs` - Điều phối chính
- ✅ `DashboardViewModel.cs` - Dashboard logic
- ✅ `ProductViewModel.cs` - CRUD sản phẩm
- ✅ `InventoryViewModel.cs` - Quản lý tồn kho
- ✅ `InboundViewModel.cs` - Nhập hàng
- ✅ `OutboundViewModel.cs` - Xuất hàng
- ✅ `ReportsViewModel.cs` - Báo cáo
- `ViewModelBase.cs` - Base class

### 4. **Services/** - Data Access
- `AppDbContext.cs` - EF Core DbContext

### 5. **Helpers/** - Utilities
- `RelayCommand.cs` - ICommand implementation
- `PasswordBoxHelper.cs` - Password binding helper

---

## 🚀 Quy trình Phát triển từng Module

### Module 1: Authentication ✅ HOÀN THÀNH
**Files:**
- `Views/LoginWindow.xaml`
- `ViewModels/LoginViewModel.cs`
- `Helpers/PasswordBoxHelper.cs`

**Chức năng:**
- Đăng nhập với username/password
- Validate thông tin
- Chuyển sang MainWindow khi thành công

---

### Module 2: Dashboard ✅ HOÀN THÀNH
**Files:**
- `Views/DashboardView.xaml`
- `ViewModels/DashboardViewModel.cs`

**Chức năng:**
- Hiển thị thống kê tổng quan
- Danh sách sản phẩm tồn kho thấp
- Quick actions
- Tự động cập nhật thời gian

**Cải thiện có thể làm:**
- [ ] Thêm biểu đồ (Chart)
- [ ] Hiển thị xu hướng theo thời gian
- [ ] Thông báo real-time

---

### Module 3: Products ✅ HOÀN THÀNH
**Files:**
- `Views/ProductsView.xaml`
- `ViewModels/ProductViewModel.cs`

**Chức năng:**
- CRUD sản phẩm đầy đủ
- Tìm kiếm real-time
- Form validation

**Cải thiện có thể làm:**
- [ ] Import/Export Excel
- [ ] Upload hình ảnh sản phẩm
- [ ] Quản lý danh mục sản phẩm
- [ ] Barcode/QR code

---

### Module 4: Inventory ✅ HOÀN THÀNH
**Files:**
- `Views/InventoryView.xaml`
- `ViewModels/InventoryViewModel.cs`

**Chức năng:**
- Xem tồn kho real-time
- Cảnh báo tồn thấp
- Cập nhật số lượng
- Hiển thị thống kê

**Cải thiện có thể làm:**
- [ ] Lịch sử thay đổi tồn kho
- [ ] Điều chỉnh ngưỡng cảnh báo theo từng SP
- [ ] Export inventory report
- [ ] Kiểm kê định kỳ

---

### Module 5: Inbound ✅ HOÀN THÀNH
**Files:**
- `Views/InboundView.xaml`
- `ViewModels/InboundViewModel.cs`

**Chức năng:**
- Tạo phiếu nhập mới
- Tự động cập nhật tồn kho
- Lịch sử nhập hàng
- Ghi nhận nhà cung cấp

**Cải thiện có thể làm:**
- [ ] Nhập nhiều sản phẩm cùng lúc (bulk)
- [ ] Quản lý nhà cung cấp riêng
- [ ] In phiếu nhập
- [ ] Upload chứng từ đính kèm
- [ ] Tính giá nhập tự động

---

### Module 6: Outbound ✅ HOÀN THÀNH
**Files:**
- `Views/OutboundView.xaml`
- `ViewModels/OutboundViewModel.cs`

**Chức năng:**
- Tạo phiếu xuất
- Kiểm tra tồn kho trước khi xuất
- Lịch sử xuất hàng
- Ghi nhận khách hàng

**Cải thiện có thể làm:**
- [ ] Xuất nhiều sản phẩm cùng lúc
- [ ] Quản lý khách hàng riêng
- [ ] In phiếu xuất/hóa đơn
- [ ] Tính tổng tiền tự động
- [ ] Trạng thái đơn hàng (pending, completed, cancelled)

---

### Module 7: Reports ✅ HOÀN THÀNH
**Files:**
- `Views/ReportsView.xaml`
- `ViewModels/ReportsViewModel.cs`

**Chức năng:**
- Báo cáo nhập/xuất theo thời gian
- Top sản phẩm bán chạy
- Tổng quan giao dịch
- Chênh lệch nhập-xuất

**Cải thiện có thể làm:**
- [x] Xuất Excel (cần implement)
- [ ] Biểu đồ trực quan (charts)
- [ ] Báo cáo lợi nhuận
- [ ] Dự báo nhu cầu
- [ ] Báo cáo chi tiết theo sản phẩm

---

## 🎨 UI/UX Improvements đã áp dụng

### ✅ Đã hoàn thành:
1. **Material Design** - Icons và components đẹp mắt
2. **Color Coding** - Màu sắc phân biệt rõ ràng:
   - 🟢 Xanh lá: Nhập hàng, tồn kho tốt
   - 🔴 Đỏ: Xuất hàng, cảnh báo
   - 🔵 Xanh dương: Thông tin, sản phẩm
   - 🟣 Tím: Báo cáo, thống kê
   - 🟠 Cam: Cảnh báo tồn thấp

3. **Responsive Layout** - Cards với elevation và spacing phù hợp
4. **Visual Feedback** - Hover effects, selected states
5. **Typography Hierarchy** - Font sizes và weights rõ ràng

### 🎯 Có thể cải thiện thêm:
- [ ] Dark mode
- [ ] Animations và transitions
- [ ] Loading spinners
- [ ] Toast notifications
- [ ] Confirmation dialogs với animation
- [ ] Drag & drop
- [ ] Context menus

---

## 📝 Quy tắc Code

### 1. Naming Convention
```csharp
// ViewModels
public class ProductViewModel : ViewModelBase { }

// Commands
public ICommand AddProductCommand { get; }

// Properties (PascalCase)
public string ProductName { get; set; }

// Private fields (_camelCase)
private string _productName;
```

### 2. MVVM Pattern
```
View (XAML) 
  ↕️ DataBinding
ViewModel (Logic + Commands)
  ↕️ 
Model (Data) + Services (Data Access)
```

### 3. File Organization
```
📁 Views/
  └─ [Feature]View.xaml + .cs
📁 ViewModels/
  └─ [Feature]ViewModel.cs
📁 Models/
  └─ [Entity].cs
```

---

## 🔧 Công nghệ sử dụng

- **Framework:** .NET 9.0 (WPF)
- **UI Library:** MaterialDesignInXaml
- **Database:** SQLite
- **ORM:** Entity Framework Core 9.0.10
- **Pattern:** MVVM

---

## 📦 Packages cần thiết

```xml
<PackageReference Include="MaterialDesignThemes" Version="5.x.x" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.10" />
```

---

## 🚦 Bước tiếp theo để hoàn thiện

### Priority 1: Core Features
1. ✅ Tách riêng Views thành UserControl
2. ✅ Tạo DashboardViewModel và ReportsViewModel
3. ✅ Cập nhật MainWindow với navigation mới
4. [ ] Test và fix bugs
5. [ ] Implement Excel export cho Reports

### Priority 2: Enhancement
1. [ ] Thêm validation rules đầy đủ
2. [ ] Implement error handling toàn diện
3. [ ] Thêm confirmation dialogs
4. [ ] Loading indicators
5. [ ] Search với debounce

### Priority 3: Advanced Features
1. [ ] User management (thêm, sửa, xóa users)
2. [ ] Role-based access control
3. [ ] Audit logs (lịch sử thao tác)
4. [ ] Backup/Restore database
5. [ ] Multi-language support

### Priority 4: Professional Touch
1. [ ] Unit tests
2. [ ] Documentation đầy đủ
3. [ ] Installer/Setup
4. [ ] User manual
5. [ ] Deployment guide

---

## 💡 Tips phát triển

### 1. Làm việc với từng module riêng biệt
Mỗi View/ViewModel là độc lập, bạn có thể:
- Chỉ focus vào 1 file View + ViewModel
- Test riêng từng chức năng
- Không ảnh hưởng các module khác

### 2. Hot Reload trong development
```bash
# Build và chạy
dotnet build
dotnet run
```

### 3. Database Migrations
```bash
# Tạo migration mới
dotnet ef migrations add [MigrationName]

# Cập nhật database
dotnet ef database update

# Rollback
dotnet ef database update [PreviousMigration]
```

### 4. Debug tips
- Sử dụng breakpoints trong ViewModels
- Check Binding errors trong Output window
- Dùng Snoop hoặc WPF Inspector để debug UI

---

## 📞 Hỗ trợ

Nếu gặp vấn đề:
1. Check error trong Output window (View > Output)
2. Xem Exceptions trong Debug console
3. Verify database connection string
4. Ensure Material Design resources are loaded

---

## 🎓 Học thêm

- **MVVM Pattern:** https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/
- **Material Design:** https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
- **EF Core:** https://learn.microsoft.com/en-us/ef/core/

---

**Chúc bạn phát triển thành công! 🚀**
