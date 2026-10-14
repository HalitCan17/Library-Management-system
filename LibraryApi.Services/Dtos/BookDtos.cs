namespace LibraryApi.Services.Dtos;

public record BookDto(int Id, string Title, string Isbn, int PublishYear, int StockCount,
int AuthorId, string AuthorName);

public record CreateBookDto(string Title, string Isbn, int PublishYear, int StockCount,
int AuthorId);

public record UpdateBookDto(string Title, string Isbn, int PublishYear, int StockCount, 
int AuthorId);