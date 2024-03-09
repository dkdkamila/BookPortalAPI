using System.Reflection.Emit;
using BookPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookPortalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet för varje modell
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BookImage> BookImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
       .HasOne(r => r.Book)
       .WithMany(b => b.Reviews)
       .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Username)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Book>()
        .HasMany(b => b.Images) // En bok kan ha många bilder
        .WithOne(i => i.Book)   // Varje bild tillhör en bok
        .HasForeignKey(i => i.BookId); // Foreign key för relationen

        }


    }
}