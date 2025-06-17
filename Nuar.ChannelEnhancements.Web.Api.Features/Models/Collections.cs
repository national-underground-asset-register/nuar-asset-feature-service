using Newtonsoft.Json;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

public class Collections
{
    /// <summary>
    /// Gets or sets a list of <see cref="Link"/> items.
    /// </summary>
    [JsonRequired]
    public List<Link> Links { get; set; }

    /// <summary>
    /// Gets or sets a list of <see cref="Collection"/> items.
    /// </summary>
    [JsonProperty(propertyName: "collections")]
    [JsonRequired]
    public List<Collection> CollectionItems { get; set; }
}