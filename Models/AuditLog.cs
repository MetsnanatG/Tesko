using System;

namespace Tesko.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } // "Request", "Approve", "Reject", "AddStock"
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
