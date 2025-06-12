// <copyright file="StyleStrokeMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    /// <summary>
    /// Initialises the <see cref="EntityMap{Layer}"/> and sets the mappings.
    /// </summary>
    internal class StyleStrokeMap : EntityMap<StyleStroke>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{Layer}"/> and sets the mappings.
        /// </summary>
        internal StyleStrokeMap()
        {
            Map(p => p.Color).ToColumn("stroke_color");
            Map(p => p.LineDash).ToColumn("stroke_line_dash");
            Map(p => p.Width).ToColumn("stroke_width");
        }
    }
}
