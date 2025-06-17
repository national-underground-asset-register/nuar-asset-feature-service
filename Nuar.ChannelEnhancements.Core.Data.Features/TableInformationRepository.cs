// <copyright file="TableInformationRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>
namespace Nce.Core.Data.Features
{
    using Dapper;
    using Nuar.ChannelEnhancements.Core.Data.Features.Interfaces;
    using Nuar.ChannelEnhancements.Core.Data.Features.Models;
    using Serilog;

    public class TableInformationRepository : QueryBase, ITableInformationRepository
    {
        private AssetDBConfig _assetDBConfig { get; set; }

        // Create a blank dictionary with a comparison that ignores case
        private Dictionary<string, List<string>> _cachedTables;

        // Create a lock for updating whole _cachedTables dictionary and accessing it
        private static readonly object tableCacheLock = new object();

        /// <summary>
        /// The classname for logging purposes
        /// </summary>
        private readonly string _ClassName = nameof(TableInformationRepository);

        /// <summary>
        /// The class constructor for TableInformationRepository
        /// </summary>
        /// <param name="assetDBConfigIn">Special configuration for the asset database</param>
        /// <param name="connectionString">The database connection string.</param>
        public TableInformationRepository(AssetDBConfig assetDBConfigIn,
            bool logConnectionsToDb) : base(assetDBConfigIn?.AssetDBConnectionString ?? throw new ArgumentNullException(nameof(assetDBConfigIn.AssetDBConnectionString)), logConnectionsToDb)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            _assetDBConfig = assetDBConfigIn ?? throw new ArgumentNullException(nameof(assetDBConfigIn));
            _cachedTables = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        // TEMP CODE UNTIL WE GET A FUNCTION
        /// <summary>
        /// Gets the tables and schemas for tables 
        /// </summary>
        /// <returns>A dictionary of Table name to schema</returns>

        public List<string>? GetTables(string schemaName, List<string>? assetTypes = null, List<string>? assetNames = null)
        {
            string logPrefix = $"{_ClassName}:{nameof(GetTables)}";

            Log.Debug($"{logPrefix}: Getting tables from db");
            // Construct parameterised SQL statement to lookup the table in the information schema
            //string sql = $"SELECT table_name, table_schema FROM information_schema.tables"; old version.
            string sql = $"SELECT f_table_name, f_table_schema FROM public.geometry_columns";
            // Define parameters to inject in the SQL
            var p = new DynamicParameters();

            // Turn the asset information into a where clauses for the sql Query
            string tableTypeWhereClause = GetTableWhereClause(assetNames, assetTypes);
            if (!tableTypeWhereClause.Equals(""))
            {
                sql += $" WHERE {tableTypeWhereClause} AND f_table_schema = '{schemaName}'";
            }
            else
            {
                sql += $" WHERE f_table_schema = '{schemaName}'";
            }

            Log.Debug($"{logPrefix}: SQL: {sql}");

            // Execute the query and if we get 1 as the result then this table exists in the database
            var results = (Connection != null) ? Connection.Query(sql, p).ToList() : throw new Exception("Failed to establish database connection.");

            var tableNameList = new List<string>();
            foreach (var row in results)
            {
                if ((!string.IsNullOrEmpty((string)row.f_table_name)) &&
                        (!tableNameList.Contains((string)row.f_table_name)))
                {
                    // if no asset type is set just add it to the dictionary
                    tableNameList.Add((string)row.f_table_name);
                }
            }

            Log.Debug($"{logPrefix}: Finished getting node tables");

            return tableNameList;
        }

