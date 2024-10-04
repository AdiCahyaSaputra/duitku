using System.ComponentModel.DataAnnotations;

namespace DuitKu.DTOs
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Nama harus di isi")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email harus di isi")]
        [EmailAddress(ErrorMessage = "Email nya nggak valid")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password harus di isi")]
        [MinLength(8, ErrorMessage = "Password minimal 8 karakter atau lebih")]
        public string Password { get; set; } = default!;
    }
}