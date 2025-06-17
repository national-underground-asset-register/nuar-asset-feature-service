using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;
using Nuar.ChannelEnhancements.Core.Data.Test.Helpers.Fixture;
using Xunit.Abstractions;

namespace Nuar.ChannelEnhancements.Core.Data.Test.MapConfiguration
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [Collection(nameof(MapConfigurationDatabaseFixture))]
    public class LayerRepositoryTests : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILayerRepository _layerRepository;

        public LayerRepositoryTests(DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            DatabaseConfigurationSettings databaseConfiguration = new DatabaseConfigurationSettings()
            {
                ConnectionString = $"Host={databaseFixture.DatabaseContainer.Hostname};Port={databaseFixture.DatabaseContainer.GetMappedPublicPort(5432)};Username=chanexadmin;Password=chanexadmin;Database=platform_db",
                MapConfigurationSchemaName = "configuration",
                MapConfigurationFunctionMap = new Dictionary<string, string>
                    {
                        { "GetLayersByGroupId", "fn_get_layers_by_group_id" },
                        { "GetLayersByMapConfigId", "fn_get_layers_by_map_config_id" },
                        { "GetLayerByMapConfigIdAndLayerId", "fn_get_layer_by_config_id_and_layer_id" },
                        { "GetAttributesByLayerId", "fn_get_attributes_by_layer_id" },
                        { "GetStyleRulesByLayerId", "fn_get_style_rules_by_layer_id"},
                        { "GetSourcePropertiesByLayerId", "fn_get_source_properties_by_layer_id"},
                        { "GetStyleRuleConditionsByRuleId", "fn_get_style_rule_conditions_by_rule_id" }
                    }
            };

            StyleRuleRepository styleRuleRepository = new StyleRuleRepository(databaseConfiguration);
            AttributeRepository attributeRepository = new AttributeRepository(databaseConfiguration);
            _layerRepository = new LayerRepository(databaseConfiguration, attributeRepository, styleRuleRepository);
        }

        public void Dispose()
        {
            _testOutputHelper.WriteLine($"Tests complete in {this.GetType().Name}");
        }

        [Fact]
        public async Task GetLayersByGroupId_ShouldReturnLayers()
        {
            // Arrange
            var layerGroupId = Guid.Parse("4c834c30-c79c-4f69-9948-2bacf70665c5");

            // Act
            var result = await _layerRepository.GetLayersByGroupId(layerGroupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(11, result.Count);

            // Assert that only layers with source type Vector are returned
            Assert.All(result, layer => Assert.Equal("Vector", layer.SourceType));

            // Log output for debugging
            _testOutputHelper.WriteLine($"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for LayerGroupId: {layerGroupId}");
        }

        [Fact]
        public async Task GetLayersByMapConfigId_ShouldReturnLayers()
        {
            // Arrange  
            var configId = Guid.Parse("146ea7bb-41f5-4e91-9d2a-e237b6615ee6");

            // Act  
            var result = await _layerRepository.GetLayersByMapConfigId(configId);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(110, result.Count);

            // Log output for debugging  
            _testOutputHelper.WriteLine($"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for map configuration id: {configId}");
        }

        [Fact]
        public async Task GetLayerByMapConfigurationIdAndLayerId_ShouldReturnLayer()
        {
            // Arrange
            var mapConfigId = Guid.Parse("146ea7bb-41f5-4e91-9d2a-e237b6615ee6");
            var layerId = Guid.Parse("b0e53b3b-78df-438c-a4ee-caad4dea2e4a");
            var layerGroupId = Guid.Parse("869b156d-7daa-444d-aab0-b98ac1ac5416");

            // Act
            var result = await _layerRepository.GetLayerByMapConfigIdAndLayerId(mapConfigId, layerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(layerId, result.Id);
            Assert.Equal(layerGroupId, result.LayerGroupId);
            Assert.Equal("waternetworklink", result.Name);
            Assert.Equal("Water network link", result.DisplayNameEng);
            Assert.Equal("Cyswllt y rhwydwaith dŵr", result.DisplayNameCym);
            Assert.NotNull(result.DisplayName);
            Assert.Equal(2, result.DisplayName.Count);
            Assert.Contains(result.DisplayName, kvp => kvp.Key == "en" && kvp.Value == "Water network link");
            Assert.Contains(result.DisplayName, kvp => kvp.Key == "cy" && kvp.Value == "Cyswllt y rhwydwaith dŵr");
            Assert.Equal("Generic layer for all regions", result.Description);
            Assert.Equal(59, result.DisplayOrder);
            Assert.Equal("Vector", result.SourceType);
            Assert.Equal(1, result.MinimumScale);
            Assert.Equal(5000, result.MaximumScale);
            Assert.True(result.IsCheckedByDefault);

            // Assert that the layer has attributes
            Assert.NotEmpty(result.Attributes ?? []);
            Assert.Equal(52, result.Attributes.Count);

            // Assert that the layer style rules
            Assert.NotEmpty(result.StyleRules ?? []);
            Assert.Equal(2, result.StyleRules.Count);
            Assert.All(result.StyleRules, rule => Assert.NotNull(rule.Conditions));

            // Log output for debugging
            _testOutputHelper.WriteLine(
                $"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for configuration id [{mapConfigId}] and layer id [{layerId}]");
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
