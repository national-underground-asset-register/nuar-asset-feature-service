using System.Reflection;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;
using Nuar.ChannelEnhancements.Core.Data.Test.Helpers.Fixture;
using Xunit.Abstractions;

namespace Nuar.ChannelEnhancements.Core.Data.Test.MapConfiguration
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [Collection(nameof(MapConfigurationDatabaseFixture))]
    public class AttributeRepositoryTests : IDisposable
    {
        // Local reference to test output helper
        private readonly ITestOutputHelper _testOutputHelper;

        // Local reference to the attribute repository
        private readonly IAttributeRepository _attributeRepository;

        public AttributeRepositoryTests(DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper) 
        { 
            // Store reference to the test output helper
            _testOutputHelper = testOutputHelper;

            // Create a database configuration settings object specific to tests for attribute repository
            DatabaseConfigurationSettings databaseConfiguration = new DatabaseConfigurationSettings()
            {
                ConnectionString = $"Host={databaseFixture.DatabaseContainer.Hostname};Port={databaseFixture.DatabaseContainer.GetMappedPublicPort(5432)};Username=chanexadmin;Password=chanexadmin;Database=platform_db",
                MapConfigurationSchemaName = "configuration",
                MapConfigurationFunctionMap = new Dictionary<string, string>
                {
                    { "GetAttributesByLayerId", "fn_get_attributes_by_layer_id" },
                    { "GetAllAttributes", "fn_get_all_attributes" },
                    { "GetQueryableAttributesByLayerId", "fn_get_queryable_attributes_by_layer_id" },
                    { "GetAttributeById", "fn_get_attribute_by_id" }
                }
            };

            // Create the attribute repository and store reference to it
            _attributeRepository = new AttributeRepository(databaseConfiguration);
        }

        public void Dispose()
        {
            _testOutputHelper.WriteLine($"Tests complete in {this.GetType().Name}");
        }

        [Fact]
        public async Task GetAllByLayerId_ShouldReturnLayerAttributes()
        {
            // Arrange  
            var layerId = Guid.Parse("b0e53b3b-78df-438c-a4ee-caad4dea2e4a");

            // Act  
            var result = await _attributeRepository.GetAllByLayerId(layerId);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(52, result.Count());

            // Log output for debugging  
            _testOutputHelper.WriteLine(
                $"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for LayerId: {layerId}");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllLayerAttributes()
        {
            // Arrange  

            // Act  
            var result = await _attributeRepository.GetAll();

            // Assert  
            Assert.NotNull(result);
            Assert.True(result.Any()); // Replace Count() > 0 with Any()  

            // Log output for debugging  
            _testOutputHelper.WriteLine($"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed");
        }

        [Fact]
        public async Task GetAllQueryableByLayerId_ShouldReturnOnlyQueryableLayerAttributes()
        {
            // Arrange
            var layerId = Guid.Parse("b0e53b3b-78df-438c-a4ee-caad4dea2e4a");

            // Act  
            var result = await _attributeRepository.GetAllQueryableByLayerId(layerId);

            // Assert  
            Assert.NotNull(result);
            var layerAttributes = result.ToList();
            Assert.Equal(38, layerAttributes.Count());
            Assert.All(layerAttributes, attr => Assert.True(attr.IsQueryable));

            // Log output for debugging  
            _testOutputHelper.WriteLine(
                $"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for LayerId: {layerId}");
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
