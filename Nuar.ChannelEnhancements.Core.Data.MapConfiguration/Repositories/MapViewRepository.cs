// <copyright file="MapViewRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Map View Repository class.
    /// </summary>
    public class MapViewRepository : QueryBase, IMapViewRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        /// <summary>
        /// Initializes an instance of <see cref="MapViewRepository"/>.
        /// </summary>
        /// <param name="platformDbConfig">The database connection configuration.</param>
        /// <param name="logDbConnections">The log database connections.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapViewRepository(DatabaseConfigurationSettings _dbSettings) : base(platformDbConfig.ConnectionString ?? throw new ArgumentNullException(nameof(platformDbConfig.ConnectionString)), logDbConnections)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
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
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetMapViewByConfigId"]}",
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
}