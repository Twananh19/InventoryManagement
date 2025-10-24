using System.ComponentModel.DataAnnotations;

namespace GoodManagement.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }
        
        [MaxLength(50)]
        public string Packaging { get; set; }
        
        [MaxLength(20)]
        public string UnitOfMeasurement { get; set; }
        
        public decimal Price { get; set; }
    }
}