// <copyright file="DataAccessLayer.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

using Interfaces;
using Models;
using Repositories;

/// <inheritdoc />
public class DataAccessLayer : IDataAccessLayer
{
    /// <summary>
    /// Private instance of the database configuration settings
    /// </summary>
    private readonly DatabaseConfigurationSettings _dbSettings;

    /// <summary>
    /// DataAccessLayer constructor.
    /// </summary>
    /// <param name="dbSettings">A <see cref="DatabaseConfigurationSettings"/> object with database connection details</param>
    public DataAccessLayer(DatabaseConfigurationSettings dbSettings)
    {
        // Set the database connection settings
        _dbSettings = dbSettings;

        // Create the repository instances
        StyleRepository = new StyleRepository(dbSettings);
        StyleRuleRepository = new StyleRuleRepository(dbSettings);
        AttributeGroupRepository = new AttributeGroupRepository(dbSettings);
        AttributeRepository = new AttributeRepository(dbSettings);
        LayerGroupRepository = new LayerGroupRepository(dbSettings);
        LayerRepository = new LayerRepository(dbSettings, AttributeRepository, StyleRuleRepository);
        MapViewRepository = new MapViewRepository(dbSettings);
        MapConfigurationRepository = new MapConfigurationRepository(dbSettings, MapViewRepository, LayerGroupRepository, LayerRepository, AttributeGroupRepository, StyleRepository);
    }

    /// <inheritdoc/>
    public IStyleRepository StyleRepository { get; set; }

    /// <inheritdoc/>
    public IStyleRuleRepository StyleRuleRepository { get; set; }

    /// <inheritdoc/>
    public IAttributeGroupRepository AttributeGroupRepository { get; set; }

    /// <inheritdoc/>
    public IAttributeRepository AttributeRepository { get; set; }

    /// <inheritdoc/>
    public ILayerGroupRepository LayerGroupRepository { get; set; }

    /// <inheritdoc/>
    public ILayerRepository LayerRepository { get; set; }

    /// <inheritdoc/>
    public IMapViewRepository MapViewRepository { get; set; }

    /// <inheritdoc/>
    public IMapConfigurationRepository MapConfigurationRepository { get; set; }
}