using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FullAuditableRepository<T> : AuditableRepository<T>, IFullAuditableRepository<T> where T : FullAuditableEntity
    {
        public FullAuditableRepository(ApplicationDbContext context):
            base(context)
        { }
        public async Task SoftDelete(Guid id, T entity)
        {
            entity.IsDeleted = true;
            await Task.Factory.StartNew(() =>
            {
                _dbSet.Update(entity);
            });
        }

        public async Task HardDelete(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            await Task.Factory.StartNew(() =>
            {
                 _dbSet.Remove(entity);
            });
        }
    }
}
