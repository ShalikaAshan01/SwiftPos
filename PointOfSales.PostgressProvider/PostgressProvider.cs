using PointOfSales.Core.Plugin;

namespace PointOfSales.PostgressProvider
{
    public class PostgressProvider : IPluggable
    {
        public PluginInfo PluginInfo => new PluginInfo { Version = "0.1.0" , Name = "Database"};
    }
}
