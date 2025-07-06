namespace PointOfSales.Core.Utils;

public interface IConnectivity
{
    // Current connectivity state
    bool IsConnected { get; }

    // Event raised when connectivity status changes
    event EventHandler<bool>? StatusChanged;
}

public interface ILocalConnectivity : IConnectivity
{
}