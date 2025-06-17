using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Nuar.ChannelEnhancements.Core.Data.Test.Helpers.Fixture
{
    public class DatabaseFixture : IAsyncLifetime
    {
        // Reference to the database container
        public IContainer? DatabaseContainer { get; private set; } = null;

        public async Task InitializeAsync()
        {
            // Get the base directory where a database backup to restore is located along with an initialisation script
            var baseDir = AppContext.BaseDirectory;
            var databaseBackupPath = Path.Combine(baseDir, "Resources\\database.backup");
            var bootstrapScriptPath = Path.Combine(baseDir, "Resources\\bootstrap-database.sh");

            // Determine if the database backup file exists
            if (!File.Exists(databaseBackupPath))
            {
                throw new FileNotFoundException($"Backup file not found at: {databaseBackupPath}");
            }
            
            // Determine if the bootstrap script exists
            if (!File.Exists(bootstrapScriptPath))
            {
                throw new FileNotFoundException($"Script file not found at: {bootstrapScriptPath}");
            }

            // Build the container specifying the backup file and the bootstrap script and loading them into the container
            DatabaseContainer = new ContainerBuilder()
                .WithImage("postgis/postgis:17-3.5")
                .WithEnvironment("POSTGRES_USER", "chanexadmin")
                .WithEnvironment("POSTGRES_PASSWORD", "chanexadmin")
                .WithEnvironment("POSTGRES_DB", "platform_db")
                .WithPortBinding(5432, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .WithBindMount(databaseBackupPath, "/docker-entrypoint-initdb.d/database.backup")
                .WithBindMount(bootstrapScriptPath, "/docker-entrypoint-initdb.d/bootstrap-database.sh")
                .Build();

            // Start the database container
            await DatabaseContainer.StartAsync()
                .ConfigureAwait(false);
        }

        public async Task DisposeAsync()
        {
            // If the database container is available then we need to shut it down cleanly
            if (DatabaseContainer != null)
            {
                await DatabaseContainer.StopAsync()
                    .ConfigureAwait(false);
                await DatabaseContainer.DisposeAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
