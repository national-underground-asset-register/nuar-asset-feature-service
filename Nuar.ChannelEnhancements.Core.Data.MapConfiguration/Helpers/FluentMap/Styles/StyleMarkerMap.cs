// <copyright file="StyleMarkerMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    /// <summary>
    /// Initialises the <see cref="EntityMap{StyleMarker}"/> and sets the mappings.
    /// </summary>
    internal class StyleMarkerMap : EntityMap<StyleMarker>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{StyleMarker}"/> and sets the mappings.
        /// </summary>
        internal StyleMarkerMap()
        {
            Map(p => p.ImageSrc).ToColumn("marker_image_source");
            Map(p => p.FillColor).ToColumn("marker_fill_color");
            Map(p => p.StrokeColor).ToColumn("marker_stroke_color");
            Map(p => p.StrokeWidth).ToColumn("marker_stroke_width");
            Map(p => p.Points).ToColumn("marker_points");
            Map(p => p.Radius).ToColumn("marker_radius");
            Map(p => p.Angle).ToColumn("marker_angle");
            Map(p => p.DisplacementX).ToColumn("marker_displacement_x");
            Map(p => p.DisplacementY).ToColumn("marker_displacement_y");
            Map(p => p.RotateWithView).ToColumn("marker_rotate_with_view");
            Map(p => p.Scale).ToColumn("marker_scale");
            Map(p => p.Rotation).ToColumn("marker_rotation");
        }
    }
}