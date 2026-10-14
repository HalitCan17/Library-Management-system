using System.Collections.Concurrent;

namespace LibraryApi.Domain.Entities;

    public class Author
    {
        public int Id {get;set;}
        public string FullName{get;set;} =string.Empty;
        public int BirthYear{get;set;}
        public List<Book> Books{get;set;} =new();


    }
