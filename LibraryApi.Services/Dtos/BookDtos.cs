using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Services.Dtos;

public record BookDto(int Id, string Title, string Isbn, int PublishYear, int StockCount,
int AuthorId, string AuthorName);

public record CreateBookDto(
    [Required(ErrorMessage ="Başlık boş olamaz!")]
    [StringLength(200,ErrorMessage ="Başlık en fazla 200 karakter olabilir!")]
    string Title, 
    
    [Required(ErrorMessage ="Isbn boş olamaz!")]
    [StringLength(20,MinimumLength =3,ErrorMessage ="Isbn 3 ile 20 karakter arasında olmalı!")]
    string Isbn, 
    
    [Range(1000,2100,ErrorMessage ="Yayın yılı 1000 ile 2100 arasında olmalı!")]
    int PublishYear, 
    
    [Range(0,1000,ErrorMessage ="Stok 0 ile 1000 arasında olmalı!")]
    int StockCount,

    [Range(1,int.MaxValue,ErrorMessage ="Geçerli bir yazar numarası girin!")]
    int AuthorId);

public record UpdateBookDto(
    [Required(ErrorMessage ="Başlık boş olamaz!")]
    [StringLength(200,ErrorMessage ="Başlık en fazla 200 karakter olabilir!")]
    string Title, 
    
    [Required(ErrorMessage ="Isbn boş olamaz!")]
    [StringLength(20,MinimumLength =3,ErrorMessage ="Isbn 3 ile 20 karakter arasında olmalı!")]
    string Isbn, 
    
    [Range(1000,2100,ErrorMessage ="Yayın yılı 1000 ile 2100 arasında olmalı!")]
    int PublishYear, 
    
    [Range(0,10000,ErrorMessage ="Stok 0 ile 10000 arasında olmalı!")]
    int StockCount, 
    
    [Range(1,int.MaxValue,ErrorMessage ="Geçerli bir yazar numarası girin!")]
    int AuthorId);