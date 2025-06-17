using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Nuar.ChannelEnhancements.Core.Data.Features;

using System.Data;
using Npgsql;
using Dapper;
using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.Features.Interfaces;

public class FeatureRepository : QueryBase, IFeaturesRepository
{
    private readonly JsonSerializer _geoJsonSerialiser;

    /// <inheritdoc />
    public List<string> ColumnNames { get; set; }

    /// <inheritdoc />
    public string GeometryColumnName { get; set; }

    /// <inheritdoc />
    public string FeatureIdColumnName { get; set; }

    /// <inheritdoc />
    public string SchemaName { get; set; }

    /// <summary>
    /// FeatureRepository constructor.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    public FeatureRepository(string connectionString) : base(connectionString)
    {
        // TODO: Add DatabaseConfigurationSettings to the constructor to allow for more flexible configuration

        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Initialise the column names, geometry column name and feature Id column name
        ColumnNames = new List<string>();
        GeometryColumnName = "geometry";
        FeatureIdColumnName = string.Empty;

        // Set the schema name to the default schema for Nuar data
        SchemaName = "nuardata";

        // Create a GeoJsonSerialiser that we can use to serialise and deserialise geometries
        _geoJsonSerialiser = GeoJsonSerializer.Create();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <param name="logConnectionsToDb">The log database connection.</param>
    /// <param name="geometryColumnName">The column name of the geometry column to use</param>
    /// <param name="featureIdColumnName">The feature Id colum name to use</param>
    /// <param name="columnNames">A list of column names used for searches</param>
    public FeatureRepository(string connectionString, bool logConnectionsToDb, 
        string geometryColumnName, string featureIdColumnName, List<string> columnNames) 
        : base(connectionString)
    {
        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Initialise the column names, geometry column name and feature Id column name
        ColumnNames = columnNames;
        GeometryColumnName = geometryColumnName;
        FeatureIdColumnName = featureIdColumnName;
        SchemaName = "nuardata";

        // Create a GeoJsonSerialiser that we can use to serialise and deserialise geometries
        _geoJsonSerialiser = GeoJsonSerializer.Create();
    }

    /// <inheritdoc />
    public Feature? GetFeature(string collectionId, string id)
    {
        // Make sure the table name exists and is sql injection safe
        if (!TableNameExists(collectionId)) throw new Exception("Could not find table with the given name in the database.", new ArgumentException($"Argument Value: {collectionId}\r\nEither a table with the name does not exist or this is a potential SQL injection attack.", "collectionId"));

        // Make sure the geometry column is not included in the list of attribute columns
        string columnNames = string.Join(",",
            ColumnNames.Where(c => c != GeometryColumnName).Select(n => n).ToArray());
        if (columnNames == string.Empty)
        {
            throw new Exception("No columnNames for Geometry", new ArgumentException($"Argument Value: {collectionId}\r\nUnknown Error", "collectionId"));
        }

        var sqlVar = $"SELECT {FeatureIdColumnName}, ST_AsGeoJson({GeometryColumnName}) AS geom_geojson FROM {SchemaName}.{collectionId} where {FeatureIdColumnName} = '{id}'";

        // Query the database for the feature
        var p = new DynamicParameters();
        p.Add("@feature_id", id, DbType.String, ParameterDirection.Input);
        var result = (Connection != null) ? Connection.Query(
            sql: sqlVar,
            param: p
        ).Cast<IDictionary<string, object>>().Select(it => it.ToDictionary(it => it.Key, it => it.Value)).FirstOrDefault<Dictionary<string, object>>() : throw new Exception("Failed to establish database connection.");

        Feature? feature = null;

        if (result == null) return feature;

        // Create a new feature for the result
        feature = new Feature();

        // Get the geometry from the dictionary
        using (StringReader reader = new StringReader(result["geom_geojson"].ToString()!))
        using (JsonReader jsonReader = new JsonTextReader(reader))
        {
            feature.Geometry = _geoJsonSerialiser.Deserialize<Geometry>(jsonReader);
        }

        // We don't want geom_geojson to now be a property so we remove it from the dictionary now we have extracted it
        result.Remove("geom_geojson");

        // The remaining dictionary now becomes the attributes for the feature
        feature.Attributes = new AttributesTable(result);
        feature.BoundingBox = new Envelope();
        return feature;
    }

    /// <inheritdoc />
    public FeatureCollection? GetFeatureCollection(string collectionId, int pageNum, int pageSize, string sqlWhereFilter)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public FeatureCollection? GetFeatureCollection(string collectionId, double minX, double minY, double maxX, double maxY, string sqlWhereFilter = "")
    {
        // Make sure the table name exists and is sql injection safe
        if (!TableNameExists(collectionId)) throw new Exception("Could not find table with the given name in the database.", new ArgumentException($"Argument Value: {collectionId}\r\nEither a table with the name does not exist or this is a potential SQL injection attack.", "collectionId"));


        // Make sure the geometry column is not included in the list of attribute columns
        string columnNames = string.Join(",",
            ColumnNames.Where(c => c != GeometryColumnName).Select(n => n).ToArray());

        // Query the database for the feature
        var p = new DynamicParameters();
        p.Add("@minx", minX, DbType.VarNumeric, ParameterDirection.Input);
        p.Add("@maxx", maxX, DbType.VarNumeric, ParameterDirection.Input);
        p.Add("@miny", minY, DbType.VarNumeric, ParameterDirection.Input);
        p.Add("@maxy", maxY, DbType.VarNumeric, ParameterDirection.Input);
        p.Add("@srid", 27700, DbType.Int16, ParameterDirection.Input);

        string sql =
            $"SELECT {columnNames}, ST_AsGeoJson(ST_Intersection({GeometryColumnName}, ST_MakeEnvelope({minX}, {minY}, {maxX}, {maxY}, {27700}))) AS geom_geojson FROM {SchemaName}.{collectionId} WHERE {GeometryColumnName} && ST_MakeEnvelope({minX}, {minY}, {maxX}, {maxY}, {27700})";

        // If a filter has been added append it to the end of the WHERE clause
        if (!string.IsNullOrEmpty(sqlWhereFilter))
        {
            sql = $"{sql} AND {sqlWhereFilter}";
        }

        var result = (Connection != null) ? Connection.Query(
            sql: sql,
            param: p
        ).Cast<IDictionary<string, object>>().Select(it => it.ToDictionary(it => it.Key, it => it.Value)) : throw new Exception("Failed to establish database connection.");

        // Convert the results to a FeatureCollection
        FeatureCollection? featureCollection = GetFeatureCollection(result);

        // If we have anything in the collection, set the bounding box params, otherwise do not bother and return null
        if (featureCollection.Count > 0)
        {
            featureCollection.BoundingBox = new Envelope(new Coordinate(minX, minY), new Coordinate(maxX, maxY));
        }
        else
        {
            featureCollection = null;
        }
        // Return the result
        return featureCollection;
    }

    /// <inheritdoc />
    public FeatureCollection? GetFeatureCollection(string collectionId, double minX, double minY, double maxX, double maxY,
        int pageNum, int pageSize, string sqlWhereFilter = "")
    {
        // <TODO> Paging will be implemented on another Sprint
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a list of <see cref="Dictionary{TKey,TValue}"/> to a <see cref="FeatureCollection"/>
    /// </summary>
    /// <param name="items">The list of <see cref="Dictionary{TKey,TValue}"/> to convert.</param>
    /// <returns><see cref="FeatureCollection"/></returns>
    /// <remarks>Assumes the items contains an element with a key of geom_geojson and the value being GeoJSON geometry.</remarks>
    private FeatureCollection GetFeatureCollection(IEnumerable<Dictionary<string, object>> items)
    {
        // Create the FeatureCollection to populate
        FeatureCollection featureCollection = new();

        // Loop over the results and create a feature for each one, splitting out the properties from the geometry
        foreach (var item in items)
        {
            // Create a new feature for the result
            Feature feature = new Feature();

            // Get the geometry from the dictionary
            using (StringReader reader = new (item["geom_geojson"].ToString()!))
            using (JsonReader jsonReader = new JsonTextReader(reader))
            {
                feature.Geometry = _geoJsonSerialiser.Deserialize<Geometry>(jsonReader);
            }

            // We don't want geom_geojson to now be a property so we remove it from the dictionary now we have extracted it
            item.Remove("geom_geojson");

            // The remaining dictionary now becomes the attributes for the feature
            feature.Attributes = new AttributesTable(item);

            // Add the created feature to the collection that will be returned
            featureCollection.Add(feature);
        }

        // Return the processed collection
        return featureCollection;
    }

    /// <summary>
    /// Gets a <see cref="bool"/> indicating if the given <paramref name="tablename"/> exists.
    /// </summary>
    /// <param name="tableName">The name of the table to check the existence of</param>
    /// <returns>True if the table exists, otherwise False.</returns>
    /// <remarks>
    /// Use this function to check if a table exists in a SQL injection safe way.  If the provided table name is a valid string and not malicious SQL then the return
    /// will be true (assuming the table actually exists).  This will then mean the string can be used in an interpolated select statement safely.  Of course, its also
    /// a useful function to check if a table actually exists in the database!
    /// </remarks>
    private bool TableNameExists(string tableName)
    {
        // Define parameters to inject in the SQL
        var p = new DynamicParameters();
        p.Add("@tablename", tableName, DbType.String, ParameterDirection.Input);
        p.Add("@schemaname", SchemaName, DbType.String, ParameterDirection.Input);

        // Construct parameterised SQL statement to lookup the table in the information schema
        string sql = "SELECT count(1) FROM information_schema.tables WHERE table_name = @tablename AND table_schema = @schemaname";

        // Execute the query and if we get 1 as the result then this table exists in the database
        return (Connection != null) ? Connection.QuerySingle<int>(sql, p) == 1 : throw new Exception("Failed to establish database connection.");
    }

    ///<inheritdoc />
    public Dictionary<string, string> GetFeatureDataTypes(string feature)
    {
        // Make sure the table name exists and is sql injection safe
        if (!TableNameExists(feature)) throw new Exception("Could not find table with the given name in the database.", new ArgumentException($"Argument Value: {feature}\r\nEither a table with the name does not exist or this is a potential SQL injection attack.", "feature"));

        // Define parameters to inject in the SQL
        var p = new DynamicParameters();
        p.Add("@tablename", feature, DbType.String, ParameterDirection.Input);
        p.Add("@schemaname", SchemaName, DbType.String, ParameterDirection.Input);

        // Construct parameterised SQL statement to lookup the table in the information schema
        string sql = "SELECT column_name, data_type FROM information_schema.columns WHERE table_name = @tablename AND table_schema = @schemaname";

        // Execute the query and if we get 1 as the result then this table exists in the database
        var results = (Connection != null) ? Connection.Query(sql, p).ToList() : throw new Exception("Failed to establish database connection.");
        return results.ToDictionary(row => (string)row.column_name, row => (string)row.data_type);
    }
}