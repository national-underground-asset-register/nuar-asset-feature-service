using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StyleMarker
    {
        /// <summary>
        /// Gets and sets the name of the symbol.
        /// </summary>
        [Column("marker_symbol_name")]
        [JsonProperty("symbolName", NullValueHandling = NullValueHandling.Ignore)]
        public string? SymbolName { get; set; }

        /// <summary>
        /// Gets and sets image source for the marker
        /// </summary>
        [Column("marker_image _source")]
        [JsonProperty("imageSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string? ImageSrc { get; set; }

        /// <summary>
        /// Gets and sets the fill colour of the marker
        /// </summary>
        [Column("marker_fill_color")]
        [JsonIgnore]
        public string? FillColor { get; set; }

        /// <summary>
        /// Gets the <see cref="StyleFill"/> for the marker style
        /// </summary>
        [JsonProperty("fill", NullValueHandling = NullValueHandling.Ignore)]
        public StyleFill Fill => new()
        {
            FillColor = FillColor ?? null
        };

        /// <summary>
        /// Gets and sets the number of points for the marker
        /// </summary>
        [Column("marker_points")]
        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public int? Points { get; set; }

        /// <summary>
        /// Gets and sets the radius of the marker points
        /// </summary>
        [Column("marker_radius")]
        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Radius { get; set; }

        /// <summary>
        /// Gets and sets the angle of the marker points
        /// </summary>
        [Column("marker_angle")]
        [JsonProperty("angle", NullValueHandling = NullValueHandling.Ignore)]
        public double? Angle { get; set; }

        /// <summary>
        /// Gets and sets the stroke colour of the marker
        /// </summary>
        [Column("marker_stroke_color")]
        [JsonIgnore]
        public string? StrokeColor { get; set; }

        /// <summary>
        /// Gets and sets the stroke width of the marker
        /// </summary>
        [Column("marker_stroke_width")]
        [JsonIgnore]
        public double? StrokeWidth { get; set; }

        /// <summary>
        /// Gets the <see cref="StyleStroke"/> for the marker style
        /// </summary>
        public StyleStroke Stroke => new()
        {
            Color = StrokeColor ?? null,
            Width = StrokeWidth ?? null,
            LineDash = null
        };

        /// <summary>
        /// Gets and sets the x displacement of the marker
        /// </summary>
        [Column("marker_displacement_x")]
        [JsonIgnore]
        public double? DisplacementX { get; set; }

        /// <summary>
        /// Gets and sets the y displacement of the marker
        /// </summary>
        [Column("marker_displacement_y")]
        [JsonIgnore]
        public double? DisplacementY { get; set; }

        /// <summary>
        /// Gets or sets the displacement for the marker style
        /// </summary>
        [JsonProperty("displacement", NullValueHandling = NullValueHandling.Ignore)]
        public List<double?>? Displacement =>
        [
            DisplacementX ?? null,
            DisplacementY ?? null
        ];

        /// <summary>
        /// Gets and sets whether it is rotated with the view
        /// </summary>
        [Column("marker_rotate_with_view")]
        [JsonProperty("rotateWithView", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RotateWithView { get; set; }

        /// <summary>
        /// Gets and sets the scaling of the marker
        /// </summary>
        [Column("marker_scale")]
        [JsonProperty("scale", NullValueHandling = NullValueHandling.Ignore)]
        public double? Scale { get; set; }

        /// <summary>
        /// Gets and set the rotation of the marker
        /// </summary>
        [Column("marker_rotation")]
        [JsonProperty("rotation", NullValueHandling = NullValueHandling.Ignore)]
        public double? Rotation { get; set; }
    }
}
