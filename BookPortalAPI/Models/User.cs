
using System.ComponentModel.DataAnnotations;

namespace BookPortalAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        // Samling av recensioner för användaren
        public ICollection<Review>? Reviews { get; set; }
    }
}
