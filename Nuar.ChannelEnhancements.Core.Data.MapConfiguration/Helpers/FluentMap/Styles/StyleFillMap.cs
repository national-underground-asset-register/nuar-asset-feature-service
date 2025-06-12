// <copyright file="StyleFillMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    /// <summary>
    /// Initialises the <see cref="EntityMap{StyleFill}"/> and sets the mappings.
    /// </summary>
    internal class StyleFillMap : EntityMap<StyleFill>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{StyleFill}"/> and sets the mappings.
        /// </summary>
        internal StyleFillMap()
        {
            Map(p => p.FillColor).ToColumn("fill_color");
            Map(p => p.ImageSource).ToColumn("fill_image_source");
        }
    }
}
