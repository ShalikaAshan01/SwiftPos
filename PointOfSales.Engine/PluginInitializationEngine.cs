using PointOfSales.Core.Constants;

namespace PointOfSales.Engine
{
    public interface IPluginInitializationEngine
    {
        public Task OnPluginInit();
    }

    public class PluginInitializationEngine : IPluginInitializationEngine
    {
        public Task OnPluginInit()
        {
            return Task.CompletedTask;
        }
    }
}
