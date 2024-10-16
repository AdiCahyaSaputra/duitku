namespace DuitKu.DTOs
{
    public record AccountTotalIncomeFilterDto: BaseParamFilterDto
    {
        public Guid? AccountId {get; set;}
    }
}