using PointOfSales.Core.IEngines;

namespace PointOfSales.Engine.Utils;

public static class Configurations
{
    public static IIniEngine IniEngine = null!;
    public static int AdminUserId { get; set; }


    public static int SplashScreenTime => GetValue(nameof(SplashScreenTime), 0);
    public static bool? IsServer => GetValue<bool?>(nameof(IsServer), null);
    public static bool? IsLocationServer => GetValue<bool?>(nameof(IsLocationServer), null);
    public static bool EnablePos => GetValue<bool>(nameof(EnablePos), false);
    public static bool VirtualKeyboardEnabled => GetValue<bool>(nameof(VirtualKeyboardEnabled), true);
    public static bool EnableBackOffice => GetValue<bool>(nameof(EnableBackOffice), false);
    public static bool? AutoAssignNewPermissionToAdmin => GetValue<bool?>(nameof(AutoAssignNewPermissionToAdmin), null);
    public static string StoreName => GetNotNullValue(nameof(StoreName), string.Empty);
    public static string StoreAddress => GetNotNullValue(nameof(StoreAddress), string.Empty);
    public static string StoreAddress2 => GetNotNullValue(nameof(StoreAddress2), string.Empty);
    public static string StoreAddress3 => GetNotNullValue(nameof(StoreAddress3), string.Empty);
    public static short StoreId => GetNotNullValue(nameof(StoreId), short.MinValue);
    public static short MachineId => GetNotNullValue(nameof(MachineId), short.MinValue);

    private static T GetNotNullValue<T>(string key, T defaultValue, string section = "default")
    {
        return GetValue(key, defaultValue, section) ?? defaultValue;
    }

    private static T? GetValue<T>(string key, T? defaultValue, string section = "default")
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