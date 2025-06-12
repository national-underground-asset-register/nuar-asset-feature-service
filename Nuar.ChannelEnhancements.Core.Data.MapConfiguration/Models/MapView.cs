namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models
{
    public class MapView
    {
        /// <summary>
        /// Gets or sets the unique identifier for the map view
        /// </summary>
        [JsonIgnore]
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the map configuration this map view belongs to
        /// </summary>
        [JsonIgnore]
        public Guid? MapConfigId { get; set; }

        /// <summary>
        /// Gets or sets the projection of map view 
        /// </summary>
        [JsonRequired]
        [JsonProperty("projection", Required = Required.Always)]
        public string? Projection { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate for the center of the map
        /// </summary>
        /// <remarks>
        /// This property is not serialised to json
        /// </remarks>
        [JsonIgnore]
        public double CenterX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate for the center of the map
        /// </summary>
        /// <remarks>
        /// This property is not serialised to json
        /// </remarks>
        [JsonIgnore]
        public double CenterY { get; set; }

        /// <summary>
        /// Gets or sets the center points of map view
        /// </summary>
        /// <remarks>
        /// Will be ordered as x then y and is serialized as the Json property
        /// </remarks>
        [JsonRequired]
        [JsonProperty("center", NullValueHandling = NullValueHandling.Ignore)]
        public List<double?>? Center => [CenterX, CenterY];

        /// <summary>
        /// Gets or sets the initial  zoom level
        /// </summary>
        [JsonProperty("zoom", NullValueHandling = NullValueHandling.Ignore)]
        public int Zoom { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom level
        /// </summary>
        [JsonProperty(PropertyName = "maxZoom", NullValueHandling = NullValueHandling.Ignore)]
        public int MaximumZoom { get; set; } = 23;

        /// <summary>
        /// Gets or sets the minimum zoom level
        /// </summary>
        [JsonProperty(PropertyName = "minZoom")]
        public int? MinimumZoom { get; set; } = 0;

        /// <summary>
        /// Gets or sets the extent minimum X
        /// </summary>
        [JsonIgnore]
        public double ExtentMinX { get; set; }

        /// <summary>
        /// Gets or sets the extent minimum Y
        /// </summary>
        [JsonIgnore]
        public double ExtentMinY { get; set; }

        /// <summary>
        /// Gets or sets the extent maximum X
        /// </summary>
        [JsonIgnore]
        public double ExtentMaxX { get; set; }

        /// <summary>
        /// Gets or sets the extent maximum Y
        /// </summary>
        [JsonIgnore]
        public double ExtentMaxY { get; set; }

        /// <summary>
        /// Gets or sets the extent bottom left x point
        /// </summary>
        /// <remarks>
        /// Will be ordered as bottom left x, bottom left y, top left x, top left y
        /// </remarks>
        [JsonRequired]
        [JsonProperty("extent", NullValueHandling = NullValueHandling.Ignore)]
        public List<double?>? Extent => [ExtentMinX, ExtentMinY, ExtentMaxX, ExtentMaxY];
    }
}
