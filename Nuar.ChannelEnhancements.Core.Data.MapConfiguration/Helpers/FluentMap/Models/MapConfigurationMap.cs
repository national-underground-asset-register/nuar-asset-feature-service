// <copyright file="MapConfigurationMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Models
{
    /// <summary>
    /// Provides a mapping between snake-case column names and <see cref="MapConfiguration"/> properties
    /// </summary>
    internal class MapConfigurationMap : EntityMap<MapConfiguration.Models.MapConfiguration>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{MapConfiguration}"/> and sets the mappings
        /// </summary>
        internal MapConfigurationMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Description).ToColumn("description");
            Map(p => p.Name).ToColumn("name");
            Map(p => p.MapViewId).ToColumn("map_view_id");
            Map(p => p.IsActive).ToColumn("is_active");
        }
    }
}