using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.Data;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;
using PointOfSales.Engine;

namespace PointOfSales.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IApplicationLogger, ApplicationLogger>();
            collection.AddSingleton<IPluginInitializationEngine, PluginInitializationEngine>();
            collection.AddSingleton<IIniEngine, IniEngine>();
            // collection.AddSingleton<IPluginLoader, PluginLoader>();
            collection.AddSingleton<IDatabaseProvider, PostgressProvider.PostgressProvider>();
            collection.AddSingleton<StartupHandler>();
        }
    }
}
