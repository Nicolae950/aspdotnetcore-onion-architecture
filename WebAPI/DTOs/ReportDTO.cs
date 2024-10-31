using Domain.Entities;

namespace WebAPI.DTOs;

public class ReportDTO
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public Report MapDTOTOReport(Guid accountId)
    {
        return new Report(accountId, From, To);
    }
}
