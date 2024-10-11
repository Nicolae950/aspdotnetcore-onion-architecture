using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class AuditableEntity : BaseEntity, IAuditableEntity
{
    public DateTimeOffset? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
