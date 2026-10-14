using Microsoft.AspNetCore.Mvc;
using LibraryApi.Domain.Entities;
using LibraryApi.Services.Dtos;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController :ControllerBase
{
    private static readonly  List<Author> _authors= new();
    private static int _nextId=1;
    
    public ActionResult<List<AuthorDto>>GetAll()
    {
        var result =_authors
        .Select(a=> new AuthorDto(a.Id, a.FullName,a.BirthYear))
        .ToList();
        return Ok(result);
    }


    [HttpGet("{id}")]
    public ActionResult<AuthorDto> GetById(int id)
    {
        var author=_authors.FirstOrDefault(a=>a.Id==id);
        if(author is null ) return NotFound();
        return Ok(new AuthorDto(author.Id,author.FullName,author.BirthYear));
    }

    [HttpPost]
    public ActionResult<AuthorDto> Create(CreateAuthorDto dto)
    {
         var author = new Author
         {
             Id=_nextId++,
             FullName=dto.FullName,
             BirthYear=dto.BirthYear

         };
         _authors.Add(author);

         var result = new AuthorDto(author.Id,author.FullName,author.BirthYear);
         return CreatedAtAction(nameof(GetById), new {id=author.Id}, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateAuthordDto dto)
    {
        var author =_authors.FirstOrDefault(a=>a.Id==id);
        if(author is null ) return NotFound();

        author.FullName= dto.FullName;
        author.BirthYear= dto.BirthYear;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var author =_authors.FirstOrDefault(a=>a.Id==id);
        if(author is null) return NotFound();

        _authors.Remove(author);
        return NoContent();
    }


}
