using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    public class StyleRule
    {
        /// <summary>
        /// Gets or sets the Style Rule Id
        /// </summary>
        [Column("id")]
        [JsonRequired]
        [JsonProperty("id", Required = Required.DisallowNull)]
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Layer Id
        /// </summary>
        [Column("layer_id")]
        [JsonIgnore]
        public required Guid LayerId { get; set; }

        /// <summary>
        /// Gets or sets the map mode
        /// </summary>
        [Column("map_mode")]
        [JsonProperty("mapMode", Required = Required.DisallowNull)]
        public string MapMode { get; set; } = "default";

        /// <summary>
        /// Gets or sets the evaluation priority
        /// </summary>
        [Column("evaluation_priority")]
        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public int? EvaluationPriority { get; set; }

        /// <summary>
        /// Gets or sets the welsh display name
        /// </summary>
        [Column("legend_display_name_cy")]
        [JsonIgnore]
        public string? LegendDisplayNameCym { get; set; }

        /// <summary>
        /// Gets or sets the english display name
        /// </summary>
        [Column("legend_display_name_eng")]
        [JsonIgnore]
        public required string LegendDisplayNameEng { get; set; }

        /// <summary>
        /// Gets the display names for the legend in multiple languages
        /// </summary>
        [JsonProperty("displayName")]
        public Dictionary<string, string?>? LegendDisplayName => new()
        {
            { "cy", LegendDisplayNameCym },
            { "en", LegendDisplayNameEng }
        };

        /// <summary>
        /// Gets or sets the geometry type for the legend
        /// </summary>
        [Column("legend_geometry_type")]
        [JsonProperty("legendGeometryType", NullValueHandling = NullValueHandling.Ignore)]
        public string? LegendGeometryType { get; set; }

        /// <summary>
        /// Gets or sets the display order for the legend
        /// </summary>
        [Column("legend_display_order")]
        [JsonProperty("legendDisplayOrder", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegendDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the attribute
        /// </summary>
        /// <remarks>
        /// This is potentially deprecated so needs to be checked and confirmed. Potentially replaced by style rule conditions.
        /// </remarks>
        [Column("attribute")]
        [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
        public string? Attribute { get; set; }

        /// <summary>
        /// Gets or sets the attribute values to match with
        /// </summary>
        /// <remarks>
        /// This is potentially deprecated so needs to be checked and confirmed. Potentially replaced by style rule conditions.
        /// </remarks>
        [JsonProperty("attributeValuesToMatch", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? AttributeValuesToMatch => string.IsNullOrEmpty(AttributeMatchValues) ? [] : AttributeMatchValues.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        /// <summary>
        /// Gets or sets the attribute values to match with as a comma separated string
        /// </summary>
        /// <remarks>
        /// This is to simplify retrieval from the database, processed for use in the AttributeValuesToMatch property.
        /// </remarks>
        [Column("attribute_values_to_match")]
        [JsonIgnore]
        public string? AttributeMatchValues { get; set; }

        /// <summary>
        /// Gets or sets the Style Id
        /// </summary>
        [Column("style_id")]
        [JsonRequired]
        [JsonProperty("styleId")]
        public required Guid StyleId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the selected style
        /// </summary>
        [Column("style_selected_id")]
        [JsonProperty("selectedStyleId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? StyleSelectedId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rotation property
        /// </summary>
        [Column("rotation_property_name")]
        [JsonProperty("rotationPropertyName", NullValueHandling = NullValueHandling.Ignore)]
        public string? RotationPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the rotation property units
        /// </summary>
        [Column("rotation_units_property_name")]
        [JsonProperty("rotationUnitsPropertyName", NullValueHandling = NullValueHandling.Ignore)]
        public string? RotationUnitsPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the conditions for the style rule
        /// </summary>
        [JsonProperty("conditions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StyleRuleCondition>? Conditions { get; set; } = new();
    }
}
