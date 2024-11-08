namespace WebApp.Models.Report
{
    public class ReportViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }
}
