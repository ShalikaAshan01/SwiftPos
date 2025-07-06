using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using PointOfSales.Utils;
using Microsoft.Extensions.DependencyInjection;
using Location = PointOfSales.Core.Entities.Infrastructure.Location; // <-- Important for GetService<T>()

namespace PointOfSales.Views.Shared
{
    public partial class ToolbarStatusWidget : UserControl
    {
        private readonly ConnectivityHelper _connectivityHelper;

        // Controls references (Ellipse, TextBlock) - either via x:Name or FindControl in constructor
        private readonly Ellipse? _connectivityEllipse = null!;
        private readonly TextBlock? _connectivityTextBlock = null!;
        private readonly TextBlock? _locationCodeBlock = null!;

        public ToolbarStatusWidget()
        {
            InitializeComponent();

            _connectivityHelper = App.ServiceProvider.GetService<ConnectivityHelper>()
                                  ?? throw new InvalidOperationException("ConnectivityHelper not registered in DI.");

            _connectivityEllipse = this.FindControl<Ellipse>("ConnectivityEllipse");
            _connectivityTextBlock = this.FindControl<TextBlock>("ConnectivityTextBlock");
            _locationCodeBlock = this.FindControl<TextBlock>("LocationCodeBlock");

            // Initial UI state
            UpdateConnectivityUi(_connectivityHelper.IsConnected);

            // Subscribe to connectivity changes
            _connectivityHelper.ConnectivityChanged += (s, isConnected) =>
            {
                // UI updates must happen on UI thread
                Dispatcher.UIThread.Post(() => UpdateConnectivityUi(isConnected));
            };
            GlobalAuthenticator.OnChangedLocation += GlobalAuthenticator_OnLocationChanged;
        }

        private void UpdateConnectivityUi(bool isConnected)
        {
            if (_connectivityEllipse != null) _connectivityEllipse.Fill = isConnected ? Brushes.LimeGreen : Brushes.Red;
            if (_connectivityTextBlock != null)
                _connectivityTextBlock.Text = isConnected
                    ? Globalization.Resources.Translations.ConnectedLabel
                    : Globalization.Resources.Translations.DisconnectedLabel;
        }

        private void GlobalAuthenticator_OnLocationChanged(Location location)
        {
            Dispatcher.UIThread.Post(() => UpdateLocationCode(location.LocationCode));
        }

        private void UpdateLocationCode(string newCode)
        {
            if (_locationCodeBlock != null)
                _locationCodeBlock.Text = newCode;
        }

        ~ToolbarStatusWidget()
        {
            GlobalAuthenticator.OnChangedLocation -= GlobalAuthenticator_OnLocationChanged;
        }
    }
}