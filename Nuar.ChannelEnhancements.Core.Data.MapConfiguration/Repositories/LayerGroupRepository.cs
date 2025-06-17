// <copyright file="LayerGroupRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using System.Data;
using Dapper;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;
using Serilog;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;

/// <summary>
/// Implementation of the <see cref="ILayerGroupRepository"/> interface for managing layer groups in the map configuration database.
/// </summary>
public class LayerGroupRepository : QueryBase, ILayerGroupRepository
{
    private readonly DatabaseConfigurationSettings _dbSettings;

    /// <summary>
    /// LayerGroupRepository constructor  
    /// <param name="dbSettings">The database connection configuration</param>
    /// </summary>
    /// <exception cref="ArgumentNullException">Raised when an argument is null</exception>
    public LayerGroupRepository(DatabaseConfigurationSettings dbSettings) : base(dbSettings.ConnectionString ?? throw new ArgumentNullException(dbSettings.ConnectionString))
    {
        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Set the database configuration settings
        _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));

        // Register the custom mappings for the LayerGroup model
        SqlMapper.SetTypeMap(typeof(LayerGroup), new ColumnAttributeTypeMapper<LayerGroup>());
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
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetLayerGroupsByConfigId"]}",
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
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetLayerGroupById"]}",
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

    /// <inheritdoc />
    public async Task<IEnumerable<LayerGroup>?> GetChildLayerGroups(Guid parentGroupId)
    {
        try
        {
            if (Connection == null)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
            }

            var p = new DynamicParameters();
            p.Add("@_parent_group_id", parentGroupId, DbType.Guid, ParameterDirection.Input);

            var layerGroups = await Connection.QueryAsync<LayerGroup>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetChildLayerGroupsByParentId"]}",
                param: p,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            // Convert the result to a list for further processing
            var resultLayerGroups = layerGroups.ToList();

            if (resultLayerGroups.Count > 0) return resultLayerGroups;

            // If no layer groups are found, return null
            Log.Information($"{nameof(LayerGroupRepository.GetChildLayerGroups)} - Layer Group [{parentGroupId}] does not have any children.");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}