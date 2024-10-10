using DuitKu.Domain;

namespace DuitKu.DTOs
{
    public record TransactionsWithRelation
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = default!;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public Account? Account  { get; set; } = null;

        public Category? Category { get; set; } = null;

        public SubCategory? SubCategory { get; set; } = null;
    }
}