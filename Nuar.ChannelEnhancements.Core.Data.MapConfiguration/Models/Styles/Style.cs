namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Style
    {
        /// <summary>
        /// Gets or sets the style identifier
        /// </summary>
        [JsonRequired]
        [JsonProperty("id", Required = Required.DisallowNull)]
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the style
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the style
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the geometry type the style applies to
        /// </summary>
        [JsonProperty("geometryType")]
        public string? PrimaryGeometryType { get; set; }

        /// <summary>
        /// Gets or sets the symbol name 
        /// </summary>
        [JsonProperty("symbolName", NullValueHandling = NullValueHandling.Ignore)]
        public string? SymbolName { get; set; }

        /// <summary>
        /// Gets or sets the image source
        /// </summary>
        [JsonProperty("imageSource", NullValueHandling = NullValueHandling.Ignore)]
        public string? ImageSource { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="StyleFill"/> to describe the fill style for the geometry
        /// </summary>
        [JsonProperty("fill", NullValueHandling = NullValueHandling.Ignore)]
        public StyleFill? Fill { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="StyleStroke"/> to describe the stroke style for the geometry
        /// </summary>
        [JsonProperty("stroke", NullValueHandling = NullValueHandling.Ignore)]
        public StyleStroke? Stroke { get; set; }

        /// <summary>
        /// Calls the StyleMarker class to get Id
        /// </summary>
        /// <remarks>
        /// This is called image in OpenLayers so the JSON property name is set to reflect this
        /// </remarks>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public StyleMarker? Marker { get; set; }
    }
}
