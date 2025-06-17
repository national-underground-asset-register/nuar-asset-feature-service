using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers
{
    public class LayerGroup
    {
        /// <summary>
        /// Gets or sets the unique Id for layer group
        /// </summary>
        [Column("id")]
        [JsonRequired]
        [JsonProperty("id", Required = Required.DisallowNull)]
        public Guid Id { get; set; }

        /// <summary>
        ///Gets or sets the display name in welsh
        /// </summary>
        [Column("display_name_cym")]
        [JsonIgnore]
        public string? DisplayNameCym { get; set; }

        /// <summary>
        /// Gets or sets the display name in english
        /// </summary>
        /// <remarks>
        /// This is required so we can always generate a valid display name JSON object
        /// </remarks>
        [Column("display_name_eng")]
        [JsonIgnore]
        public required string DisplayNameEng { get; set; }

        /// <summary>
        /// Gets the display names in multiple languages
        /// </summary>
        /// <remarks>
        /// A display name is always required so by default the English name is used.
        /// </remarks>
        [JsonProperty("displayName")]
        public Dictionary<string, string?> DisplayName => new()
        {
            { "cy", DisplayNameCym },
            { "en", DisplayNameEng }
        };

        /// <summary>
        /// Gets or sets the order for displaying groups
        /// </summary>
        [Column("display_order")]
        [JsonProperty("displayOrder", Required = Required.DisallowNull)]
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the  map configuration class list
        /// </summary>
        [Column("map_config_id")]
        [JsonIgnore]
        public Guid MapConfigId { get; set; }

        /// <summary>
        /// Gets or sets the parent layer group Id
        /// </summary>
        [Column("parent_layer_group_id")]
        [JsonProperty(PropertyName = "parentId", NullValueHandling = NullValueHandling.Include)]
        public Guid? ParentLayerGroupId { get; set; }

        /// <summary>
        /// Gets or sets whether the layer group is checked by default
        /// </summary>
        /// <remarks>
        /// This is primarily designed for user interface purposes, allowing certain layer groups to be pre-selected when the map configuration is loaded.
        /// </remarks>
        [Column("is_checked_by_default")]
        [JsonProperty("isCheckedByDefault", Required = Required.DisallowNull)]
        public bool IsCheckedByDefault { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the layer group has child layers or groups
        /// </summary>
        [Column("has_children")]
        [JsonProperty("hasChildren", Required = Required.DisallowNull)]
        public bool HasChildren { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the layer group is a child of another layer group
        /// </summary>
        [Column("is_child")]
        [JsonProperty("isChild", Required = Required.DisallowNull)]
        public bool IsChild { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the layer group is a base map group
        /// </summary>
        [Column("is_base_map")]
        [JsonProperty("isBaseMap", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsBaseMap { get; set; } = false;
    }
}
