using System.ComponentModel.Composition;
using PointOfSales.Core.Commands;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.Plugin;

namespace PointOfSales.PostgressProvider.Commands;

[Export(typeof(IPluggable))]
public class LoginCommand: IPlugin,ILoginCommand
{
    public async Task<User> LoginAsync(string username, string password)
    {
        return new User
        {
            UserName = "shrrs",
            Password = string.Empty,
        };
    }

    public PluginInfo PluginInfo  => PostgressProvider.PluginInfo;
}