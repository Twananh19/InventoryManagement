using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model lưu lại tất cả các thao tác trong hệ thống (Audit Trail)
    /// THÀNH VIÊN 1: Audit & Activity Log Module
    /// TODO: Implement auto-logging cho tất cả CRUD operations
    /// </summary>
    public class AuditLog_T1
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User_T1 User { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty; // CREATE, UPDATE, DELETE, LOGIN, LOGOUT

        [Required]
        [MaxLength(50)]
        public string TableName { get; set; } = string.Empty; // Products, Inventory, Users, etc.

        public int? RecordId { get; set; } // ID của bản ghi bị thay đổi

        [MaxLength(2000)]
        public string? OldValue { get; set; } // JSON của giá trị cũ

        [MaxLength(2000)]
        public string? NewValue { get; set; } // JSON của giá trị mới

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
