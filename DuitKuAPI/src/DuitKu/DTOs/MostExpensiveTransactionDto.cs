using DuitKu.Domain;

namespace DuitKu.DTOs
{
    public record MostExpensiveTransactionDto
    {
        public Guid Id {get; set;}
        public Account Account {get; set;} = new Account();
        public decimal Amount {get; set;}
        public string Description {get; set;} = default!;
        public DateTime Date {get; set;}
    }
}