// <copyright file="IAttributeRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// IAttributeRepository interface.
    /// </summary>
    public interface IAttributeRepository
    {
        /// <summary>
        /// Gets a list of all layer attributes for a specific layer using its unique identifier.
        /// </summary>
        /// <param name="layerId">The layer identifier</param>
        public Task<IEnumerable<LayerAttribute>?> GetAllByLayerId(Guid layerId);

        /// <summary>
        /// Gets all the attributes from the database
        /// </summary>
        public Task<IEnumerable<LayerAttribute>?> GetAll();
        
        /// <summary>
        /// Gets all the attributes that are queryable for a specific layer using its unique identifier.
        /// </summary>
        /// <param name="layerId">The unique identifier of the layer</param>
        /// <returns>List of LayerAttribute</returns>
        public Task<IEnumerable<LayerAttribute>?> GetAllQueryableByLayerId(Guid layerId);
    }
}