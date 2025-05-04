using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(MyDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> SaveAsync(T entity)
    {
        var entry = await _dbSet.AddAsync(entity);
        return entry.Entity;
    }

    public virtual async Task<List<T>> SaveListAsync(List<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities;
    }

    public virtual async Task<T?> GetByIdAsync<TKey>(TKey id) where TKey : struct
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }
}