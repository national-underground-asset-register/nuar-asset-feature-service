namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StyleStroke
    {
        /// <summary>
        /// Colour of style stroke
        /// stroke_color
        /// </summary>
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string? Color { get; set; }

        /// <summary>
        /// Gets and sets the line dash string
        /// line_dash_string
        /// </summary>
        [JsonProperty("lineDash", NullValueHandling = NullValueHandling.Ignore)]
        public string? LineDash { get; set; }

        /// <summary>
        /// Width of style stroke
        /// width
        /// </summary>
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public double? Width { get; set; }
    }
}
