using DuitKu.Domain;

namespace DuitKu.DTOs
{
    public record AccountWithTransactionsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Balance { get; set; } = default!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}