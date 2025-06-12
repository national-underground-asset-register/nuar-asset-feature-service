// <copyright file="StyleMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    /// <summary>
    /// Initialises the <see cref="EntityMap{Style}"/> and sets the mappings.
    /// </summary>
    internal class StyleMap : EntityMap<Style>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{Style}"/> and sets the mappings.
        /// </summary>
        internal StyleMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Name).ToColumn("name");
            Map(p => p.Description).ToColumn("description");
            Map(p => p.PrimaryGeometryType).ToColumn("primary_geometry_type");
            Map(p => p.ImageSource).ToColumn("fill_image_source");
            Map(p => p.SymbolName).ToColumn("symbol_name");
        }
    }
}
