using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// This interface defines the methods for accessing map views in the data store.
    /// </summary>
    public interface IMapViewRepository
    {
        /// <summary>
        /// Gets the <see cref="MapView"/> belonging to the <see cref="MapConfiguration"/> matching the provided identifier.
        /// </summary>
        /// <param name="configId">The unique identifier of the map configuration</param>
        public Task<MapView?> GetMapViewByMapConfigId(Guid configId);
    }
}