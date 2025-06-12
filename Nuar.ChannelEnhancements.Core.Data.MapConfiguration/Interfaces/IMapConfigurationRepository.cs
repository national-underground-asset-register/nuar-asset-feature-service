// <copyright file="IMapConfigRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// Map Configuration Repository.
    /// </summary>
    public interface IMapConfigurationRepository
    {
        /// <summary>
        /// Get all map configuration
        /// </summary>
        /// <returns>The map config.</returns>
        public Task<IEnumerable<Models.MapConfiguration>?> GetAll();

        /// <summary>
        /// Get all map configuration for a given Id
        /// </summary>
        /// <returns>The map config.</returns>
        public Task<Models.MapConfiguration?> GetMapConfigById(Guid Id);
    }
}