
namespace Nuar.ChannelEnhancements.Core.Data.Features.Interfaces;

/// <summary>
/// This interface is used to get the tables for the given schema and asset types and names.
/// </summary>
public interface ITableInformationRepository
{
    /// <summary>
    /// Gets the tables for the given schema and asset types and names.
    /// </summary>
    /// <param name="schemaName">The schema name.</param>
    /// <param name="assetTypes">A list of asset types.</param>
    /// <param name="assetNames">A list of asset names.</param>
    /// <returns>A list of tables.</returns>
    public List<string>? GetTables(string schemaName, List<string>? assetTypes = null, List<string>? assetNames = null);

    // TEMP CODE UNTIL WE GET A FUNCTION
    /// <summary>
    /// Gets all the tables and schemas for tables and can cache it if appropriate
    /// </summary>
    /// <returns>A dictionary of Table name to schema</returns>
    public List<string>? GetAllTables(string schemaName);
}