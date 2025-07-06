using Npgsql;
using PointOfSales.Core.Utils;

namespace PointOfSales.PostgressProvider.Utils
{
    public class LocalDbConnectivity : ILocalConnectivity, IDisposable
    {
        private readonly string _connectionString;
        private Timer? _timer;
        private bool _isConnected;

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    StatusChanged?.Invoke(this, _isConnected);
                }
            }
        }

        public event EventHandler<bool>? StatusChanged;

        public LocalDbConnectivity(string connectionString)
        {
            _connectionString = connectionString;

            // Check connectivity every 5 seconds (adjust as needed)
            _timer = new Timer(async _ => await CheckConnectionAsync(), null, 0, 5000);
        }

        private async Task CheckConnectionAsync()
        {
            var builder = new NpgsqlConnectionStringBuilder(_connectionString)
            {
                Pooling = false // disable pooling for health check
            };

            try
            {
                await using var conn = new NpgsqlConnection(builder.ConnectionString);
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await conn.OpenAsync(cts.Token);
                IsConnected = true;
                await conn.CloseAsync();
            }
            catch
            {
                IsConnected = false;
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}