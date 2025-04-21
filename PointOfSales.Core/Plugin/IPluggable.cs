namespace PointOfSales.Core.Plugin
{
    public interface IPluggable
    {
        public static PluginInfo PluginInfo { get; } = null!;
        public Task OnInitAsync();
    }
}
