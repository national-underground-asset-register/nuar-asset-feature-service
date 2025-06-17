using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StyleFill
    {
        /// <summary>
        /// Get and sets the colour of the fill
        /// </summary>
        /// <remarks>Named FillColor to remove confusion with system Color</remarks>
        
        [Column("fill_color")]
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string? FillColor { get; set; }

        /// <summary>
        /// Gets and sets the location of a fill image source
        /// </summary>
        [Column("fill_image_source")]
        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string? ImageSource { get; set; }
    }
}
