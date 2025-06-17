namespace Nuar.ChannelEnhancements.Core.Data.Features.Interfaces;

using NetTopologySuite.Features;

/// <summary>
/// This interface is used to get the features for the given collection and feature id.
/// </summary>
public interface IFeaturesRepository
{
    /// <summary>
    /// Gets or sets a list of column names that will be used to generate attributes on returned features.
    /// </summary>
    List<string> ColumnNames { get; set; }

    /// <summary>
    /// Gets or sets the name of the column that holds the feature geometry.
    /// </summary>
    string GeometryColumnName { get; set; }

    /// <summary>
    /// Gets or sets the name of the column that is the unique identifier for a feature.
    /// </summary>
    string FeatureIdColumnName { get; set; }

    /// <summary>
    /// Gets or sets the name of the database schema where the tables can be found.
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// Gets a feature by its unique identifier.
    /// </summary>
    /// <param name="collectionId">The name or id of the collection the feature belongs to.</param>
    /// <param name="id">The unique identifier of the feature.</param>
    /// <returns><see cref="Feature"/></returns>
    Feature? GetFeature(string collectionId, string id);

    /// <summary>
    /// Gets a collection of features limited by page size and number.
    /// </summary>
    /// <param name="collectionId">The name or id of the collection to get features from.</param>
    /// <param name="pageNum">The page number to fetch.</param>
    /// <param name="pageSize">The total number of features in the page</param>
    /// <param name="sqlWhereFilter">The optional additional filter string to be added to a WHERE clause</param>
    /// <returns><see cref="FeatureCollection"/></returns>
    FeatureCollection? GetFeatureCollection(string collectionId, int pageNum, int pageSize, string sqlWhereFilter );

    /// <summary>
    /// Gets a collection of features limited by a spatial extent.
    /// </summary>
    /// <param name="collectionId">The name or id of the collection to get features from.</param>
    /// <param name="minX">The minimum X coordinate of the extent.</param>
    /// <param name="maxX">The maximum X coordinate of the extent.</param>
    /// <param name="minY">The minimum Y coordinate of the extent.</param>
    /// <param name="maxY">The maximum Y coordinate of the extent.</param>
    /// <param name="sqlWhereFilter">The additional filter string to be added to a WHERE clause</param>
    /// <returns><see cref="FeatureCollection"/></returns>
    FeatureCollection? GetFeatureCollection(string collectionId, double minX, double maxX, double minY, double maxY, string sqlWhereFilter = "");

    /// <summary>
    /// Gets a collection of features limited by a spatial extent and
    /// </summary>
    /// <param name="collectionId">The name or id of the collection to get features from.</param>
    /// <param name="minX">The minimum X coordinate of the extent.</param>
    /// <param name="maxX">The maximum X coordinate of the extent.</param>
    /// <param name="minY">The minimum Y coordinate of the extent.</param>
    /// <param name="maxY">The maximum Y coordinate of the extent.</param>
    /// <param name="pageNum">The page number to fetch.</param>
    /// <param name="pageSize">The total number of features in the page</param>
    /// <param name="sqlWhereFilter">The additional filter string to be added to a WHERE clause</param>
    /// <returns><see cref="FeatureCollection"/></returns>
    FeatureCollection? GetFeatureCollection(string collectionId, double minX, double maxX, double minY, double maxY,
        int pageNum, int pageSize, string sqlWhereFilter);

    /// <summary>
    /// This function will get the data types for the given feature 
    /// </summary>
    /// <param name="feature">The name of the Feature to retrieve data types for </param>
    /// <returns>dictionary of column to datatype</returns>
    Dictionary<string, string> GetFeatureDataTypes(string feature);
}