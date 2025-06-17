// <copyright file="AttributeRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>using Dapper;

using System.Data;
using Dapper;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;
using Serilog;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;

/// <summary>
/// Implementation of <see cref="IAttributeRepository"/>.
/// </summary>
public class AttributeRepository : QueryBase, IAttributeRepository
{
    private readonly DatabaseConfigurationSettings _dbSettings;

    /// <summary>
    /// Initializes an instance of <see cref="AttributeRepository"/>.
    /// </summary>
    /// <param name="dbSettings">The database connection configuration.</param>
    public AttributeRepository(DatabaseConfigurationSettings dbSettings) : base(dbSettings.ConnectionString ?? throw new ArgumentNullException(nameof(dbSettings.ConnectionString)))
    {
        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Set the database configuration settings
        _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));

        // Register the custom mappings for the LayerAttribute model
        SqlMapper.SetTypeMap(typeof(LayerAttribute), new ColumnAttributeTypeMapper<LayerAttribute>());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LayerAttribute>?> GetAll()
    {
        try
        {
            if (Connection == null)
            {
                throw new Exception("Database connection is null, unable to connect to database for querying.");
            }

            var attributes = await Connection.QueryAsync<LayerAttribute>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetAllAttributes"]}",
                param: null,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            return attributes.ToList() ?? null;
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetAll)} - Unable to get attributes from the database.", ex);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LayerAttribute>?> GetAllByLayerId(Guid layerId)
    {
        try
        {
            if (Connection == null)
            {
                throw new Exception("Database connection is null, unable to connect to database for querying.");
            }

            var p = new DynamicParameters();
            p.Add("@_layer_id", layerId, DbType.Guid, ParameterDirection.Input);

            var attributes = await Connection.QueryAsync<LayerAttribute>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetAttributesByLayerId"]}",
                param: p,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            return attributes.ToList() ?? null;
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetAllByLayerId)} - Unable to get attribute groups from the database.", ex);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LayerAttribute>?> GetAllQueryableByLayerId(Guid layerId)
    {
        try
        {
            if (Connection == null)
            {
                throw new Exception("Database connection is null, unable to connect to database for querying.");
            }

            var p = new DynamicParameters();
            p.Add("@_layer_id", layerId, DbType.Guid, ParameterDirection.Input);

            var attributes = await Connection.QueryAsync<LayerAttribute>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetQueryableAttributesByLayerId"]}",
                param: p,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);
                
            return attributes.ToList() ?? null;
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetAllQueryableByLayerId)} - Unable to get queryable attributes from the database.", ex);
            return null;
        }
    }
}