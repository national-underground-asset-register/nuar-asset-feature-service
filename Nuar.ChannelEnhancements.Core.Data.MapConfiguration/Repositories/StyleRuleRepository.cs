// <copyright file="StyleRuleRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using System.Data;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories
{
    /// <summary>
    /// Implementation of the <see cref="IStyleRuleRepository"/> interface for managing style rules in the database.
    /// </summary>
    public class StyleRuleRepository : QueryBase, IStyleRuleRepository
    {
        private readonly DatabaseConfigurationSettings _dbSettings;

        /// <summary>
        /// StyleRuleRepository constructor.
        /// </summary>
        /// <param name="logDbConnections">The log database connections.</param>
        /// <param name="platformDbConfig">The platform database configuration.</param>
        public StyleRuleRepository(DatabaseConfigurationSettings _dbSettings, bool logDbConnections) : base(platformDbConfig.ConnectionString ?? throw new ArgumentNullException(platformDbConfig.ConnectionString), logDbConnections)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _platformDbConfig = platformDbConfig ?? throw new ArgumentNullException(nameof(platformDbConfig));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StyleRule>?> GetAll()
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                // Query the database for the data
                var styleRules = await Connection.QueryAsync<StyleRule>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetAllStyleRules"]}",
                    param: null,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900) as List<StyleRule>;

                // Loop over the returned style rules to get their conditions, unless the list is null
                if (styleRules == null) return styleRules;
                foreach (var rule in styleRules)
                {
                    rule.Conditions = await GetStyleRuleConditions(rule.Id) as List<StyleRuleCondition>;
                }

                return styleRules;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred trying to get all the style rules from the database.");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StyleRule>?> GetAllByLayerId(Guid layerId)
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                // Query the database for the data
                DynamicParameters p = new DynamicParameters();
                p.Add("@_layer_id", layerId, DbType.Guid, ParameterDirection.Input);

                var styleRules = await Connection.QueryAsync<StyleRule>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetStyleRulesByLayerId"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900) as List<StyleRule>;

                // Loop over the returned style rules to get their conditions, unless the list is null
                if (styleRules == null) return styleRules;
                foreach (var rule in styleRules)
                {
                    rule.Conditions = await GetStyleRuleConditions(rule.Id) as List<StyleRuleCondition>;
                }

                return styleRules;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred trying to get all the style rules from the database.");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<StyleRule?> GetRuleById(Guid styleRuleId)
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                // Query the database for the data
                DynamicParameters p = new DynamicParameters();
                p.Add("@_style_rule_id", styleRuleId, DbType.Guid, ParameterDirection.Input);

                var styleRule = await Connection.QueryAsync<StyleRule>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetStyleRuleById"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900);
                
                // Get the first style rule from the result
                var rule = styleRule.FirstOrDefault();

                // If no rule is found, return null
                if (rule == null) return null;

                // Retrieve the conditions for the style rule
                rule.Conditions = await GetStyleRuleConditions(rule.Id) as List<StyleRuleCondition>;

                return rule;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred trying to retrieve from the database the StyleRule matching identifier {styleRuleId}.", styleRuleId);
                return null;
            }
        }

        /// <summary>
        /// Retrieves the style rule conditions for a given style rule identifier.
        /// </summary>
        /// <param name="styleRuleId">The id of the style rule to get conditions for</param>
        /// <returns>A list of <see cref="StyleRuleCondition"/> if they exist for the style rule</returns>
        private async Task<IEnumerable<StyleRuleCondition>?> GetStyleRuleConditions(Guid styleRuleId)
        {
            try
            {
                // Ensure the Connection object is not null before using it
                if (Connection == null)
                {
                    throw new InvalidOperationException($"{nameof(Connection)} is null. Ensure the database connection is properly initialized.");
                }

                DynamicParameters p = new DynamicParameters();
                p.Add("@_style_rule_id", styleRuleId, DbType.Guid, ParameterDirection.Input);

                // Query the database for the data
                var conditions = await Connection.QueryAsync<StyleRuleCondition>(
                    sql: $"{_platformDbConfig.MapConfigurationSchemaName}.{_platformDbConfig.MapConfigurationFunctionMap["GetStyleRuleConditionsByRuleId"]}",
                    param: p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 900) as List<StyleRuleCondition>;

                // If no conditions are found, return null
                if (conditions == null) return conditions;

                // Take the flat list of conditions returned and turn it into a tree structure
                var conditionTree = new List<StyleRuleCondition>();
                foreach (var condition in conditions)
                {
                    // If the condition has no parent, add it to the root list
                    if (condition.ParentConditionId == Guid.Empty)
                    {
                        conditionTree.Add(condition);
                    }
                    else
                    {
                        // Find the parent condition and add this condition to its Conditions list
                        var parentCondition = conditions.FirstOrDefault(c => c.Id == condition.ParentConditionId);
                        if (parentCondition != null)
                        {
                            parentCondition.Conditions ??= new List<StyleRuleCondition>();
                            parentCondition.Conditions.Add(condition);
                        }
                    }
                }

                // Return the tree structure of conditions
                return conditionTree;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving style rule conditions for StyleRuleId: {StyleRuleId}", styleRuleId);
                throw;
            }
        }
        
    }
}