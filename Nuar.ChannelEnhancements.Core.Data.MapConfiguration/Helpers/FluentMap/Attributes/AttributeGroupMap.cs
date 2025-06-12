// <copyright file="AttributeGroupMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Attributes
{
    /// <summary>
    /// AttributeGroupMap class.
    /// </summary>
    internal class AttributeGroupMap : EntityMap<AttributeGroup>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{AttributeGroup}"/> and sets the mappings
        /// </summary>
        internal AttributeGroupMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.DisplayNameCym).ToColumn("display_name_cym");
            Map(p => p.DisplayNameEng).ToColumn("display_name_eng");
            Map(p => p.DisplaySection).ToColumn("display_section");
            Map(p => p.DisplayOrder).ToColumn("display_order");
            Map(p => p.IsVisible).ToColumn("is_visible");
            Map(p => p.Description).ToColumn("description");
        }
    }
}
