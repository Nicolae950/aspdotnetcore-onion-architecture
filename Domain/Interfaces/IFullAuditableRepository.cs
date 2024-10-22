using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFullAuditableRepository<T> : IAuditableRepository<T> where T : FullAuditableEntity
    {
        Task SoftDeleteAsync(Guid id, T entity);
        Task HardDeleteAsync(Guid id);
    }
}
