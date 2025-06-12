
using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Implementation of <see cref="AttributeGroup"/>.
    /// </summary>
    public class AttributeGroupRepository : QueryBase, IAttributeGroupRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        /// <summary>
        /// AttributeGroupRepository constructor.
        /// </summary>
        /// <param name="platformDbConfig">The database connection configuration.</param>
        /// <param name="logDbConnections">Whether to log database connections.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AttributeGroupRepository(DatabaseConfigurationSettings dbSettings) : base(dbSettings.ConnectionString ?? throw new ArgumentNullException(dbSettings.ConnectionString))
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AttributeGroup>?> GetAll()
        {
            try
            {
                if (Connection == null)
                {
                    throw new Exception("Database connection is null, unable to connect to the database for querying.");
                }

                var attributeGroups = await Connection.QueryAsync<AttributeGroup>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAllAttributeGroups"]}",
                    param: null,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                return attributeGroups.ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(GetAll)} - Unable to retrieve data from database.", ex);
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AttributeGroup>?> GetByMapConfigurationId(Guid mapConfigurationId)
        {
            try
            {
                if (Connection == null)
                {
                    throw new Exception("Database connection is null, unable to connect to the database for querying.");
                }

                var p = new DynamicParameters();
                p.Add("@_map_config_id", mapConfigurationId, DbType.Guid, ParameterDirection.Input);
                
                var attributeGroups = await Connection.QueryAsync<AttributeGroup>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAttributeGroupsByMapConfigurationId"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);
                
                return attributeGroups.ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(GetByMapConfigurationId)} - Unable to retrieve attribute groups for map configuration {mapConfigurationId} from the database.", ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<AttributeGroup?> GetByAttributeGroupId(Guid attributeGroupId)
        {
            try
            {
                if (Connection == null)
                {
                    throw new Exception("Database connection is null, unable to connect to the database for querying.");
                }

                var p = new DynamicParameters();
                p.Add("@_id", attributeGroupId, DbType.Guid, ParameterDirection.Input);

                var attributeGroup = await Connection.QueryAsync<AttributeGroup>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAttributeGroupById"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);

                return attributeGroup.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                Log.Error($"{nameof(GetByAttributeGroupId)} - Unable to retrieve attribute group with identifier {attributeGroupId} from the database.");
                return null;
            }
        }

        
    }
}