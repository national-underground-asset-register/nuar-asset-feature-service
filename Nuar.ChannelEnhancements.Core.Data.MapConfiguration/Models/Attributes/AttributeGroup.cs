using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;
public class AttributeGroup
{
    /// <summary>
    /// Gets or sets the Attribute Group Id.
    /// </summary>
    [Column("id")]
    [JsonRequired]
    [JsonProperty("id", Required = Required.Always)]
    public required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the display name in welsh
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
    /// Gets or sets the name of the display section
    /// </summary>
    [Column("display_section")]
    [JsonProperty("displaySection", NullValueHandling = NullValueHandling.Ignore)]
    public string? DisplaySection { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// display_order
    /// </summary>
    [Column("display_order")]
    [JsonProperty("displayOrder")]
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets whether the attribute group is visible
    /// is_visible  
    /// </summary>
    [Column("is_visible")]
    [JsonProperty("isVisible")]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets the description for the attribute group
    /// </summary>
    [Column("description")]
    [JsonProperty("description")]
    public string? Description { get; set; }
}