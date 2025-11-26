using System.ComponentModel.DataAnnotations;

namespace Tesko.Models
{
    public class Asset
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; } // "SIM", "Voucher", "Device"
        public string? Description { get; set; }
        
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int AllocatedStock { get; set; }
        public int DefectiveStock { get; set; }
        
        public int LowStockThreshold { get; set; }
    }
}
