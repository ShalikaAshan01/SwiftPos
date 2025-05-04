using Microsoft.Extensions.DependencyInjection;

namespace PointOfSales.Core.Data;

public interface IDatabaseProvider
{
    public Task OnInitAsync(IServiceCollection collection);
}