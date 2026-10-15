using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Services.Dtos;

    public record AuthorDto(int Id,string FullName,int BirthYear);
    public record CreateAuthorDto(
        [Required(ErrorMessage ="Ad boş olamaz!")]
        [StringLength(100,MinimumLength =2, ErrorMessage ="Ad 2 ile 100 karakter arasında olabilir!")]
        string FullName,
        
        [Range(1000,2100,ErrorMessage ="Doğum yılı 1000-2100 arasında olabilir")]
        int BirthYear);
    public record UpdateAuthordDto(
        [Required(ErrorMessage ="Ad boş olamaz!")]
        [StringLength(100,MinimumLength =2,ErrorMessage ="Ad 2 ile 100 karakter arasında olabilir!")]
        string FullName,
        
        [Range(1000,2100,ErrorMessage ="Doğum yılı 1000-2100 arasında olabilir!")]
        int BirthYear);
