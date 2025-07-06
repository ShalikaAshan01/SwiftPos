using System;
using PointOfSales.Core.Utils;

namespace PointOfSales.Utils
{
    public class ConnectivityHelper
    {
        private readonly ILocalConnectivity _localConnectivity;

        // Event to notify subscribers about connectivity changes
        public event EventHandler<bool>? ConnectivityChanged;

        public ConnectivityHelper(ILocalConnectivity localConnectivity)
        {
            _localConnectivity = localConnectivity;

            // Example: subscribe to localConnectivity event or poll status
            _localConnectivity.StatusChanged += OnLocalConnectivityChanged;
        }

        private void OnLocalConnectivityChanged(object? sender, bool isConnected)
        {
            // Raise event to subscribers
            ConnectivityChanged?.Invoke(this, isConnected);
        }

        // Helper method to get current connectivity state
        public bool IsConnected => _localConnectivity.IsConnected;
    }
}