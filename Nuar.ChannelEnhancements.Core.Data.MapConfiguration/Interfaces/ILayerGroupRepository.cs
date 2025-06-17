// <copyright file="ILayerGroupRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// ILayerGroupRepository interface.
    /// </summary>
    public interface ILayerGroupRepository
    {
        /// <summary>
        /// Get Layer Group by Map config Id
        /// <param name="configId">map config ID</param>
        /// </summary> 
        public Task<IEnumerable<LayerGroup>?> GetLayerGroupsByMapConfigId(Guid configId);

        /// <summary>
        /// Gets the layer groups given a map config and layer group ID
        /// </summary>
        /// <param name="configId">map config ID</param>
        /// <param name="layerGroupId">layer group id</param>
        public Task<LayerGroup?> GetLayerGroupById(Guid configId, Guid layerGroupId);

        /// <summary>
        /// Gets the layer groups that are children of the <see cref="LayerGroup"/> matching the provided identifier.
        /// </summary>
        /// <param name="parentGroupId">The unique identifier of the parent <see cref="LayerGroup"/></param>
        /// <returns></returns>
        public Task<IEnumerable<LayerGroup>?> GetChildLayerGroups(Guid parentGroupId);
    }
}