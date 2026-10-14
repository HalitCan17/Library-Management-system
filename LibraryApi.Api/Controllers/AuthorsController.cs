using Microsoft.AspNetCore.Mvc;
using LibraryApi.Domain.Entities;
using LibraryApi.Services.Dtos;
using LibraryApi.Data;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController :ControllerBase
{
    
    [HttpGet]
    public ActionResult<List<AuthorDto>>GetAll()
    {
        var result = InMemorStore.Authors
        .Select(a=> new AuthorDto(a.Id, a.FullName,a.BirthYear))
        .ToList();
        return Ok(result);
    }


    [HttpGet("{id}")]
    public ActionResult<AuthorDto> GetById(int id)
    {
        var author=InMemorStore.Authors.FirstOrDefault(a=>a.Id==id);
        if(author is null ) return NotFound();
        return Ok(new AuthorDto(author.Id,author.FullName,author.BirthYear));
    }

    [HttpPost]
    public ActionResult<AuthorDto> Create(CreateAuthorDto dto)
    {
         var author = new Author
         {
             Id=InMemorStore.NextAuthorId(),
             FullName=dto.FullName,
             BirthYear=dto.BirthYear

         };
         InMemorStore.Authors.Add(author);

         var result = new AuthorDto(author.Id,author.FullName,author.BirthYear);
         return CreatedAtAction(nameof(GetById), new {id=author.Id}, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateAuthordDto dto)
    {
        var author =InMemorStore.Authors.FirstOrDefault(a=>a.Id==id);
        if(author is null ) return NotFound();

        author.FullName= dto.FullName;
        author.BirthYear= dto.BirthYear;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var author =InMemorStore.Authors.FirstOrDefault(a=>a.Id==id);
        if(author is null) return NotFound();

        if(InMemorStore.Books.Any(b=>b.AuthorId==id))
        return Conflict("Bu yazarın kitapları var önce kitapları silin");

        InMemorStore.Authors.Remove(author);
        return NoContent();
    }


}
