using System.ComponentModel.DataAnnotations;

namespace Konyvtar_katalogus.Models
{
    public class NotificationQueueItem
    {
        [Key]
        public int notificationQueueItemId { get; set; }
        public int LoanId { get; set; }
        [Required]
        [MaxLength(300)]
        
        public string Message { get; set; }
        [Required]
        [MaxLength(40)]
        
        public string Status { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? SentAt { get; set; }

        public Loan Loan { get; set; }
    }
}
