// <copyright file="LayerGroupMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Models
{
    /// <summary>
    /// LayerGroupMap class.
    /// </summary>
    internal class LayerGroupMap : EntityMap<LayerGroup>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{LayerGroup}"/> and sets the mappings
        /// </summary>
        internal LayerGroupMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.DisplayNameCym).ToColumn("display_name_cym");
            Map(p => p.DisplayNameEng).ToColumn("display_name_eng");
            Map(p => p.DisplayOrder).ToColumn("display_order");
            Map(p => p.MapConfigId).ToColumn("map_config_id");
            Map(p => p.ParentLayerGroupId).ToColumn("parent_layer_group_id");
            Map(p => p.IsCheckedByDefault).ToColumn("is_checked_by_default");
            Map(p => p.IsBaseMap).ToColumn("is_base_map");
            Map(p => p.IsChild).ToColumn("is_child");
            Map(p => p.HasChildren).ToColumn("has_children");
        }
    }
}