using Newtonsoft.Json;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

public class Collection
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    [JsonRequired]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the title 
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of links
    /// </summary>
    [JsonRequired]
    public List<Link> Links { get; set; }

    /// <summary>
    /// Gets or sets the extent of the collection
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public double[]? SpatialExtent { get; set; } = null;

    /// <summary>
    /// Gets or sets the list of supported coordinate reference systems
    /// </summary>
    [JsonProperty(propertyName: "crs", NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? CoordinateReferenceSystems { get; set; } = new List<string>() { "http://www.opengis.net/def/crs/EPSG/0/27700" };

    /// <summary>
    /// Gets or sets the type of items contained in the collection
    /// </summary>
    /// <remarks>
    /// In the OGC Features specification this is always "Features"
    /// </remarks>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ItemType { get; set; } = "feature";

    /// <summary>
    /// Gets or sets the list of link templates for the collection
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Link>? LinkTemplates { get; set; }
}