// <copyright file="IAttributeGroupRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// IAttributeGroupRepository interface.
    /// </summary>
    public interface IAttributeGroupRepository
    {
        /// <summary>
        /// Gets all the attribute groups
        /// </summary>
        Task<IEnumerable<AttributeGroup>?> GetAll();

        /// <summary>
        /// Gets the attribute by attribute group ID
        /// </summary>
        /// <param name="attributeId">Attribute Group ID</param>
        Task<AttributeGroup?> GetByAttributeGroupId(Guid attributeId);

        /// <summary>
        /// Gets the attribute groups by map configuration ID
        /// </summary>
        /// <param name="mapConfigurationId">The unique map configuration identifier</param>
        /// <returns></returns>
        Task<IEnumerable<AttributeGroup>?> GetByMapConfigurationId(Guid mapConfigurationId);
    }
}