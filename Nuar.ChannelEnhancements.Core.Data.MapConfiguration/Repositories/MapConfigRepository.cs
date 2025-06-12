using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    public class MapConfigRepository : QueryBase, IMapConfigurationRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        private readonly IMapViewRepository _mapViewRepository;
        private readonly ILayerGroupRepository _layerGroupRepository;
        private readonly ILayerRepository _layerRepository;
        private readonly IAttributeGroupRepository _attributeGroupRepository;
        private readonly IStyleRepository _styleRepository;

        /// <summary>
        /// Initializes an instance of <see cref="MapConfigRepository"/>.
        /// </summary>
        /// <param name="platformDbConfig">The database connection configuration.</param>
        /// <param name="logDbConnections">The Log database connections.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapConfigRepository(DatabaseConfigurationSettings _dbSettings, IMapViewRepository mapViewRepository, ILayerGroupRepository layerGroupRepository, ILayerRepository layerRepository, IAttributeGroupRepository attributeGroupRepository, IStyleRepository styleRepository) : base(
            platformDbConfig.ConnectionString ??
            throw new ArgumentNullException(nameof(platformDbConfig.ConnectionString)), logDbConnections)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
            _mapViewRepository = mapViewRepository ?? throw new ArgumentNullException(nameof(mapViewRepository));
            _layerGroupRepository = layerGroupRepository ?? throw new ArgumentNullException(nameof(layerGroupRepository));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
            _attributeGroupRepository = attributeGroupRepository ?? throw new ArgumentNullException(nameof(attributeGroupRepository));
            _styleRepository = styleRepository ?? throw new ArgumentNullException(nameof(styleRepository));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Models.MapConfiguration>?> GetAll()
        {
            try
            {
                if (Connection == null)
                {
                    throw new InvalidOperationException("Database connection is not initialized.");
                }

                // Get all map configurations from the database using Dapper
                var mapConfigs = Task.FromResult<IEnumerable<Models.MapConfiguration>?>(Connection.Query<Models.MapConfiguration>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetMapConfigs"]}",
                    param: null,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900) as List<Models.MapConfiguration>);

                // Return null if no map configurations returned
                if (mapConfigs.Result == null || !mapConfigs.Result.Any())
                {
                    return Task.FromResult<IEnumerable<Models.MapConfiguration>?>(null);
                }

                // Create a new list to store updated map configurations
                var updatedConfigs = new List<Models.MapConfiguration>();

                // Loop over each map configuration and populate the related entities
                foreach (var config in mapConfigs.Result ?? Enumerable.Empty<Models.MapConfiguration>())
                {
                    var updatedConfig = config; // Create a local copy of the iteration variable
                    PopulateRelatedEntities(ref updatedConfig); // Pass the local copy by reference
                    updatedConfigs.Add(updatedConfig); // Add the updated configuration to the new list
                }

                return Task.FromResult<IEnumerable<Models.MapConfiguration>?>(updatedConfigs);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(MapConfigRepository.GetAll)} - Unable to retrieve list of map configurations. : {ex.Message}");
                return Task.FromResult<IEnumerable<Models.MapConfiguration>?>(null);
            }
        }

        /// <inheritdoc/>
        public Task<Models.MapConfiguration?> GetMapConfigById(Guid Id)
        {
            try
            {
                if (Connection == null)
                {
                    throw new InvalidOperationException("Database connection is not initialized.");
                }

                var p = new DynamicParameters();
                p.Add("@_id", Id, DbType.Guid, ParameterDirection.Input);

                var mapConfig = Task.FromResult(Connection.Query<Models.MapConfiguration>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetMapConfigById"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900).FirstOrDefault());

                // Return null if no map configuration is found
                if (mapConfig.Result == null)
                {
                    return Task.FromResult<Models.MapConfiguration?>(null);
                }

                // Populate related entities for the retrieved map configuration
                var config = mapConfig.Result;
                PopulateRelatedEntities(ref config);

                // Return the populated map configuration
                return Task.FromResult<Models.MapConfiguration?>(config);
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(MapConfigRepository.GetMapConfigById)} - Unable to retrieve map configuration: {Id}.  {ex.Message}");
                return Task.FromResult<Models.MapConfiguration?>(null);
            }
        }

        private void PopulateRelatedEntities(ref Models.MapConfiguration mapConfig)
        {
            // Get the map view for the map configuration, this is required so throw an exception if not found
            mapConfig.MapView = _mapViewRepository.GetMapViewByMapConfigId(mapConfig.Id).Result
                ?? throw new InvalidOperationException($"Map view with ID {mapConfig.MapViewId} not found for map configuration {mapConfig.Id}.");

            // Get the layer groups for the map configuration
            mapConfig.LayerGroups = _layerGroupRepository.GetLayerGroupsByMapConfigId(mapConfig.Id).Result as List<LayerGroup> ?? new List<LayerGroup>();

            // Get the layers for the map configuration
            mapConfig.Layers = _layerRepository.GetLayersByMapConfigId(mapConfig.Id).Result ?? new List<Layer>();

            // Get the base maps for the map configuration
            var baseMapGroups = mapConfig.LayerGroups?
                .Where(lg => lg.IsBaseMap)
                .ToList() ?? new List<LayerGroup>();

            if (baseMapGroups.Any())
            {
                mapConfig.BaseMaps = [];

                // Loop over the identified base maps groups, mostly going to be 1 but schema allows more
                foreach (var baseMapGroup in baseMapGroups)
                {
                    // Get the layers for the base map group from the repository
                    var baseMapLayer = mapConfig.Layers?.Find(l => l.LayerGroupId == baseMapGroup.Id);

                    // Bail if no base map layer found
                    if (baseMapLayer == null) continue;

                    // Add the layer to the base maps layer collection
                    mapConfig.BaseMaps.Add(baseMapLayer);

                    // Remove it from the main layers collection
                    mapConfig.Layers?.Remove(baseMapLayer);
                }
            }
            
            // Get the attribute groups for the map configuration
            mapConfig.AttributeGroups = _attributeGroupRepository.GetByMapConfigurationId(mapConfig.Id).Result as List<AttributeGroup>
                ?? new List<AttributeGroup>();

            // Get the styles for the map configuration
            mapConfig.Styles = _styleRepository.GetAllStylesByMapConfig(mapConfig.Id).Result as List<Style>
                ?? new List<Style>();
        }
    }
}