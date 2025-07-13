using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class AuditLogRepository(MyDbContext context) : IAuditLogRepository
{
    public async Task WriteToLogAsync(ActivityLog log)
    {
        await context.Set<ActivityLog>().AddAsync(log);
    }

    public async Task<(List<ActivityLog> result, int totalPages)> SearchAsync(
        Dictionary<string, dynamic> parameters,
        int pageNo = 1,
        int pageSize = 10)
    {
        var query = context.Set<ActivityLog>().AsQueryable();

        // Apply filters from parameters
        foreach (var parameter in parameters)
        {
            var propertyName = parameter.Key;
            var value = parameter.Value;

            // Create a parameter expression
            var parameterExpression = Expression.Parameter(typeof(ActivityLog), "x");
            var property = Expression.Property(parameterExpression, propertyName);
            var constant = Expression.Constant(value);

            // Create equality comparison
            var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<ActivityLog, bool>>(equality, parameterExpression);

            // Get the correct Where method
            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(ActivityLog));

            // Apply the filter using Expression.Call
            if (query != null)
                query = (IQueryable<ActivityLog>)whereMethod.Invoke(null, [query, lambda])!;
        }

        // Get total count for pagination
        var totalCount = await query?.CountAsync()!;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Apply pagination
        var result = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (result, totalPages);
    }
}