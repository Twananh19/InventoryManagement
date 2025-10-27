# 🏪 Good Management System - Hệ thống Quản lý Kho hàng

![.NET](https://img.shields.io/badge/.NET-9.0-blue)
![WPF](https://img.shields.io/badge/WPF-Windows-green)
![Material Design](https://img.shields.io/badge/UI-Material%20Design-red)
![SQLite](https://img.shields.io/badge/Database-SQLite-blue)

Hệ thống quản lý kho hàng hiện đại với giao diện đẹp mắt, dễ sử dụng, được xây dựng trên nền tảng WPF với kiến trúc MVVM.

---

## 📸 Screenshots

### Dashboard - Tổng quan
![Dashboard](docs/screenshots/dashboard.png)
*Hiển thị thống kê tổng quan, cảnh báo tồn kho thấp, và quick actions*

### Quản lý Sản phẩm
![Products](docs/screenshots/products.png)
*CRUD sản phẩm với search real-time và form validation*

### Tồn kho
![Inventory](docs/screenshots/inventory.png)
*Theo dõi tồn kho với cảnh báo màu sắc trực quan*

### Nhập/Xuất hàng
![Inbound/Outbound](docs/screenshots/inbound-outbound.png)
*Ghi nhận phiếu nhập/xuất với tự động cập nhật tồn kho*

### Báo cáo & Thống kê
![Reports](docs/screenshots/reports.png)
*Phân tích dữ liệu với báo cáo chi tiết*

---

## ✨ Tính năng chính

### 🔐 Authentication
- Đăng nhập với username/password
- Quản lý phiên làm việc
- Role-based access (Admin/User)

### 📦 Quản lý Sản phẩm
- ➕ Thêm sản phẩm mới
- ✏️ Chỉnh sửa thông tin
- 🗑️ Xóa sản phẩm
- 🔍 Tìm kiếm real-time
- 📋 Hiển thị danh sách với pagination

### 📊 Quản lý Tồn kho
- 👁️ Xem tồn kho real-time
- ⚠️ Cảnh báo tồn thấp (với màu sắc)
- 📈 Thống kê số lượng
- 🔄 Cập nhật số lượng
- 📅 Lịch sử thay đổi

### 📥 Nhập hàng
- ➕ Tạo phiếu nhập mới
- 🏢 Ghi nhận nhà cung cấp
- 🔄 Tự động cập nhật tồn kho
- 📜 Lịch sử nhập hàng
- 📄 In phiếu nhập

### 📤 Xuất hàng
- ➕ Tạo phiếu xuất
- ✅ Kiểm tra tồn kho trước khi xuất
- 👥 Ghi nhận khách hàng
- 🔄 Tự động cập nhật tồn kho
- 📜 Lịch sử xuất hàng

### 📈 Báo cáo & Thống kê
- 📊 Báo cáo nhập/xuất theo thời gian
- 🏆 Top sản phẩm bán chạy
- 💰 Tổng quan giao dịch
- 📉 Phân tích xu hướng
- 📥 Xuất báo cáo Excel

### 🎨 Dashboard
- 📈 Thống kê tổng quan
- ⚡ Quick actions
- ⏰ Hiển thị thời gian real-time
- 🔔 Notifications
- 📊 Widgets động

---

## 🏗️ Kiến trúc

### MVVM Pattern
```
┌─────────────┐
│    View     │ ← XAML UI
│  (UserControl)
└──────┬──────┘
       │ DataBinding
┌──────▼──────┐
│  ViewModel  │ ← Business Logic + Commands
└──────┬──────┘
       │
┌──────▼──────┐
│   Model +   │ ← Data Entities + Data Access
│  Services   │
└─────────────┘
```

### Cấu trúc thư mục
```
GoodManagement/
├── 📁 Models/              # Data entities
│   ├── User.cs
│   ├── Product.cs
│   ├── Inventory.cs
│   ├── InboundLog.cs
│   └── OutboundLog.cs
│
├── 📁 Views/               # UI Components
│   ├── LoginWindow.xaml
│   ├── DashboardView.xaml
│   ├── ProductsView.xaml
│   ├── InventoryView.xaml
│   ├── InboundView.xaml
│   ├── OutboundView.xaml
│   └── ReportsView.xaml
│
├── 📁 ViewModels/          # Business Logic
│   ├── ViewModelBase.cs
│   ├── LoginViewModel.cs
│   ├── MainViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── ProductViewModel.cs
│   ├── InventoryViewModel.cs
│   ├── InboundViewModel.cs
│   ├── OutboundViewModel.cs
│   └── ReportsViewModel.cs
│
├── 📁 Services/            # Data Access
│   └── AppDbContext.cs
│
├── 📁 Helpers/             # Utilities
│   ├── RelayCommand.cs
│   └── PasswordBoxHelper.cs
│
├── 📁 Migrations/          # EF Core Migrations
│   └── ...
│
├── 📁 Resources/           # Images, Icons, etc.
│   └── ...
│
├── App.xaml                # Application entry point
├── MainWindow.xaml         # Main window
└── GoodManagement.csproj   # Project file
```

---

## 🛠️ Công nghệ sử dụng

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 9.0 | Framework |
| WPF | Latest | Desktop UI |
| Material Design | 5.x | UI Library |
| Entity Framework Core | 9.0.10 | ORM |
| SQLite | Latest | Database |
| C# | 12.0 | Programming Language |

---

## 📦 Cài đặt

### Yêu cầu hệ thống
- Windows 10/11
- .NET 9.0 SDK
- Visual Studio 2022 (hoặc VS Code)
- 4GB RAM minimum

### Bước 1: Clone repository
```bash
git clone https://github.com/Twananh19/InventoryManagement.git
cd InventoryManagement/GoodManagement
```

### Bước 2: Restore packages
```bash
dotnet restore
```

### Bước 3: Update database
```bash
dotnet ef database update
```

### Bước 4: Build và chạy
```bash
dotnet build
dotnet run
```

### Bước 5: Đăng nhập
**Default credentials:**
- Username: `admin`
- Password: `admin123`

---

## 🚀 Sử dụng

### Đăng nhập
1. Mở ứng dụng
2. Nhập username và password
3. Click "Đăng nhập"

### Quản lý sản phẩm
1. Click "Quản lý sản phẩm" trên sidebar
2. Click "Thêm sản phẩm" để thêm mới
3. Click "Sửa" hoặc "Xóa" trên từng dòng

### Nhập hàng
1. Click "Nhập hàng"
2. Chọn sản phẩm
3. Nhập số lượng và nhà cung cấp
4. Click "Nhập hàng"

### Xuất hàng
1. Click "Xuất hàng"
2. Chọn sản phẩm (xem số tồn)
3. Nhập số lượng và khách hàng
4. Click "Xuất hàng"

### Xem báo cáo
1. Click "Báo cáo & Thống kê"
2. Chọn khoảng thời gian
3. Click "Tạo báo cáo"
4. Có thể xuất Excel

---

## 🎨 UI/UX Features

### Material Design
- Modern và professional
- Consistent design language
- Beautiful icons và colors

### Color Coding
- 🟢 **Xanh lá:** Nhập hàng, trạng thái tốt
- 🔴 **Đỏ:** Xuất hàng, cảnh báo
- 🔵 **Xanh dương:** Thông tin, sản phẩm
- 🟣 **Tím:** Báo cáo, phân tích
- 🟠 **Cam:** Cảnh báo tồn thấp

### Responsive Design
- Tự động điều chỉnh layout
- Hoạt động tốt trên các độ phân giải
- MinHeight: 600px, MinWidth: 1200px

### Visual Feedback
- Hover effects
- Active states
- Loading indicators
- Toast notifications

---

## 📚 Documentation

- [📖 Development Guide](DEVELOPMENT_GUIDE.md) - Hướng dẫn phát triển chi tiết
- [🔄 MainWindow Migration](MAINWINDOW_MIGRATION_GUIDE.md) - Hướng dẫn migrate UI
- [🎓 API Documentation](docs/API.md) - API reference
- [🐛 Troubleshooting](docs/TROUBLESHOOTING.md) - Giải quyết vấn đề

---

## 🗺️ Roadmap

### Phase 1: Core Features ✅
- [x] Authentication
- [x] CRUD Products
- [x] Inventory management
- [x] Inbound/Outbound operations
- [x] Basic reports

### Phase 2: Enhancement 🚧
- [x] Dashboard với statistics
- [x] Material Design UI
- [x] Separated Views
- [ ] Excel export
- [ ] Advanced search & filters

### Phase 3: Advanced Features 📋
- [ ] User management
- [ ] Role-based permissions
- [ ] Audit logs
- [ ] Multi-warehouse support
- [ ] Barcode/QR scanning

### Phase 4: Professional 🎯
- [ ] Unit tests
- [ ] API integration
- [ ] Cloud backup
- [ ] Mobile app
- [ ] Multi-language

---

## 🤝 Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) first.

### Quy trình contribute
1. Fork repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **Twananh19** - *Initial work* - [GitHub](https://github.com/Twananh19)

---

## 🙏 Acknowledgments

- Material Design In XAML - For beautiful UI components
- Entity Framework Core - For powerful ORM
- SQLite - For lightweight database
- WPF Community - For inspiration and support

---

## 📧 Contact

- GitHub: [@Twananh19](https://github.com/Twananh19)
- Repository: [InventoryManagement](https://github.com/Twananh19/InventoryManagement)

---

## 📊 Project Status

![GitHub last commit](https://img.shields.io/github/last-commit/Twananh19/InventoryManagement)
![GitHub issues](https://img.shields.io/github/issues/Twananh19/InventoryManagement)
![GitHub pull requests](https://img.shields.io/github/issues-pr/Twananh19/InventoryManagement)

**Status:** 🚀 Active Development

---

**Made with ❤️ by Twananh19**
