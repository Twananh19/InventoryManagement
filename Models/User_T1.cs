using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model đại diện cho người dùng trong hệ thống
    /// THÀNH VIÊN 1: User Management Module
    /// TODO: Implement user CRUD, password change, activity tracking
    /// </summary>
    public class User_T1
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        // Role relationship
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role_T1 Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        [MaxLength(50)]
        public string? CreatedBy { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<AuditLog_T1> AuditLogs { get; set; } = new List<AuditLog_T1>();
    }
}
