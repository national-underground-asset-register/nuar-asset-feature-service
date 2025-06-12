// <copyright file="AttributeRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>using Dapper;

using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Implementation of <see cref="IAttributeRepository"/>.
    /// </summary>
    public class AttributeRepository : QueryBase, IAttributeRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        /// <summary>
        /// Initializes an instance of <see cref="AttributeRepository"/>.
        /// </summary>
        /// <param name="platformDbConfig">The database connection configuration.</param>
        /// <param name="logDbConnections">Whether to log database connections.</param>
        public AttributeRepository(DatabaseConfigurationSettings _dbSettings, bool logDbConnections) : base(platformDbConfig.ConnectionString ?? throw new ArgumentNullException(nameof(platformDbConfig.ConnectionString)), logDbConnections)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
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
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAllAttributes"]}",
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
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAttributesByLayerId"]}",
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
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetQueryableAttributesByLayerId"]}",
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
}