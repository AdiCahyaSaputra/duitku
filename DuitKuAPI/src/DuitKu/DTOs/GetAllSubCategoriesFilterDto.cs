namespace DuitKu.DTOs
{
    public record GetAllSubCategoriesFilterDto : BaseParamFilterDto
    {
        public Guid CategoryId { get; set; }
    }
}