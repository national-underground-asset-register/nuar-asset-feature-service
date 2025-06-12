using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers
{
    public class Layer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the layer.
        /// </summary>
        [Column("")]
        [JsonRequired]
        [JsonProperty("id", Required = Required.Always)]
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the layer group Id.
        /// </summary>
        [Column("")]
        [JsonProperty(PropertyName = "layerGroup")]
        public Guid LayerGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        [Column("")]
        [JsonProperty(PropertyName = "assetType")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name in welsh
        /// </summary>
        [Column("")]
        [JsonIgnore]
        public string? DisplayNameCym { get; set; }

        /// <summary>
        /// Gets or sets the display name in english
        /// </summary>
        /// <remarks>
        /// This is required so we can always generate a valid display name JSON object
        /// </remarks>
        [Column("")]
        [JsonIgnore]
        public required string DisplayNameEng { get; set; }

        /// <summary>
        /// Gets or sets the display names in multiple languages.
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
        /// Gets or sets the description for the layer.
        /// </summary>
        [Column("")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the order this is displayed in
        /// </summary>
        [Column("")]
        [JsonRequired]
        [JsonProperty("displayOrder", Required = Required.DisallowNull)]
        public required int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the source type
        /// </summary>
        [Column("")]
        [JsonRequired]
        [JsonProperty("sourceType", Required = Required.DisallowNull)]
        public required string SourceType { get; set; }

        /// <summary>
        /// Gets or sets the set of source properties that control how the layer is loaded into OpenLayers
        /// </summary>
        /// <remarks>
        /// Each key is a property name and the value is the property value. Properties can be any valid OpenLayers source property for the type
        /// of data being loaded. For example, for a WMS layer, you might have properties like "url", "params", etc.
        /// </remarks>
        [Column("")]
        [JsonProperty("sourceProperties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string>? SourceProperties { get; set; }

        /// <summary>
        /// Gets or sets the minimum scale for layer display
        /// </summary>
        [Column("")]
        [JsonProperty("minimumScale", NullValueHandling = NullValueHandling.Ignore)]
        public int MinimumScale { get; set; }

        /// <summary>
        /// Gets or sets the maximum scale for layer display
        /// </summary>
        [Column("")]
        [JsonProperty("maximumScale", NullValueHandling = NullValueHandling.Ignore)]
        public int MaximumScale { get; set; }

        /// <summary>
        /// Gets or sets whether the layer is selected by default
        /// </summary>
        [Column("")]
        [JsonProperty("isCheckedByDefault", Required = Required.DisallowNull)]
        public bool IsCheckedByDefault { get; set; } = false;

        /// <summary>
        /// Gets or sets the default style Id for the layer
        /// </summary>
        /// <remarks>
        /// This could be null if the layer source does not have attributes, for example raster layer sources
        /// </remarks>
        [Column("")]
        [JsonProperty("styleRules", NullValueHandling = NullValueHandling.Ignore)]
        public List<Styles.StyleRule>? StyleRules { get; set; } = new List<Styles.StyleRule>();

        /// <summary>
        /// Gets or sets the list of attributes for this layer
        /// </summary>
        /// <remarks>
        /// This could be null if the layer source does not have attributes, for example raster layer sources
        /// </remarks>
        [Column("")]
        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Attributes.LayerAttribute>? Attributes { get; set; }
    }
}
