using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model cho điều chỉnh tồn kho (kiểm kê, hư hỏng, mất mát)
    /// THÀNH VIÊN 2: Stock Adjustment Module
    /// TODO: Implement stock adjustment operations
    /// TODO: Lý do điều chỉnh: Kiểm kê, Hư hỏng, Mất mát, Trả hàng, Khác
    /// TODO: Auto-update Inventory quantity sau khi adjustment
    /// </summary>
    public class StockAdjustment_T2
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string AdjustmentType { get; set; } = string.Empty; // IN (tăng), OUT (giảm)

        [Required]
        [MaxLength(100)]
        public string Reason { get; set; } = string.Empty; 
        // Kiểm kê (Stock Take), Hư hỏng (Damaged), Mất mát (Lost), 
        // Trả hàng (Return), Hết hạn (Expired), Khác (Other)

        public int QuantityBefore { get; set; } // Số lượng trước khi điều chỉnh
        public int AdjustmentQuantity { get; set; } // Số lượng điều chỉnh (+/-)
        public int QuantityAfter { get; set; } // Số lượng sau khi điều chỉnh

        [MaxLength(500)]
        public string? Notes { get; set; }

        [MaxLength(50)]
        public string? ReferenceNumber { get; set; } // Số chứng từ

        [MaxLength(50)]
        public string? ApprovedBy { get; set; } // Người duyệt

        public DateTime AdjustmentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? CreatedBy { get; set; }
    }
}
