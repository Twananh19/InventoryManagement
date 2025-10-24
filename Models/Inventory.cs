using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodManagement.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        public int StockQuantity { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public virtual Product Product { get; set; }
    }
}