        // TEMP CODE UNTIL WE GET A FUNCTION
        /// <summary>
        /// Gets all the tables and schemas for tables and can cache it if appropriate
        /// </summary>
        /// <param name="schemaName">The name of the schema.</param>
        /// <returns>A dictionary of Table name to schema.</returns>
        /// <exception cref="Exception"></exception>
        public List<string>? GetAllTables(string schemaName)
        {
            string logPrefix = $"{_ClassName}:{nameof(GetAllTables)}";

            if (schemaName == null)
            {
                Log.Error($"{logPrefix}: Null schema Name supplied");
                return null;
            }
            // look up lock for schema <TODO>
            List<string>? cachedTableNameList = null;
            if (_cachedTables.ContainsKey(schemaName))
            {
                // we have an entry for this schema
                // At the moment this is not cached but it could be cached here
                cachedTableNameList = _cachedTables[schemaName];
                if (cachedTableNameList != null)
                {
                    // return a copy of the dictionary so we do not have race problems
                    lock (cachedTableNameList)
                    {
                        List<string> tableNameList = new (cachedTableNameList);
                        if (tableNameList == null)
                        {
                            Log.Error($"{logPrefix}: Could not copy the table Names list");
                        }
                        else
                        {
                            return tableNameList;
                        }
                    }
                }
            }
            // If we get here our cache is empty or there is no tableNameList associated to the given schema
            // We are refreshing the cache or this is a first time grab
            Log.Debug($"{logPrefix}: Getting tables from db for our schema:{schemaName}");

            // Construct parameterised SQL statement to lookup the table in the information schema
            string sql = $"SELECT table_name, table_schema FROM information_schema.tables where table_schema = '{schemaName}'";
            // Define parameters to inject in the SQL
            var p = new DynamicParameters();
            Log.Debug($"{logPrefix}: {sql}");
            // Execute the query and if we get 1 as the result then this table exists in the database
            var results = (Connection != null) ? Connection.Query(sql, p).ToList() : throw new Exception("Failed to establish database connection.");

            var tableNamesList = new List<string>();
            lock (tableNamesList)
            {
                foreach (var row in results)
                {
                    if (!string.IsNullOrEmpty((string)row.table_name))
                    {
                        // Add it to our list
                        tableNamesList.Add(row.table_name);
                    }
                }
            }
            List<string>? resultTableNamesList = null;
            lock (tableCacheLock)
            {
                if ((_cachedTables != null) && (_cachedTables.ContainsKey(schemaName)))
                {
                    _cachedTables[schemaName] = tableNamesList;
                }
                else if (_cachedTables == null)
                {
                    Log.Error($"{logPrefix}: Unknown error with table cache");
                    return null;
                }
                else
                {
                    _cachedTables.Add(schemaName, tableNamesList);
                }
                // Return copy of the tableNamesList
                resultTableNamesList = new List<string>(tableNamesList);
            }

            Log.Debug($"{logPrefix}: Finished getting tables");

            return resultTableNamesList;
        }

        // TEMP CODE UNTIL WE GET A FUNCTION
        /// <summary>
        /// Gets the tables and schemas for tables that end in Link.
        /// </summary>
        /// <param name="assetTypes">A list of asset types as strings.</param>
        /// <param name="assetNames">A list of asset names as strings.</param>
        /// <returns>The string to use in a WHERE clause or "".</returns>
        private string GetTableWhereClause(List<string>? assetTypes, List<string>? assetNames)
        {
            string logPrefix = $"{_ClassName}:{nameof(GetTableWhereClause)}";
            string sql = "";

            Log.Debug($"{logPrefix}: Putting together a where clause if required");
            // Turn the tableTypes into the sql Query
            string tableNameSql = "";
            int iNameClauseAdded = 0;
            if (assetNames != null)
            {
                // This is giving us specific asset Names to use
                foreach (string assetName in assetNames)
                {
                    if (AssetTypes.validateAssetName(assetName))
                    {
                        tableNameSql += $"f_table_name like '%{assetName.ToLower()}'";
                        iNameClauseAdded++;
                        // Check to see if we need to add an 'and' - remember we do not want to
                        // add 'and' for the last one
                        if ((assetNames.Count > 1) && (assetNames.Count == iNameClauseAdded))
                        {
                            tableNameSql += " and";
                        }
                    }
                    else
                    {
                        Log.Debug($"{logPrefix}: Ignoring unknown type:{assetName}");
                    }
                }
            }

            int iTypeClauseAdded = 0;
            string tableTypeSql = "";

            // If we have a valid asset type but it is not ALL then we add to the where clause
            if ((assetTypes != null) && (!(assetTypes[0].Equals(AssetTypes.All))))
            {
                foreach (string assetType in assetTypes)
                {
                    if (AssetTypes.validateAssetType(assetType))
                    {
                        tableTypeSql += $"f_table_name like '%{assetType}'";
                        iTypeClauseAdded++;

                        // Check to see if we need to add an 'and' - remember we do not want to
                        // add 'and' for the last one
                        if ((assetTypes.Count > 1) && (assetTypes.Count == iTypeClauseAdded))
                        {
                            tableTypeSql += " and";
                        }
                    }
                    else
                    {
                        Log.Debug($"{logPrefix}: Ignoring unknown type:{assetType}");
                    }
                }
            }
            if (tableTypeSql.Length > 0)
            {
                if (tableNameSql.Length > 0)
                {
                    sql = $"{tableTypeSql} AND {tableNameSql}";
                }
                else
                {
                    sql = $"{tableTypeSql}";
                }
            }
            else if (tableNameSql.Length > 0)
            {
                sql = $"{tableNameSql}";
            }

            // 'WHERE' to be added by caller in case they need to add more 
            Log.Debug($"{logPrefix}: Returning SQL: {sql}");
            return sql;
        }
    }
}
