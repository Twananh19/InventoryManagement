using System;
using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model cấu hình hệ thống
    /// THÀNH VIÊN 1: Settings & Configuration Module
    /// TODO: Implement Settings page với các cấu hình sau:
    /// - Company info (tên công ty, logo, địa chỉ)
    /// - Low stock threshold (ngưỡng cảnh báo tồn kho thấp)
    /// - Password policy (độ dài mật khẩu tối thiểu)
    /// - Auto backup settings
    /// - Email/SMS notifications
    /// </summary>
    public class SystemSettings_T1
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? SettingValue { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; } // Company, Security, Notification, Inventory

        [MaxLength(50)]
        public string? DataType { get; set; } // String, Integer, Boolean, Date

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }
    }
}
