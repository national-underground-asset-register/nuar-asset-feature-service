using Xunit.Abstractions;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;
using Nuar.ChannelEnhancements.Core.Data.Test.Helpers.Fixture;

namespace Nuar.ChannelEnhancements.Core.Data.Test.MapConfiguration
{
    
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [Collection(nameof(MapConfigurationDatabaseFixture))]
    public class DataAccessLayerTests : IDisposable
    {
        // Local reference to the test output helper
        private readonly ITestOutputHelper _testOutputHelper;

        // Local reference to the database container fixture
        private readonly DatabaseFixture _databaseFixture;
        
        public DataAccessLayerTests(Helpers.Fixture.DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _databaseFixture = databaseFixture;
        }

        public void Dispose()
        {
            _testOutputHelper.WriteLine($"Tests complete in {this.GetType().Name}");
        }

        [Fact]
        public void Constructor_ShouldCreateDataAccessLayer()
        {
            // Arrange
            DatabaseConfigurationSettings dbConfiguration = new DatabaseConfigurationSettings()
            {
                ConnectionString = $"Host={_databaseFixture.DatabaseContainer.Hostname};Port={_databaseFixture.DatabaseContainer.GetMappedPublicPort(5432)};Username=chanexadmin;Password=chanexadmin;Database=platform_db",
                MapConfigurationSchemaName = "configuration",
                MapConfigurationFunctionMap = []
            };

            // Act
            var dataAccessLayer = new DataAccessLayer(dbConfiguration);

            // Assert
            Assert.NotNull(dataAccessLayer);
            Assert.NotNull(dataAccessLayer.LayerRepository);
            Assert.NotNull(dataAccessLayer.LayerGroupRepository);
            Assert.NotNull(dataAccessLayer.StyleRepository);
            Assert.NotNull(dataAccessLayer.StyleRuleRepository);
            Assert.NotNull(dataAccessLayer.AttributeGroupRepository);
            Assert.NotNull(dataAccessLayer.AttributeRepository);
            Assert.NotNull(dataAccessLayer.MapViewRepository);
            Assert.NotNull(dataAccessLayer.MapConfigurationRepository);
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
