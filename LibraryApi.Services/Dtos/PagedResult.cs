namespace LibraryApi.Services.Dtos;

public record PagedResult<T>(List<T> Item, int Page,int PageSize,int TotalCount)
{
    public int TotalPages=> (int)Math.Ceiling(TotalCount/(double)PageSize);
}