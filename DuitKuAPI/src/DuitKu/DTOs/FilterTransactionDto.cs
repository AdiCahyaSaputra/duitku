namespace DuitKu.DTOs
{
    public record FilterTransactionDto : BaseParamFilterDto
    {
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}