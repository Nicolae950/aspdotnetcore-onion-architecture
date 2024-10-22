using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(Guid id);
    Task<T> GetByIdAsync(Guid? id);
    Task<T> CreateAsync(T entity);
    Task SaveAsync();
}
