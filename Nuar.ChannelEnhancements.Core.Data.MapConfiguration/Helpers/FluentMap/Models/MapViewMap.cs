// <copyright file="MapViewMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Models
{
    /// <summary>
    /// Provides a mapping between snake-case column names and <see cref="MapView"/> properties
    /// </summary>
    internal class MapViewMap : EntityMap<MapView>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{MapView}"/> and sets the mappings
        /// </summary>
        internal MapViewMap()
        {
            Map(p => p.Id).ToColumn("map_view_id");
            Map(p => p.MapConfigId).ToColumn("map_config_id");
            Map(p => p.CenterX).ToColumn("center_x");
            Map(p => p.CenterY).ToColumn("center_y");
            Map(p => p.ExtentMinX).ToColumn("extent_bottom_left_x");
            Map(p => p.ExtentMinY).ToColumn("extent_bottom_left_y");
            Map(p => p.ExtentMaxX).ToColumn("extent_top_right_x");
            Map(p => p.ExtentMaxY).ToColumn("extent_top_right_y");
            Map(p => p.Zoom).ToColumn("initial_zoom");
            Map(p => p.MaximumZoom).ToColumn("maximum_zoom");
            Map(p => p.MinimumZoom).ToColumn("minimum_zoom");
            Map(p => p.Projection).ToColumn("projection");
        }
    }
}