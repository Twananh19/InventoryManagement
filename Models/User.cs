using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // "Admin" or "User"
    }
}