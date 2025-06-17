// <copyright file="IDataAccessLayer.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;

/// <summary>
/// A helper class that can be used to instantiate all the repositories in one place, this is useful for dependency injection.
/// </summary>
public interface IDataAccessLayer
{
    /// <summary>
    /// Gets the <see cref="IStyleRepository"/> repository instance.
    /// </summary>
    IStyleRepository StyleRepository { get; }

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