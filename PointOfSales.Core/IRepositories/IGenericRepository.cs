using PointOfSales.Core.Entities;

namespace PointOfSales.Core.IRepositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> SaveAsync(T entity);
    Task<List<T>> SaveListAsync(List<T> entities);
    Task<T?> GetByIdAsync<TKey>(TKey id) where TKey: struct;
    Task<List<T>> GetAllAsync();
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
}