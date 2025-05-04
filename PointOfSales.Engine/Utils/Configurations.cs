using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine.Utils;

public static class Configurations
{
    public static IIniEngine IniEngine = null!;


    public static int SplashScreenTime => GetValue(nameof(SplashScreenTime), 0);


    private static T GetValue<T>(string key, T defaultValue, string section = "default")
    {
        try
        {
            var value = IniEngine.Read<T>(section, key);
            return value ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}