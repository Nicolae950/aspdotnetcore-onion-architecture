﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FullAuditableEntity : AuditableEntity, IFullAuditableEntity, ISoftDeleteEntity
    {
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
