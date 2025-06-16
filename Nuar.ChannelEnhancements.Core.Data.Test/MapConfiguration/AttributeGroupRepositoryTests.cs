using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xunit.Abstractions;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;

namespace Nuar.ChannelEnhancements.Core.Data.Test.MapConfiguration
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [Collection(nameof(MapConfigurationDatabaseFixture))]
    public class AttributeGroupRepositoryTests : IDisposable
    {
        // Local reference to the test output helper
        private readonly ITestOutputHelper _testOutputHelper;

        // Local reference to the AttributeGroupRepository
        private readonly IAttributeGroupRepository _attributeGroupRepository;

        public AttributeGroupRepositoryTests(Helpers.Fixture.DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            DatabaseConfigurationSettings databaseConfigurationSettings = new DatabaseConfigurationSettings()
            {
                ConnectionString = $"Host={databaseFixture.DatabaseContainer.Hostname};Port={databaseFixture.DatabaseContainer.GetMappedPublicPort(5432)};Username=chanexadmin;Password=chanexadmin;Database=platform_db",
                MapConfigurationSchemaName = "configuration",
                MapConfigurationFunctionMap = new Dictionary<string, string>
                    {
                        { "GetAllAttributeGroups", "fn_get_all_attribute_groups" },
                        { "GetAttributeGroupById", "fn_get_attribute_group_by_id" },
                        { "GetSourcePropertyById", "fn_get_source_property_by_id" },
                    }
            };

            _attributeGroupRepository = new AttributeGroupRepository(databaseConfigurationSettings);
        }

        public void Dispose()
        {
            _testOutputHelper.WriteLine($"Tests complete in {this.GetType().Name}");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAttributeGroups()
        {
            // Arrange  

            // Act  
            var result = await _attributeGroupRepository.GetAll();

            // Assert  
            Assert.NotNull(result);
            Assert.True(result.Any()); // Replace Count() > 0 with Any()  

            // Log output for debugging  
            _testOutputHelper.WriteLine($"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed");
        }

        [Fact]
        public async Task GetByAttributeGroupId_ShouldReturnAttributeGroup()
        {
            // Arrange
            var attributeGroupId = Guid.Parse("26213fdf-4f9e-4b80-a570-56a9de36330a");

            // Act
            var result = await _attributeGroupRepository.GetByAttributeGroupId(attributeGroupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(attributeGroupId, result.Id);
            Assert.Equal("Asset Section", result.DisplaySection);
            Assert.Equal(2, result.DisplayOrder);
            Assert.Equal("Installation details", result.DisplayNameEng);
            Assert.Equal("Manylion gosod", result.DisplayNameCym);
            Assert.NotNull(result.DisplayName);
            Assert.Equal(2, result.DisplayName.Count);
            Assert.Contains(result.DisplayName, kvp => kvp.Key == "en" && kvp.Value == "Installation details");
            Assert.Contains(result.DisplayName, kvp => kvp.Key == "cy" && kvp.Value == "Manylion gosod");
            Assert.Equal("This is intended for asset layers only.", result.Description);

            // Log output for debugging
            _testOutputHelper.WriteLine($"Test {MethodBase.GetCurrentMethod().ReflectedType.Name} passed for AttributeGroupId: {attributeGroupId}");
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
