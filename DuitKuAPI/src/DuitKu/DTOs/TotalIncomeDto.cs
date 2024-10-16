using DuitKu.Domain;

namespace DuitKu.DTOs
{
    public record TotalIncomeDto
    {
        public decimal TotalAsset { get; set; }
        public IEnumerable<Account> Accounts { get; set; } = [];
    }
}