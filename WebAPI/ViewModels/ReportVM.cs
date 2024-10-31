using Domain.Entities;

namespace WebAPI.ViewModels
{
    public class ReportVM
    {
        public Guid Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        
        public ReportVM(Report report)
        {
            Id = report.Id;
            From = report.From;
            To = report.To;
        }
    }
}
