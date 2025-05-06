using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PointOfSales.Core.Constants;

namespace PointOfSales.Views.Shared;

public partial class PoweredBy : UserControl
{
    public PoweredBy()
    {
        InitializeComponent();
        LocalConfigurations.PowerdBy??= $"© {DateTime.UtcNow.Year} {LocalConfigurations.ApplicationName} | Powered by {LocalConfigurations.OrganizationName}";
        PoweredByTxt.Text = LocalConfigurations.PowerdBy;
    }
}