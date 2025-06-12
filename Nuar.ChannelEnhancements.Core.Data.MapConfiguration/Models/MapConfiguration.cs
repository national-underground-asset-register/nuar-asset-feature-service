namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models
{
    /// <summary>
    /// A <see cref="MapConfiguration"/> class which is the root element of a complete map configuration object
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Include)]
    public class MapConfiguration
    {
        /// <summary>
        /// Gets or sets the unique Id for Map Configuration
        /// </summary>
        [JsonRequired]
        [JsonProperty("id", Required = Required.Always)]
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of map configuration
        /// </summary>
        [JsonRequired]
        [JsonProperty("name", Required = Required.Always)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of map configuration
        /// </summary>
        [JsonRequired]
        [JsonProperty("description", Required = Required.Always)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the map view Id
        /// </summary>
        [JsonIgnore]
        public Guid MapViewId { get; set; }

        /// <summary>
        /// Gets or sets whether the map configuration is active
        /// </summary>
        [JsonIgnore]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MapView"/> for the map configuration
        /// </summary>
        [JsonRequired]
        [JsonProperty("mapView", Required = Required.Always)]
        public required MapView MapView { get; set; }

        /// <summary>
        /// Gets or sets the list of layer groups in the map configuration
        /// </summary>
        [JsonProperty("layerGroups", NullValueHandling = NullValueHandling.Ignore)]
        public List<Layers.LayerGroup> LayerGroups { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of layers in the map configuration
        /// </summary>
        [JsonProperty("layers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Layers.Layer> Layers { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of base maps in the map configuration
        /// </summary>
        /// <remarks>
        /// Base maps are really just <see cref="Layers.Layer"/> objects that are used as the base layer of the map. They can be fetched from the LayerGroup that has the IsBaseMap flag set to true.
        /// </remarks>
        [JsonProperty("baseMaps", NullValueHandling = NullValueHandling.Ignore)]
        public List<Layers.Layer>? BaseMaps { get; set; }

        /// <summary>
        /// Gets or sets the list of styles in the map configuration
        /// </summary>
        [JsonProperty("styles", NullValueHandling = NullValueHandling.Ignore)]
        public List<Styles.Style>? Styles { get; set; }

        /// <summary>
        /// Gets or sets the list of attribute groups in the map configuration
        /// </summary>
        [JsonProperty("attributeGroups", NullValueHandling = NullValueHandling.Ignore)]
        public List<Attributes.AttributeGroup>? AttributeGroups { get; set; }
    }
}
