using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodManagement.Models
{
    public class InboundLog
    {
        [Key]
        public int LogId { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        public int Quantity { get; set; }
        
        public DateTime Date { get; set; }
        
        public virtual Product Product { get; set; }
    }
}