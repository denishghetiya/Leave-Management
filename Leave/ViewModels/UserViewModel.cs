using System.ComponentModel.DataAnnotations;

namespace Leave.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
