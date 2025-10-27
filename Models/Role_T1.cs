using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    /// <summary>
    /// Model đại diện cho vai trò trong hệ thống
    /// THÀNH VIÊN 1: User Management & Settings Module
    /// TODO: Implement CRUD operations trong RoleViewModel
    /// </summary>
    public class Role_T1
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty; // Admin, Manager, Staff

        [MaxLength(200)]
        public string? Description { get; set; }

        // Permissions
        public bool CanManageUsers { get; set; } = false;
        public bool CanManageProducts { get; set; } = false;
        public bool CanManageInventory { get; set; } = false;
        public bool CanViewReports { get; set; } = false;
        public bool CanExportData { get; set; } = false;
        public bool CanManageSettings { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<User_T1> Users { get; set; } = new List<User_T1>();
    }
}
