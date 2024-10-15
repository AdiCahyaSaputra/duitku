namespace DuitKu.DTOs
{
    public record TransactionExpenseFilterDto
    {
        public Guid? AccountId {get; set;}
        public DateTime? DateStart {get; set;}
        public DateTime? DateEnd {get; set;}
    }
}