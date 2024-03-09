namespace BookPortalAPI.Models;

public class BookImage
{
    public int Id { get; set; }
    public byte[]? ImageData { get; set; } // Byte array to store image data
    public string? ContentType { get; set; } // Content type of the image (e.g., image/jpeg, image/png)
    public int BookId { get; set; } // Foreign key to relate the image to a book
    public Book? Book { get; set; } // Navigation property to the related book
}