namespace LibraryApi.Domain.Entities
{
        public class Book
    {
        public  int Id {get;set;}
        public string Title{get;set;}
        public string Isbn {get;set;}=string.Empty;
        public int PublishYear{get;set;}
        public int StockCount{get;set;}
        public int AuthorId{get;set;}
        public Author Author{get;set;}
        
    }
}