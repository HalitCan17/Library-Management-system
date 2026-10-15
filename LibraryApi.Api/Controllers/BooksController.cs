using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using LibraryApi.Services.Dtos;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public BooksController(LibraryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BookDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? authorId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;


        IQueryable<Book> query = _db.Books;
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => EF.Functions.ILike(b.Title, $"%{search}%"));

        if (authorId is not null)
            query = query.Where(b => b.AuthorId == authorId);

        var TotalCount = await query.CountAsync();

        var books = await query
           .Include(b => b.Author)
           .OrderBy(b => b.Id)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        var items = books.Select(ToDto).ToList();

        return Ok(new PagedResult<BookDto>(items, page, pageSize, TotalCount));


    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound();

        return Ok(ToDto(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
        if (author is null) return BadRequest("Böyle bir yazar yok");
        if (await _db.Books.AnyAsync(b => b.Isbn == dto.Isbn))
            return Conflict("Bu ISBN ile kayıtlı kitap zaten var.");

        var book = new Book
        {

            Title = dto.Title,
            Isbn = dto.Isbn,
            PublishYear = dto.PublishYear,
            StockCount = dto.StockCount,
            AuthorId = dto.AuthorId,
            Author = author
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, ToDto(book));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBookDto dto)
    {
        var book = await _db.Books.FirstOrDefaultAsync(a => a.Id == id);
        if (book is null) return NotFound();

        var author = await _db.Authors.FirstOrDefaultAsync(b => b.Id == dto.AuthorId);
        if (author is null) return BadRequest("Böyle bir yazar yok.");

        if (await _db.Books.AnyAsync(b => b.Isbn == dto.Isbn && b.Id != id))
            return Conflict("Bu ISBN başka bir kitapta kullanılıyor.");

        book.Title = dto.Title;
        book.Isbn = dto.Isbn;
        book.PublishYear = dto.PublishYear;
        book.StockCount = dto.StockCount;
        book.AuthorId = author.Id;
        book.Author = author;
        await _db.SaveChangesAsync();
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.Books.FirstOrDefaultAsync(a => a.Id == id);
        if (book is null) return NotFound();

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return NoContent();

    }

    private static BookDto ToDto(Book b) =>
       new(b.Id, b.Title, b.Isbn, b.PublishYear, b.StockCount, b.AuthorId, b.Author?.FullName ?? string.Empty);
}