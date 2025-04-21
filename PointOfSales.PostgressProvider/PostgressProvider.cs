using PointOfSales.Core.Plugin;
using PointOfSales.Core.Utils;

namespace PointOfSales.PostgressProvider
{
    public class PostgressProvider : IPluggable
    {
        public PostgressProvider()
        {
            
        }
        public static PluginInfo PluginInfo => new PluginInfo { Version = "0.1.0" , Name = "Database"};
        public Task OnInitAsync()
        {
            // logger.LogInfo("PostgresProvider OnInitAsync");
            return Task.CompletedTask;
        }
    }
}
