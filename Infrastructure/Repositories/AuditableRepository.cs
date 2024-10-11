using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuditableRepository<T> : BaseRepository<T>, IAuditableRepository<T> where T : AuditableEntity
    {
        public AuditableRepository(ApplicationDbContext context):
            base(context)
        { }

        public async Task Update(T entity)
        {
            await Task.Factory.StartNew(() =>
            {
                _dbSet.Update(entity);
            });
        }

        public async Task ExecuteUpdate(Guid id, 
            T entity, 
            Expression<Func<T, bool>> predicateExpression, 
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> propertyExpression)
        {
            await _dbSet
                    .Where(predicateExpression)
                    .ExecuteUpdateAsync(u => u.SetProperty(e => e.LastModifiedBy, id));
            await _dbSet
                    .Where(predicateExpression)
                    .ExecuteUpdateAsync(u => u.SetProperty(e => e.LastModifiedAt, DateTimeOffset.Now));
            await _dbSet
                    .Where(predicateExpression)
                    .ExecuteUpdateAsync(propertyExpression);
        }
    }
}
