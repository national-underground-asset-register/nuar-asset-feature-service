// <copyright file="LayerGroupRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Implementation of the <see cref="ILayerGroupRepository"/> interface for managing layer groups in the map configuration database.
    /// </summary>
    public class LayerGroupRepository : QueryBase, ILayerGroupRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        /// <summary>
        /// LayerGroupRepository constructor  
        /// <param name="platformDbConfig">The database connection configuration</param>
        /// <param name="logDbConnections">Whether to log database connections</param>
        /// </summary>
        /// <exception cref="ArgumentNullException">Raised when an argument is null</exception>
        public LayerGroupRepository(DatabaseConfigurationSettings _dbSettings, bool logDbConnections) : base(platformDbConfig.ConnectionString ?? throw new ArgumentNullException(platformDbConfig.ConnectionString), logDbConnections)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LayerGroup>?> GetLayerGroupsByMapConfigId(Guid configId)
        {
            try
            {
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                var p = new DynamicParameters();
                p.Add("@_id", configId, DbType.Guid, ParameterDirection.Input);

                var layerGroups = await Connection.QueryAsync<LayerGroup>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetLayerGroupsByConfigId"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                // Convert the result to a list for further processing
                var resultLayerGroups = layerGroups.ToList();

                if (resultLayerGroups.Count > 0) return resultLayerGroups;

                // If no layer groups are found, return null
                Log.Information($"{nameof(LayerGroupRepository.GetLayerGroupsByMapConfigId)} - No layer groups found for configId: {configId}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LayerGroupRepository.GetLayerGroupsByMapConfigId)} - Unable to retrieve layer groups from database for configId:{configId}", ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<LayerGroup?> GetLayerGroupById(Guid configId, Guid layerGroupId)
        {
            try
            {
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                var p = new DynamicParameters();
                p.Add("@_layer_group_id", layerGroupId, DbType.Guid, ParameterDirection.Input);

                var layerGroup = await Connection.QueryFirstOrDefaultAsync<LayerGroup>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetLayerGroupById"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                return layerGroup;
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LayerGroupRepository.GetLayerGroupById)} - Unable to retrieve layer group from database identifier {layerGroupId}.", ex);
                return null;
            }
        }
    }
}