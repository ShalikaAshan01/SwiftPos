using PointOfSales.Core.Plugin;

namespace PointOfSales.Engine;

public class BaseEngine
{
    private readonly IEnumerable<IPlugin> _plugins;

    public BaseEngine(IEnumerable<IPlugin> plugins)
    {
        _plugins = plugins;
    }

    public List<T> GetPlugins<T>() where T : class, IPlugin
    {
        return _plugins
            .OfType<T>()
            .ToList();
    }
}