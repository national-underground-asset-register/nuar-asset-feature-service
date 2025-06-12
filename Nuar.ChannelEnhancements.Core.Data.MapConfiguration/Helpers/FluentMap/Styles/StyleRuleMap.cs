// <copyright file="StyleRuleMap.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    /// <summary>
    /// Initialises the <see cref="EntityMap{Layer}"/> and sets the mappings.
    /// </summary>
    internal class StyleRuleMap : EntityMap<StyleRule>
    {
        /// <summary>
        /// Initialises the <see cref="EntityMap{Layer}"/> and sets the mappings.
        /// </summary>
        internal StyleRuleMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.LayerId).ToColumn("layer_id");
            Map(p => p.MapMode).ToColumn("map_mode");
            Map(p => p.EvaluationPriority).ToColumn("evaluation_priority");
            Map(p => p.LegendDisplayNameCym).ToColumn("legend_display_name_cy");
            Map(p => p.LegendDisplayNameEng).ToColumn("legend_display_name_en");
            Map(p => p.LegendGeometryType).ToColumn("legend_geometry_type");
            Map(p => p.LegendDisplayOrder).ToColumn("legend_display_order");
            Map(p => p.Attribute).ToColumn("attribute");
            Map(p => p.AttributeMatchValues).ToColumn("attribute_values_to_match");
            Map(p => p.StyleId).ToColumn("style_id");
            Map(p => p.StyleSelectedId).ToColumn("style_selected_id");
            Map(p => p.RotationPropertyName).ToColumn("rotation_property_name");
            Map(p => p.RotationUnitsPropertyName).ToColumn("rotation_units_property_name");
        }
    }
}
