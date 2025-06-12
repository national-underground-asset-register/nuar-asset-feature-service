// <copyright file="DataAccessLayer.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration
{
    using Interfaces;
    using Models;

    /// <summary>
    /// Unit of work implementation.
    /// </summary>
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
            StyleFillRepository = new StyleRepository()
        }

        /// <inheritdoc/>
        public IStyleRepository StyleFillRepository { get; set; }

        /// <inheritdoc/>
        public IStyleRuleRepository StyleRuleRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public IAttributeGroupRepository AttributeGroupRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public IAttributeRepository AttributeRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public ILayerGroupRepository LayerGroupRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public ILayerRepository LayerRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public IMapViewRepository MapViewRepository => throw new NotImplementedException();

        /// <inheritdoc/>
        public IMapConfigurationRepository MapConfigurationRepository => throw new NotImplementedException();
    }
}
