namespace WebApp.DTOs
{
    public class FilterDTO
    {
        public string? OrderBy { get; set; }
        public string? OrderByDescending { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 4;
    }
}
