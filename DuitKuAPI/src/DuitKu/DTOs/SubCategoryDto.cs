using System.ComponentModel.DataAnnotations;

using DuitKu.Attribute;

namespace DuitKu.DTOs
{
    public record SubCategoryDto
    {
        [Required(ErrorMessage = "Nama Sub Kategori wajib di isi")]
        public string Name { get; set; } = default!;

        [RequiredGuidAttribute(ErrorMessage = "Kategori wajib di isi")]
        public Guid CategoryId { get; set; }

        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}