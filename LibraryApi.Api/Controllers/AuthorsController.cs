using Microsoft.AspNetCore.Mvc;
using LibraryApi.Domain.Entities;
using LibraryApi.Services.Dtos;
using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public AuthorsController(LibraryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAll()
    {
        var result = await _db.Authors
        .Select(a => new AuthorDto(a.Id, a.FullName, a.BirthYear))
        .ToListAsync();
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();
        return Ok(new AuthorDto(author.Id, author.FullName, author.BirthYear));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto dto)
    {
        var author = new Author
        {
            
            FullName = dto.FullName,
            BirthYear = dto.BirthYear

        };
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        var result = new AuthorDto(author.Id, author.FullName, author.BirthYear);
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAuthorDto dto)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();

        author.FullName = dto.FullName;
        author.BirthYear = dto.BirthYear;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author =await  _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return NotFound();

        if ( await _db.Books.AnyAsync(b => b.AuthorId == id))
            return Conflict("Bu yazarın kitapları var önce kitapları silin");

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        
        return NoContent();
    }


}
