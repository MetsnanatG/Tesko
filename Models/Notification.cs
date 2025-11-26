using System;
using System.ComponentModel.DataAnnotations;

namespace Tesko.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public int? RelatedRequestId { get; set; }
    }
}
