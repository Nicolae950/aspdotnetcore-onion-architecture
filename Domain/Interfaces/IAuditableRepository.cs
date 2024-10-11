using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuditableRepository<T> : IBaseRepository<T> where T : AuditableEntity
    {
        Task Update(T entity);
        Task ExecuteUpdate(Guid id,
            T entity,
            Expression<Func<T, bool>> predicate,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> property);
    }
}
