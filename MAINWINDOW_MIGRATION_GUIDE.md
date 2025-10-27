# 🎨 Hướng dẫn Sử dụng MainWindow Mới

## ⚠️ Quan trọng

Dự án hiện có **2 files MainWindow.xaml**:

1. **MainWindow.xaml** (cũ) - Sử dụng DataTemplate inline
2. **MainWindow_New.xaml** (mới) - Sử dụng Views riêng biệt

---

## 🔄 Cách chuyển sang MainWindow mới

### Bước 1: Backup file cũ
```bash
# Đổi tên file cũ để backup
Rename MainWindow.xaml to MainWindow_Old.xaml
Rename MainWindow.xaml.cs to MainWindow_Old.xaml.cs
```

### Bước 2: Đổi tên file mới
```bash
Rename MainWindow_New.xaml to MainWindow.xaml
```

### Bước 3: Cập nhật MainWindow.xaml.cs

File `MainWindow.xaml.cs` không cần thay đổi gì, giữ nguyên như hiện tại:

```csharp
using GoodManagement.Models;
using GoodManagement.ViewModels;

namespace GoodManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user);
        }
    }
}
```

### Bước 4: Rebuild project
```bash
dotnet clean
dotnet build
```

---

## ✨ Ưu điểm của MainWindow mới

### 1. **Tách biệt Views**
- Mỗi View là UserControl riêng
- Dễ maintain và debug
- Code cleaner và organized

### 2. **Reusable Components**
- Views có thể tái sử dụng
- Dễ dàng test từng View độc lập

### 3. **Better Performance**
- Chỉ load View khi cần
- Không cần load tất cả DataTemplate

### 4. **Scalability**
- Dễ thêm Views mới
- Không làm file MainWindow phình to

---

## 📋 Cấu trúc mới

```
MainWindow.xaml (280px Sidebar + Main Content)
├─ Sidebar Navigation
│  ├─ Header (Logo + Username)
│  ├─ Menu Items (Dashboard, Products, Inventory, etc.)
│  └─ Bottom Actions (Refresh, Logout)
│
└─ Main Content Area
   ├─ DashboardView (DataContext: DashboardViewModel)
   ├─ ProductsView (DataContext: ProductViewModel)
   ├─ InventoryView (DataContext: InventoryViewModel)
   ├─ InboundView (DataContext: InboundViewModel)
   ├─ OutboundView (DataContext: OutboundViewModel)
   └─ ReportsView (DataContext: ReportsViewModel)
```

---

## 🎨 UI Improvements

### Sidebar (280px wide)
- ✅ Dark background với Material Design
- ✅ Icons cho mỗi menu item
- ✅ Active state highlighting
- ✅ User info ở header
- ✅ Logout button ở bottom

### Main Content
- ✅ Light background (#F5F5F5)
- ✅ Full-width layout
- ✅ Smooth transitions giữa các views

### Window Settings
- ✅ Kích thước: 1400x750 (min: 1200x600)
- ✅ WindowState: Maximized
- ✅ Responsive layout

---

## 🔧 Customization

### Thay đổi màu sắc Sidebar
```xaml
<!-- In MainWindow.xaml -->
<Border Background="{DynamicResource PrimaryHueDarkBrush}">
    <!-- Hoặc dùng màu cố định -->
    <Border Background="#263238">
```

### Thay đổi kích thước Sidebar
```xaml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="280"/> <!-- Thay đổi số này -->
    <ColumnDefinition Width="*"/>
</Grid.ColumnDefinitions>
```

### Thêm menu item mới
```xaml
<Button Command="{Binding NavigateToSettingsCommand}" 
       Height="55" 
       Margin="15,5"
       HorizontalContentAlignment="Left" 
       Padding="20,0"
       Foreground="White"
       BorderThickness="0">
    <StackPanel Orientation="Horizontal">
        <materialDesign:PackIcon Kind="Settings" 
                                Width="24" Height="24"
                                VerticalAlignment="Center"
                                Margin="0,0,15,0"/>
        <TextBlock Text="Cài đặt" 
                  FontSize="15"
                  VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

---

## 🐛 Troubleshooting

### Lỗi: "The name 'InitializeComponent' does not exist"
**Giải pháp:** Build lại project
```bash
dotnet clean
dotnet build
```

### Lỗi: Views không hiển thị
**Kiểm tra:**
1. Namespace trong Views có đúng không
2. DataContext binding có đúng không
3. ViewModels có được initialize không

### Lỗi: Material Design icons không hiển thị
**Kiểm tra:** App.xaml có import Material Design resources không
```xaml
<ResourceDictionary.MergedDictionaries>
    <materialDesign:BundledTheme BaseTheme="Light" 
                                PrimaryColor="Blue" 
                                SecondaryColor="Lime" />
    <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
</ResourceDictionary.MergedDictionaries>
```

---

## 📝 Next Steps

1. **Test tất cả Views** - Click qua từng menu item
2. **Check data binding** - Verify data hiển thị đúng
3. **Test các commands** - CRUD operations
4. **UI polish** - Điều chỉnh spacing, colors nếu cần

---

## 💡 Tips

- Sử dụng **Ctrl + Click** trên View name để jump to definition
- Dùng **Live Visual Tree** trong VS để debug UI hierarchy
- Test responsive bằng cách resize window

---

**Happy coding! 🚀**
