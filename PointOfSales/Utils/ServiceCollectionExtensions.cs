using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Engine;

namespace PointOfSales.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IApplicationLogger, ApplicationLogger>();
            collection.AddSingleton<IPluginInitializationEngine, PluginInitializationEngine>();
        }
    }
}
