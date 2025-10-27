# 📦 Packages Installation Guide

## Required Packages for All Team Members

Chạy các lệnh sau trong **Package Manager Console** (Tools > NuGet Package Manager > Package Manager Console):

### 🔧 Core Packages (Đã có)
```powershell
# Entity Framework Core
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Sqlite
Install-Package Microsoft.EntityFrameworkCore.Tools

# Material Design
Install-Package MaterialDesignThemes
Install-Package MaterialDesignColors

# Password Hashing
Install-Package BCrypt.Net-Next
```

---

## 👤 THÀNH VIÊN 1: User Management Module

```powershell
# JSON Serialization for Audit Logs
Install-Package Newtonsoft.Json

# Already included:
# - BCrypt.Net-Next (for password hashing)
```

### Verify Installation:
```csharp
using Newtonsoft.Json;
using BCrypt.Net;
```

---

## 👤 THÀNH VIÊN 2: Suppliers & Customers Module

```powershell
# Excel Import/Export
Install-Package EPPlus -Version 7.0.0
```

### EPPlus License Configuration:
Thêm vào constructor của ViewModel hoặc App.xaml.cs:
```csharp
using OfficeOpenXml;

// Set license context (for non-commercial use)
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

### Verify Installation:
```csharp
using OfficeOpenXml;
using OfficeOpenXml.Style;
```

---

## 👤 THÀNH VIÊN 3: Reports & Export Module

```powershell
# Excel Export
Install-Package EPPlus -Version 7.0.0

# PDF Export
Install-Package iTextSharp.LGPLv2.Core -Version 3.4.0

# Charts & Graphs
Install-Package LiveChartsCore.SkiaSharpView.WPF -Version 2.0.0-rc2

# JSON (if not already installed)
Install-Package Newtonsoft.Json
```

### LiveCharts Configuration:
Thêm vào App.xaml:
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- Existing resources -->
            <ResourceDictionary Source="pack://application:,,,/LiveChartsCore.SkiaSharpView.WPF;component/Themes/Generic.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Verify Installation:
```csharp
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
```

---

## 🔍 Check Installed Packages

Xem danh sách packages đã cài:
```powershell
Get-Package
```

Hoặc check trong file `GoodManagement.csproj`:
```xml
<ItemGroup>
  <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
  <PackageReference Include="EPPlus" Version="7.0.0" />
  <PackageReference Include="iTextSharp.LGPLv2.Core" Version="3.4.0" />
  <PackageReference Include="LiveChartsCore.SkiaSharpView.WPF" Version="2.0.0-rc2" />
  <PackageReference Include="MaterialDesignColors" Version="3.1.0" />
  <PackageReference Include="MaterialDesignThemes" Version="5.1.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

---

## 🐛 Troubleshooting

### Error: Package not found
```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Error: Version conflict
```powershell
# Update all packages
Update-Package

# Or update specific package
Update-Package EPPlus
```

### Error: License issue with EPPlus
**Solution**: Thêm license context vào code:
```csharp
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

### Error: LiveCharts not rendering
**Solution**: 
1. Clean and rebuild solution
2. Make sure Generic.xaml is referenced in App.xaml
3. Check if SkiaSharp dependencies are installed

---

## 📝 Usage Examples

### EPPlus (Excel Export)
```csharp
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

using (var package = new ExcelPackage())
{
    var worksheet = package.Workbook.Worksheets.Add("Products");
    worksheet.Cells["A1"].Value = "Product Name";
    worksheet.Cells["B1"].Value = "Price";
    
    // Add data...
    
    var file = new FileInfo("Products.xlsx");
    package.SaveAs(file);
}
```

### iTextSharp (PDF Export)
```csharp
using iTextSharp.text;
using iTextSharp.text.pdf;

var document = new Document(PageSize.A4);
var writer = PdfWriter.GetInstance(document, new FileStream("Report.pdf", FileMode.Create));

document.Open();
document.Add(new Paragraph("Sales Report"));
document.Close();
```

### LiveCharts (Charts)
```xaml
<lvc:CartesianChart Series="{Binding Series}">
    <lvc:CartesianChart.XAxes>
        <lvc:Axis Labels="{Binding Labels}"/>
    </lvc:CartesianChart.XAxes>
</lvc:CartesianChart>
```

```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

public ISeries[] Series { get; set; } = new ISeries[]
{
    new LineSeries<double>
    {
        Values = new double[] { 2, 5, 4, 7, 3, 8, 6 },
        Fill = null
    }
};
```

---

## ✅ Checklist

Sau khi cài đặt packages, verify:

### Thành viên 1:
- [ ] BCrypt.Net-Next imported thành công
- [ ] Newtonsoft.Json imported thành công
- [ ] Có thể hash password
- [ ] Có thể serialize/deserialize JSON

### Thành viên 2:
- [ ] EPPlus imported thành công
- [ ] License context được set
- [ ] Có thể tạo Excel file
- [ ] Có thể đọc Excel file

### Thành viên 3:
- [ ] EPPlus imported thành công
- [ ] iTextSharp imported thành công
- [ ] LiveChartsCore imported thành công
- [ ] Có thể tạo Excel file
- [ ] Có thể tạo PDF file
- [ ] Có thể render charts

---

## 🔗 Official Documentation

- **EPPlus**: https://github.com/EPPlusSoftware/EPPlus
- **iTextSharp**: https://github.com/VahidN/iTextSharp.LGPLv2.Core
- **LiveCharts**: https://livecharts.dev/
- **Newtonsoft.Json**: https://www.newtonsoft.com/json/help/html/Introduction.htm
- **BCrypt.Net**: https://github.com/BcryptNet/bcrypt.net

---

## 💡 Tips

1. **Always use latest stable versions** of packages
2. **Check license** before using in commercial projects
3. **Read documentation** before implementing
4. **Test packages** in a separate project first if unsure
5. **Keep packages updated** but test after updating

---

## 🆘 Support

Nếu gặp vấn đề với packages:
1. Check package documentation
2. Search on Stack Overflow
3. Check GitHub Issues
4. Ask team members
5. Contact instructor

**Happy Coding! 🚀**
