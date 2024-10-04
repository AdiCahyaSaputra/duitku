using System.ComponentModel.DataAnnotations;

using DuitKu.Attribute;

namespace DuitKu.DTOs
{
    public record TransactionDto
    {
        [Required(ErrorMessage = "Deskripsi wajib di isi")]
        public string Description { get; set; } = default!;

        [RequiredMin(100.0, ErrorMessage = "Jumlah Uang wajib di isi dan harus lebih dari 100")]
        public decimal Amount { get; set; }

        [RequiredGuid(ErrorMessage = "Akun wajib di isi")]
        public Guid AccountId { get; set; }

        [RequiredGuid(ErrorMessage = "Kategori wajib di isi")]
        public Guid CategoryId { get; set; }

        [MustValidGuidIfExists(ErrorMessage = "Sub kategori tidak ada")]
        public Guid? SubCategoryId { get; set; }

        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}