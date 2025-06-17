namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

public class LandingPage
{
    /// <summary>
    /// Gets or sets the title of the service
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description for the service
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of links for the service
    /// </summary>
    public List<Link> Links { get; set; }
}