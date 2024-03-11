

using System.ComponentModel.DataAnnotations;

namespace BookPortalAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; }


        public ICollection<Review>? Reviews { get; set; }

    }

    public class Review
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? Username { get; set; }
        [Required]
        public string? ReviewText { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
