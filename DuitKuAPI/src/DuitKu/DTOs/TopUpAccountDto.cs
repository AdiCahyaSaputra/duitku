using System.ComponentModel.DataAnnotations;

namespace DuitKu.DTOs
{
    public record TopUpAccountDto
    {
        [Required(ErrorMessage = "Jumlah Saldo wajib di isi")]
        public decimal Balance {get; set;}
    }
}