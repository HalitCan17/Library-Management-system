namespace LibraryApi.Services.Dtos;

    public record AuthorDto(int Id,string FullName,int BirthYear);
    public record CreateAuthorDto(string FullName,int BirthYear);
    public record UpdateAuthordDto(string FullName,int BirthYear);
