// <copyright file="IDataAccessLayer.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IDataAccessLayer
    {
        /// <summary>
        /// Gets the <see cref="IStyleRepository"/> repository instance.
        /// </summary>
        IStyleRepository StyleFillRepository { get; }

        /// <summary>
        /// Gets the <see cref="IStyleRuleRepository"/> repository instance.
        /// </summary>
        IStyleRuleRepository StyleRuleRepository { get; }

        /// <summary>
        /// Gets the <see cref="IAttributeGroupRepository"/> repository instance.
        /// </summary>
        IAttributeGroupRepository AttributeGroupRepository { get; }

        /// <summary>
        /// Gets the <see cref="IAttributeRepository"/> repository instance.
        /// </summary>
        IAttributeRepository AttributeRepository { get; }

        /// <summary>
        /// Gets the <see cref="ILayerGroupRepository"/> repository instance.
        /// </summary>
        ILayerGroupRepository LayerGroupRepository { get; }

        /// <summary>
        /// Gets the <see cref="ILayerRepository"/> repository instance.
        /// </summary>
        ILayerRepository LayerRepository { get; }

        /// <summary>
        /// Gets the <see cref="IMapViewRepository"/> repository instance.
        /// </summary>
        IMapViewRepository MapViewRepository { get; }

        /// <summary>
        /// Gets the <see cref="IMapConfigurationRepository"/> repository instance.
        /// </summary>
        IMapConfigurationRepository MapConfigurationRepository { get; }
    }
}