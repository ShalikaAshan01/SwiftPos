namespace PointOfSales.Core.Constants
{
    public static class LocalConfigurations
    {
        public const string ApplicationName = "SwiftPos";
        public static readonly string LocalFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationName);
        public static readonly string IniFile = ".ini";

        public static int SplashScreenWidth = 600;
        public static int SplashScreenHeight = 800;
        public static int MainScreenWidth = 1024;
        public static int MainScreenHeight = 768;
    }
}
