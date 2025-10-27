using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model đại diện cho khách hàng
    /// THÀNH VIÊN 2: Customer Management Module
    /// TODO: Implement CRUD operations cho Customer
    /// TODO: Liên kết với OutboundLog để xem lịch sử mua hàng
    /// TODO: Thống kê tổng giá trị mua hàng của mỗi customer
    /// TODO: Customer loyalty program (khách hàng thân thiết)
    /// </summary>
    public class Customer_T2
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

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
        public string? CustomerType { get; set; } // Retail, Wholesale, VIP

        [MaxLength(50)]
        public string? TaxCode { get; set; }

        public decimal TotalPurchaseAmount { get; set; } = 0; // Tổng giá trị đã mua

        public int LoyaltyPoints { get; set; } = 0; // Điểm tích lũy

        [MaxLength(500)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<OutboundLog> OutboundLogs { get; set; } = new List<OutboundLog>();
    }
}
