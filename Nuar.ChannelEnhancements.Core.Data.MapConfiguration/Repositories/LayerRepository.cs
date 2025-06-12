// <copyright file="LayerRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Implements the <see cref="ILayerRepository"/> interface to provide access to layer data in the database.
    /// </summary>
    public class LayerRepository : QueryBase, ILayerRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        private readonly IAttributeRepository _attributeRepository;
        private readonly IStyleRuleRepository _styleRuleRepository;

        /// <summary>
        /// Initializes an instance of <see cref="LayerRepository"/>.
        /// </summary>
        /// <param name="platformDbConfig">The database configuration</param>
        /// <param name="logDbConnection">Whether to log database connections</param>
        /// <param name="attributeRepository">The <see cref="AttributeRepository"/></param>
        /// <param name="styleRuleRepository">The <see cref="StyleRepository"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LayerRepository(DatabaseConfigurationSettings _dbSettings, bool logDbConnection,
                               IAttributeRepository attributeRepository, IStyleRuleRepository styleRuleRepository) : base(
                               platformDbConfig.ConnectionString ?? throw new ArgumentNullException(nameof(platformDbConfig.ConnectionString)), logDbConnection)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

            _attributeRepository = attributeRepository;
            _styleRuleRepository = styleRuleRepository;
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
        }

        /// <inheritdoc/>
        public async Task<List<Layer>?> GetLayersByGroupId(Guid layerGroupId)
        {
            // Ensure the Connection object is not null before using it
            if (Connection == null)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
            }

            try
            {
                var param = new DynamicParameters();
                param.Add("@_layer_group_id", layerGroupId, DbType.Guid, ParameterDirection.Input);

                var layers = await Connection.QueryAsync<Layer>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetLayersByGroupId"]}",
                    param: param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                var layerList = layers.ToList();

                // Return null if no layers returned
                if (layerList.Count == 0) return null;

                // Populate source properties for each layer
                foreach (var layer in layerList)
                {
                    layer.SourceProperties = GetSourceProperties(layer.Id);
                    layer.Attributes = (await _attributeRepository.GetAllByLayerId(layer.Id))?.ToList();
                    layer.StyleRules = (await _styleRuleRepository.GetAllByLayerId(layer.Id))?.ToList();
                }

                return layerList;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LayerRepository.GetLayersByGroupId)} - Unable to retrieve layers from the database belonging to layer group with identifier {layerGroupId}.", ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Layer>?> GetLayersByMapConfigId(Guid configId)
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                var param = new DynamicParameters();
                param.Add("@_id", configId, DbType.Guid, ParameterDirection.Input);

                var layers = await Connection.QueryAsync<Layer>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetLayersByMapConfigId"]}",
                    param: param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                var layerList = layers.ToList();

                // Return null if no layers returned
                if (layerList.Count == 0) return null;

                // Populate source properties for each layer
                foreach (var layer in layerList)
                {
                    layer.SourceProperties = GetSourceProperties(layer.Id);
                    layer.Attributes = (await _attributeRepository.GetAllByLayerId(layer.Id))?.ToList();
                    layer.StyleRules = (await _styleRuleRepository.GetAllByLayerId(layer.Id))?.ToList();
                }

                return layerList;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LayerRepository.GetLayersByMapConfigId)} - Unable to retrieve layers from the database belonging to layer group with identifier {configId}.", ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<Layer?> GetLayerByMapConfigIdAndLayerId(Guid mapConfigId, Guid layerId)
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                var param = new DynamicParameters();
                param.Add("@_layer_id", layerId, DbType.Guid, ParameterDirection.Input);
                param.Add("@_config_id", mapConfigId, DbType.Guid, ParameterDirection.Input);
                
                var layer = await Connection.QueryAsync<Layer>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetLayerByMapConfigIdAndLayerId"]}",
                    param: param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                // Get the first layer from the result set or null if no layers found
                var returnLayer = layer.FirstOrDefault();

                // If no layer was found return the null result
                if (returnLayer == null) return returnLayer;

                // If a layer was found, populate its source properties and attributes
                returnLayer.SourceProperties = GetSourceProperties(layerId);
                returnLayer.Attributes = (await _attributeRepository.GetAllByLayerId(layerId))?.ToList();
                returnLayer.StyleRules = (await _styleRuleRepository.GetAllByLayerId(layerId))?.ToList();

                return returnLayer;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LayerRepository.GetLayerByMapConfigIdAndLayerId)} - Unable to retrieve layer from database for layer with identifier {layerId}.", ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieves the source properties for a layer by its identifier.
        /// </summary>
        /// <param name="layerId">The identifier of the layer</param>
        /// <returns>A dictionary of key/value pairs, or null if layer has no source properties</returns>
        /// <exception cref="Exception">Thrown if any exception occurs</exception>
        private Dictionary<string, string>? GetSourceProperties(Guid layerId)
        {
            // Ensure the Connection object is not null before using it
            if (Connection == null)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
            }

            var p = new DynamicParameters();
            p.Add("@_layer_id", layerId, DbType.Guid, ParameterDirection.Input);

            var sourceProperties = Connection.Query(
                sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetSourcePropertiesByLayerId"]}",
                param: p,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            // Return null if no properties fetched from the database
            var properties = sourceProperties.ToList();

            if (properties.Count == 0) return null;

            Dictionary<string, string> sourcePropertiesDict = new();
            foreach (var property in properties)
            {
                if (property != null)
                {
                    var key = property.Key.ToString();
                    var value = property.Value?.ToString();
                    if (!string.IsNullOrEmpty(key))
                    {
                        sourcePropertiesDict[key] = value;
                    }
                }
            }

            return sourcePropertiesDict;
        }
    }
}