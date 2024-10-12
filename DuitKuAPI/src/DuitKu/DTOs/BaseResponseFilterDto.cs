namespace DuitKu.DTOs
{
    public record BaseResponseFilterDto
    {
        public bool isPreviousExists { get; set; }
        public bool isNextExists { get; set; }
    }
}