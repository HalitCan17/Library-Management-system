using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using LibraryApi.Services.Dtos;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{

    private static BookDto ToDto(Book b) => new(b.Id, b.Title, b.Isbn, b.PublishYear, b.StockCount, b.AuthorId, b.Author?.FullName ?? string.Empty);

    [HttpGet]
    public ActionResult<PagedResult<BookDto>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? authorId,
        [FromQuery] int page = 1,
        [FromQuery] int PageSize = 10)
    {
        IEnumerable<Book> query = InMemorStore.Books;
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (authorId is not null)
            query = query.Where(b => b.AuthorId == authorId);

        var TotalCount = query.Count();

        var items = query.OrderBy(b => b.Id).Skip((page - 1) * PageSize).Take(PageSize).Select(ToDto).ToList();

        return Ok(new PagedResult<BookDto>(items,page,PageSize,TotalCount));
    }

    [HttpGet("{id}")]
    public ActionResult<BookDto> GetById(int id)
    {
        var book = InMemorStore.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        return Ok(ToDto(book));
    }

    [HttpPost]
    public ActionResult<BookDto> Create(CreateBookDto dto)
    {
        var author = InMemorStore.Authors.FirstOrDefault(a => a.Id == dto.AuthorId);
        if (author is null) return BadRequest("Böyle bir yazar yok");
        if (InMemorStore.Books.Any(b => b.Isbn == dto.Isbn))
            return Conflict("Bu ISBN ile kayıtlı kitap zaten var.");

        var book = new Book
        {
            Id = InMemorStore.NextBookId(),
            Title = dto.Title,
            Isbn = dto.Isbn,
            PublishYear = dto.PublishYear,
            StockCount = dto.StockCount,
            AuthorId = dto.AuthorId,
            Author = author
        };
        InMemorStore.Books.Add(book);

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, ToDto(book));
    }


    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateBookDto dto)
    {
        var book = InMemorStore.Books.FirstOrDefault(a => a.Id == id);
        if (book is null) return NotFound();

        var author = InMemorStore.Authors.FirstOrDefault(b => b.Id == dto.AuthorId);
        if (author is null) return BadRequest("Böyle bir yazar yok.");

        if (InMemorStore.Books.Any(b => b.Isbn == dto.Isbn && b.Id != id))
            return Conflict("Bu ISBN başka bir kitapta kullanılıyor.");

        book.Title = dto.Title;
        book.Isbn = dto.Isbn;
        book.PublishYear = dto.PublishYear;
        book.StockCount = dto.StockCount;
        book.AuthorId = author.Id;
        book.Author = author;
        return NoContent();

    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = InMemorStore.Books.FirstOrDefault(a => a.Id == id);
        if (book is null) return NotFound();

        InMemorStore.Books.Remove(book);
        return NoContent();

    }


}