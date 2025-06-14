// <copyright file="MapViewRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;

using System.Data;
using Dapper;
using Helpers;
using Interfaces;
using Models;
using Serilog;

/// <summary>
/// Map View Repository class.
/// </summary>
public class MapViewRepository : QueryBase, IMapViewRepository
{
    private readonly DatabaseConfigurationSettings _dbSettings;

    /// <summary>
    /// Initializes an instance of <see cref="MapViewRepository"/>.
    /// </summary>
    /// <param name="dbSettings">The database connection configuration.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MapViewRepository(DatabaseConfigurationSettings dbSettings) : base(dbSettings.ConnectionString ?? throw new ArgumentNullException(nameof(dbSettings.ConnectionString)))
    {
        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Set the database configuration settings
        _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));

        // Register the custom mappings for the MapView model
        SqlMapper.SetTypeMap(typeof(MapView), new ColumnAttributeTypeMapper<MapView>());
    }

    /// <inheritdoc/>
    public async Task<MapView?> GetMapViewByMapConfigId(Guid configId)
    {
        try
        {
            if (Connection == null)
            {
                throw new InvalidOperationException("Database connection is not initialized.");
            }

            var p = new DynamicParameters();
            p.Add("@_id", configId, DbType.Guid, ParameterDirection.Input);
            var mapView = await Connection.QueryAsync<MapView>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetMapViewByConfigId"]}",
                param: p,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            return mapView.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetMapViewByMapConfigId)} - Unable to retrieve Map View from database for map configuration with identifier {configId}", ex);
            return null;
        }
            
    }

        
}