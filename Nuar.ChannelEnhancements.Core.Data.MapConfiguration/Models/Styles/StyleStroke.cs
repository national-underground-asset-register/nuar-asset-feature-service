using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StyleStroke
    {
        /// <summary>
        /// Colour of style stroke
        /// stroke_color
        /// </summary>
        [Column("stroke_color")]
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string? Color { get; set; }

        /// <summary>
        /// Gets and sets the line dash string
        /// line_dash_string
        /// </summary>
        [Column("stroke_line_dash")]
        [JsonProperty("lineDash", NullValueHandling = NullValueHandling.Ignore)]
        public string? LineDash { get; set; }

        /// <summary>
        /// Width of style stroke
        /// width
        /// </summary>
        [Column("stroke_width")]
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public double? Width { get; set; }
    }
}
