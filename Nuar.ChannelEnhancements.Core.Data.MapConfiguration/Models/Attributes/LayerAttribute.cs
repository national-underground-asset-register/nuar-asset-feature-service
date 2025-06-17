using Newtonsoft.Json;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;
public class LayerAttribute
{
    /// <summary>
    /// Gets or sets the Attribute Id.
    /// </summary>
    [Column("id")]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Attribute name
    /// </summary>
    [Column("attribute_name")]
    [JsonRequired]
    [JsonProperty("attributeName", Required = Required.Always)]
    public required string AttributeName { get; set; }

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
    public string? DisplayNameEng { get; set; }

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
    /// Gets or sets the attribute suffix
    /// </summary>
    [Column("attribute_suffix")]
    [JsonProperty("attributeSuffix",NullValueHandling = NullValueHandling.Ignore)]
    public string? AttributeSuffix { get; set; }

    /// <summary>
    /// Gets or sets the default value
    /// default_value
    /// </summary>
    [Column("default_value")]
    [JsonProperty("defaultValue", NullValueHandling = NullValueHandling.Ignore)]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets whether the attribute is visible
    /// is_visible
    /// </summary>
    [Column("is_visible")]
    [JsonProperty("isVisible")]
    public bool IsVisible { get; set; } = false;

    /// <summary>u
    /// Gets or sets the id of the layer this attribute belongs to.
    /// </summary>
    [Column("layer_id")]
    [JsonIgnore]
    public Guid? LayerId { get; set; }

    /// <summary>
    /// Gets or sets the attribute group id
    /// attribute_group_id
    /// </summary>
    [Column("attribute_group_id")]
    [JsonRequired]
    [JsonProperty("attributeGroup")]
    public Guid AttributeGroupId { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    [Column("display_order")]
    [JsonProperty("displayOrder", Required = Required.Always)]
    public int DisplayOrder { get; set; } = 0;

    /// <summary>
    /// Gets or sets whether the attribute is queryable
    /// </summary>
    [Column("is_queryable")]
    [JsonIgnore]
    public bool IsQueryable { get; set; } = false;
}