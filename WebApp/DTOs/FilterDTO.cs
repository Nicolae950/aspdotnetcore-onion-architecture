namespace WebApp.DTOs
{
    public class FilterDTO
    {
        public string? OrderCol { get; set; }
        public string? OrderDir { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 4;
    }
}
