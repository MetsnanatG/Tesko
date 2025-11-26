using System;
using System.ComponentModel.DataAnnotations;

namespace Tesko.Models
{
    public class Request
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
        
        public int AssetId { get; set; }
        public Asset? Asset { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public string Purpose { get; set; }
        
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
        
        public DateTime RequestDate { get; set; } = DateTime.Now;
        
        public int? ApproverId { get; set; }
        public User? Approver { get; set; }
        
        public DateTime? ActionDate { get; set; }
        public string? Comment { get; set; }
    }
}
