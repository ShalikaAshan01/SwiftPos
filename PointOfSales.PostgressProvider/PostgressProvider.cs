using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PointOfSales.Core.Data;
using PointOfSales.Core.IRepositories;
using PointOfSales.Core.Utils;
using PointOfSales.PostgressProvider.Repositories;
using PointOfSales.PostgressProvider.Utils;

namespace PointOfSales.PostgressProvider
{
    public class PostgressProvider: IDatabaseProvider
    {
        private IApplicationLogger _logger;
        public PostgressProvider(IApplicationLogger logger)
        {
            _logger = logger;
        }
        public async Task OnInitAsync(IServiceCollection services)
        {
            const string adminConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=postgres";
            _logger.LogInfo("PostgresProvider OnInitAsync");
            const string localDb = "Host=localhost;Port=5432;Database=SwiftPOS;Username=postgres;Password=password";
            try
            {
                await using var adminConnection = new NpgsqlConnection(adminConnectionString);
                await adminConnection.OpenAsync();
                var databaseName = "SwiftPOS";
                var cmdText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
                await using var checkCmd = new NpgsqlCommand(cmdText, adminConnection);
                var exists = await checkCmd.ExecuteScalarAsync() != null;
                if (!exists)
                {
                    var createCmdText = $"CREATE DATABASE \"{databaseName}\"";
                    await using var createCmd = new NpgsqlCommand(createCmdText, adminConnection);
                    await createCmd.ExecuteNonQueryAsync();

                    _logger.LogInfo("Database {0} created successfully.", databaseName);
                }
                else
                {
                    _logger.LogInfo("Database {0} already exists.", databaseName);
                }
                await using var connection = new NpgsqlConnection(localDb);
                await connection.OpenAsync();
                // run the init scripts
                var transaction = await connection.BeginTransactionAsync();
                var sql = await GetIniSql();
                await using var cmd = new NpgsqlCommand( sql, connection, transaction);
                await cmd.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
                _logger.LogInfo("Database {0} initialized successfully.", databaseName);
                
                services.AddDbContext<MyDbContext>(options =>
                    options.UseNpgsql(localDb));
                services.AddTransient<IEncryptionService, EncryptionService>();
                services.AddTransient<IPermissionRepository, PermissionRepository>();
                services.AddTransient<IUserRepository, UserRepository>();
                services.AddTransient<IUnitOfWork, UnitOfWork>();

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create or verify the database.");
            }


        }

        private async Task<string> GetIniSql()
        {
            
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "PointOfSales.PostgressProvider.Resources.Scripts.1.sql"; 
            await using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
