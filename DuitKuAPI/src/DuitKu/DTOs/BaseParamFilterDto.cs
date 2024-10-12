namespace DuitKu.DTOs
{
    public record BaseParamFilterDto
    {
        public bool paginate { get; set; }
        public int? pageNumber { get; set; }
        public int? limit { get; set; }
        public string? search { get; set; }
    }
}