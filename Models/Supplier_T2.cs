using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model đại diện cho nhà cung cấp
    /// THÀNH VIÊN 2: Supplier Management Module
    /// TODO: Implement CRUD operations cho Supplier
    /// TODO: Liên kết với InboundLog để xem lịch sử nhập hàng từ supplier
    /// TODO: Thống kê tổng giá trị nhập hàng từ mỗi supplier
    /// </summary>
    public class Supplier_T2
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; } = "Vietnam";

        [MaxLength(50)]
        public string? TaxCode { get; set; } // Mã số thuế

        [MaxLength(100)]
        public string? BankAccount { get; set; }

        [MaxLength(100)]
        public string? BankName { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<InboundLog> InboundLogs { get; set; } = new List<InboundLog>();
    }
}
