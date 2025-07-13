using System.Linq.Expressions;
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
    public async Task<(List<T> result, int totalPages)> SearchAsync(
        Dictionary<string, dynamic> parameters,
        int pageNo = 1,
        int pageSize = 10)
    {
        var query = _dbSet.AsQueryable();

        // Apply filters from parameters
        foreach (var parameter in parameters)
        {
            var propertyName = parameter.Key;
            var value = parameter.Value;

            // Create a parameter expression
            var parameterExpression = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameterExpression, propertyName);
            var constant = Expression.Constant(value);
        
            // Create equality comparison
            var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameterExpression);

            // Get the correct Where method
            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T));

            // Apply the filter using Expression.Call
            query = (IQueryable<T>)whereMethod.Invoke(null, new object[] { query, lambda });
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Apply pagination
        var result = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (result, totalPages);
    }

}