using System.ComponentModel.DataAnnotations;

namespace Leave.ViewModels
{
    public class LeaveViewModel
    {
        [Key]
        public int LeaveId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Reason { get; set; } = null!;

        [Required]
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? IsApprove { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
    }
}
