using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using PointOfSales.Core.Constants;

namespace PointOfSales.Views.Shared;

public partial class CompanyLogo : UserControl
{
    public CompanyLogo()
    {
        InitializeComponent();
        TryLoadCustomLogo();
    }
    
    private void TryLoadCustomLogo()
    {
        var logoPath = Path.Combine(LocalConfigurations.LocalFolderPath, LocalConfigurations.LogoFileName);

        if (File.Exists(logoPath))
        {
            try
            {
                using var stream = File.OpenRead(logoPath);
                var bitmap = new Bitmap(stream);

                var image = new Image
                {
                    Source = bitmap,
                    Stretch = Avalonia.Media.Stretch.Uniform
                };

                RootGrid.Children.Add(image);
                return;
            }
            catch (Exception e)
            {
                Engine.Utils.Common.Logger.LogError(e, e.ToString());
            }
        }

        // Show fallback shared:Logo
        RootGrid.Children.Add(new Logo());
    }
}