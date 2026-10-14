using LibraryApi.Domain.Entities;

namespace LibraryApi.Data;

public static class InMemorStore
{
    public static List<Author> Authors  {get;}=new();
    public static List<Book> Books {get;}=new();

    private static int _nextAuthorId=1;
    private static int _nextBookId=1;

    public static int NextAuthorId()=>_nextAuthorId++;
    public static int NextBookId()=>_nextBookId++;


}