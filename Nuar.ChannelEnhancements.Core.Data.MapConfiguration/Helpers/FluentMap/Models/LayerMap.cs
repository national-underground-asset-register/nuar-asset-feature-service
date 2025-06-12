// <copyright file="LayerMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Models
{
    /// <summary>
    /// Provides a mapping between snake-case column names and <see cref="Layer"/> properties
    /// </summary>
    internal class LayerMap : EntityMap<Layer>
    {   
        /// <summary>
        /// Initialises the <see cref="EntityMap{Layer}"/> and sets the mappings
        /// </summary>
        internal LayerMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Name).ToColumn("name");
            Map(p => p.Description).ToColumn("description");
            Map(p => p.DisplayNameCym).ToColumn("display_name_cym");
            Map(p => p.DisplayNameEng).ToColumn("display_name_eng");
            Map(p => p.DisplayOrder).ToColumn("display_order");
            Map(p => p.LayerGroupId).ToColumn("layer_group_id");
            Map(p => p.SourceType).ToColumn("source_type");
            Map(p => p.MinimumScale).ToColumn("minimum_scale");
            Map(p => p.MaximumScale).ToColumn("maximum_scale");
            Map(p => p.IsCheckedByDefault).ToColumn("is_checked_by_default");
        }
    }
}
