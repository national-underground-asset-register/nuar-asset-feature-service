namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models
{
    public class DatabaseConfigurationSettings
    {
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the schema that contains the map configuration data
        /// </summary>
        public string MapConfigurationSchemaName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dictionary of function names that are used to query the map configuration data.
        /// </summary>
        /// <remarks>The key is the platform type.</remarks>
        public Dictionary<string, string> MapConfigurationFunctionMap { get; set; } = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);
    }
}
