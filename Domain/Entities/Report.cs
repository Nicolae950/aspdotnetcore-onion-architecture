using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Report : BaseEntity
{
    public Guid AccountId { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public Report()
    {
        
    }

    public Report(Guid accountId, DateTimeOffset from, DateTimeOffset to)
    {
        Id = new Guid();
        AccountId = accountId;
        From = from;
        To = to;
    }
}
