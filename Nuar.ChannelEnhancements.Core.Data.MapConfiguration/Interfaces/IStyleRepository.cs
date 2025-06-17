using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// Provides an interface for accessing Style data in the database.
    /// </summary>
    public interface IStyleRepository
    {
        /// <summary>
        /// Gets a <see cref="Style"/> by its unique identifier.
        /// </summary>
        /// <param name="styleId">The unique identifier for the style</param>
        public Task<Style?> GetById(Guid styleId);

        /// <summary>
        /// Gets all styles from the database.
        /// </summary>
        public Task<IEnumerable<Style?>?> GetAllStyles();

        /// <summary>
        /// Gets all styles from the database that are linked to the <see cref="Models.MapConfiguration"/> matching the given unique identifier.
        /// </summary>
        /// <param name="configId">The identifier of the map configuration to get styles for</param>
        public Task<IEnumerable<Style?>?> GetAllStylesByMapConfig(Guid configId);

    }
}