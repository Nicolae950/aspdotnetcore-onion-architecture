using Domain.Exceptions;
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

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
{
    private readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public BaseRepository(ApplicationDbContext context)
    {
        if (_context == null)
        { 
            _context = context;
            _dbSet = _context.Set<T>();
        }
    }

    public async Task<IEnumerable<T>> GetAll(Guid id)
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T> Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T> GetById(Guid? id)
    {
        var trackedEntity = await _dbSet.FindAsync(id);
        if (trackedEntity == null)
            throw new NotFoundException<T>();
        return trackedEntity;
    }

    public async Task Save()
    {
        int rowsChanged = await _context.SaveChangesAsync();
        if (rowsChanged == 0)
            throw new SaveException();
    }
}

