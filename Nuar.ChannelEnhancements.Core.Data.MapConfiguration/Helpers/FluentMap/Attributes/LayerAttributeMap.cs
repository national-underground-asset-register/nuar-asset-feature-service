// <copyright file="AttributeGroupMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Attributes
{
    /// <summary>
    /// LayerAttributeMap class.
    /// </summary>
    internal class LayerAttributeMap : EntityMap<LayerAttribute>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{LayerAttribute}"/> and sets the mappings
        /// </summary>
        internal LayerAttributeMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.AttributeName).ToColumn("attribute_name");
            Map(p => p.AttributeSuffix).ToColumn("attribute_suffix");
            Map(p => p.DefaultValue).ToColumn("default_value");
            Map(p => p.DisplayNameCym).ToColumn("display_name_cym");
            Map(p => p.DisplayNameEng).ToColumn("display_name_eng");
            Map(p => p.IsVisible).ToColumn("is_visible");
            Map(p => p.LayerId).ToColumn("layer_id");
            Map(p => p.AttributeGroupId).ToColumn("attribute_group_id");
            Map(p => p.DisplayOrder).ToColumn("display_order");
            Map(p => p.IsQueryable).ToColumn("is_queryable");
        }
    }
}
