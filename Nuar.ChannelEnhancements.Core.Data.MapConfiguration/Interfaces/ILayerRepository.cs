// <copyright file="ILayerRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// ILayerRepository interface.
    /// </summary>
    public interface ILayerRepository
    {
        /// <summary>
        /// Get Layers by the group Id
        /// </summary> 
        /// <param name="groupId">The id of the <see cref="LayerGroup"/> to get layers for</param>
        public Task<List<Layer>?> GetLayersByGroupId(Guid groupId);

        /// <summary>
        /// Gets a Layer using the provided map configuration and layer identifiers.
        /// </summary>
        /// <param name="layerId">The unique layer identifier</param>
        /// <param name="mapConfigId">The unique map configuration identifier</param>
        public Task<Layer?> GetLayerByMapConfigIdAndLayerId(Guid mapConfigId, Guid layerId);

        /// <summary>
        /// Gets all the layers for a given map config
        /// </summary>
        /// <param name="mapConfigId">Map config ID</param>
        public Task<List<Layer>?> GetLayersByMapConfigId(Guid mapConfigId);
    }
}