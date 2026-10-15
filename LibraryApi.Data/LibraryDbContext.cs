using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> optinos) : base(optinos) { }

    public DbSet<Author> Authors=> Set<Author>();
    public DbSet<Book> Books=> Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(a=>a.FullName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity=>
        {
            entity.Property(b=>b.Title).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Book>(entity=>
        {
            entity.Property(b=>b.Isbn).IsRequired().HasMaxLength(20);
            entity.HasIndex(b=>b.Isbn).IsUnique();

            entity.HasOne(b=>b.Author)
            .WithMany(a=>a.Books)
            .HasForeignKey(b=>b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}