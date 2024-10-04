using System.ComponentModel.DataAnnotations;

namespace DuitKu.DTOs
{
    public record AccountDto
    {
        [Required(ErrorMessage = "Nama Akun wajib di isi")]
        public string Name { get; set; } = default!;
        public decimal Balance { get; set; } = 0;

        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}