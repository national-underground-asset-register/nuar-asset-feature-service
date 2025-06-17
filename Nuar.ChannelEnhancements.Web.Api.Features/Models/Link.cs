using Newtonsoft.Json;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class Link
{
    /// <summary>
    /// Gets or sets the target of the link
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the URI template for the link
    /// </summary>
    public string? UriTemplate { get; set; }

    /// <summary>
    /// Gets or sets the rel for the link
    /// </summary>
    [JsonRequired]
    public required string Rel { get; set; }

    /// <summary>
    /// Gets or sets the type of the link
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the language for the link
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? HrefLang { get; set; }

    /// <summary>
    /// Gets or sets the title of the link
    /// </summary>
    [JsonRequired]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the length of the link
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int Length { get; set; } = 0;

    /// <summary>
    /// Gets or sets the variable base for the link template
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? VarBase { get; set; }
}