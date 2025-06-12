namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StyleFill
    {
        /// <summary>
        /// Get and sets the colour of the fill
        /// </summary>
        /// <remarks>Named FillColor to remove confusion with system Color</remarks>
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string? FillColor { get; set; }

        /// <summary>
        /// Gets and sets the location of a fill image source
        /// </summary>
        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string? ImageSource { get; set; }
    }
}
