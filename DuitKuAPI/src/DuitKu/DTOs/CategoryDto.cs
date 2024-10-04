using System.ComponentModel.DataAnnotations;

namespace DuitKu.DTOs
{
    public record CategoryDto
    {
        [Required(ErrorMessage = "Nama Kategori wajib di isi")]
        public string Name { get; set; } = default!;

        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}