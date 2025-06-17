namespace Nuar.ChannelEnhancements.Web.Api.Features.Controllers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;
using Nuar.ChannelEnhancements.Web.Api.Features.Models;
using Serilog;

[ApiController]
public class CapabilitiesController : ControllerBase
{
    // Local variable to hold the data access layer instance
    private readonly IDataAccessLayer _dataAccessLayer;

    public CapabilitiesController(IDataAccessLayer dataAccessLayer)
    {
        // Fetch the data access layer from the dependency injection container
        _dataAccessLayer = dataAccessLayer ?? throw new ArgumentNullException(nameof(dataAccessLayer));
    }

    /// <summary>
    /// Gets the landing page for the service.  The landing page provides links to the API definition, the conformance statements and to the feature collections in this dataset.
    /// </summary>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///         "title": "NUAR Channel Enhancements - Asset Query Service",
    ///         "description": "Provides an (almost) OGC API Features compliant service to query asset data.",
    ///         
    ///         "links": [
    ///             {
    ///                 "href": "https://assetqueryservice/api/v1",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "self",
    ///                 "title": "This document",
    ///                 "type": "application/json"
    ///             },
    ///             {
    ///                 "href": "https://assetqueryservice.nce.dev/swagger/v1/swagger.json",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "service-desc",
    ///                 "title": "The API Definition",
    ///                 "type": "application/vnd.oai.openapi+json;version=3.0"
    ///             },
    ///             {
    ///                 "href": "https://assetqueryservice.nce.dev/redoc/index.html",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "service-desc",
    ///                 "title": "The API Documentation",
    ///                 "type": "text/html"
    ///             },
    ///             {
    ///                 "href": "https://assetqueryservice.nce.dev/api/v1/conformance",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "conformance",
    ///                 "title": "OGC API conformance classes implemented by this server",
    ///                 "type": "application/json"
    ///             },
    ///             {
    ///                 "href": "https://assetqueryservice.nce.dev/api/v1/configurations/3648cbd1-7911-430b-aeb9-58d999a7fb24/groups",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "data",
    ///                 "title": "NUAR Data Model v2.0.5",
    ///                 "type": "application/json"
    ///             },
    ///             {
    ///                 "href": "https://assetqueryservice.nce.dev/api/v1/configurations/adc57284-9682-4c6e-a86d-ec64861f5202/groups",
    ///                 "hrefLang": "",
    ///                 "length": 0,
    ///                 "rel": "data",
    ///                 "title": "NUAR Sandbox Data Model v2.0.5",
    ///                 "type": "application/json"
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Success - The service landing page.</response>
    /// <response code="401">Unauthorised - User is not authenticated.</response>
    /// <response code="403">Forbidden - User is not authorised to access this endpoint.</response>
    /// <response code="500">Server Error - A server error has occurred.</response>
    [HttpGet("/api/v1")]
    [EndpointName("GetLandingPage")]
    [EndpointDescription("The landing page provides links to the API definition, the conformance statements and to the feature collections in this dataset.")]
    [EndpointSummary("Landing Page")]
    [ProducesResponseType(typeof(LandingPage), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetLandingPage()
    {
        try
        {
            // Populate a new LandingPage object
            LandingPage landingPage = new LandingPage()
            {
                Title = "NUAR Channel Enhancements - Asset Feature Service",
                Description = "Provides an (almost) OGC API Features compliant service to query asset data."
            };

            // Create the list of links that will be reported and initialise it with a reference to the landing page
            List<Link> links = new List<Link>()
            {
                new()
                {
                    Href = $"/api/v1",
                    Title = "This document",
                    Rel = "self",
                    Type = "application/json"
                },
                new()
                {
                    Href = $"/openapi/v1.json",
                    Title = "The API Definition",
                    Rel = "service-desc",
                    Type = "application/vnd.oai.openapi+json;version=3.0"
                },
                new()
                {
                    Href = $"/api-docs/index.html",
                    Title = "The API Documentation",
                    Rel = "service-doc",
                    Type = "text/html"
                },
                new()
                {
                    Href = $"api/v1/conformance",
                    Title = "OGC API conformance classes implemented by this server",
                    Rel = "conformance",
                    Type = "application/json"
                }
            };

            // Create a list of Links to report, these are routes to different aspects of the service
            List<MapConfiguration> mapConfigurations = (_dataAccessLayer.MapConfigurationRepository.GetAll().Result ?? Array.Empty<MapConfiguration>()).ToList();

            links.AddRange(mapConfigurations.Select(mapConfig => new Link()
            {
                Href = $"/api/v1/{mapConfig.Id}/collections",
                Type = "application/json",
                Title = mapConfig.Name ?? string.Empty,
                Rel = "data"
            }).ToList());

            landingPage.Links = links;

            // Return the landing page data
            return Ok(landingPage);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return StatusCode(500, new ErrorResponse(ex));
        }
    }

    /// <summary>
    /// Gets information about specifications that this API conforms to.
    /// </summary>
    /// <remarks>
    /// Sample response:
    ///
    ///     {
    ///         "conformsTo": [
    ///             "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/core",
    ///             "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/geojson",
    ///             "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/oas30"
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Success - The URIs of all conformance classes supported by the server.</response>
    /// <response code="500">Server Error - A server error has occurred.</response>
    [HttpGet("/api/v1/conformance")]
    [EndpointName("GetConformance")]
    [EndpointSummary("OGC API Conformance")]
    [EndpointDescription("Gets information about specifications that this API conforms to.")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public virtual Task<IActionResult> GetConformance()
    {
        try
        {
            // Create a new conformance class
            OgcApiConformance conformanceClasses = new();

            // Set the conformance uris we support
            conformanceClasses.SetConformance(OgcApiConformance.ApiConformanceClass.Core, true);
            conformanceClasses.SetConformance(OgcApiConformance.ApiConformanceClass.GeoJson, true);
            conformanceClasses.SetConformance(OgcApiConformance.ApiConformanceClass.OpenApiSpecification3, true);

            // Return the response wrapped in a Task
            return Task.FromResult<IActionResult>(Ok(conformanceClasses.ConformsTo));
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return Task.FromResult<IActionResult>(StatusCode(500, new ErrorResponse(ex)));
        }
    }

    /// <summary>
    /// Gets the list of collections available in the dataset.
    /// </summary>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///         "links": [
    ///             {
    ///                 "href": "https://localhost:28007/api/v1/configurations/adc57284-9682-4c6e-a86d-ec64861f5202/groups/27f1172b-ed2b-40ae-9001-e4d7191cb065/collections?layerGroupId=27f1172b-ed2b-40ae-9001-e4d7191cb065",
    ///                 "rel": "self",
    ///                 "type": "application/json",
    ///                 "hrefLang": "",
    ///                 "title": "",
    ///                 "length": 0
    ///             }
    ///         ],
    ///         "collections": [
    ///             {
    ///                 "id": "2da9424a-4d09-40fe-89ff-74453fe780db",
    ///                 "title": "Water Network Node Zone of Interest",
    ///                 "description": "Water Network Node Zone of Interest",
    ///                 "links": [
    ///                     {
    ///                         "href": "https://localhost:28007/api/v1/configurations/adc57284-9682-4c6e-a86d-ec64861f5202/groups/27f1172b-ed2b-40ae-9001-e4d7191cb065/collections/2da9424a-4d09-40fe-89ff-74453fe780db",
    ///                         "rel": "self",
    ///                         "type": "application/json",
    ///                         "hrefLang": "",
    ///                         "title": "",
    ///                         "length": 0
    ///                     }
    ///                 ],
    ///                 "crs": [
    ///                     "http://www.opengis.net/def/crs/EPSG/0/27700"
    ///                 ],
    ///                 "itemType": "feature"
    ///             },
    ///             {
    ///                 "id": "8322e835-b57b-40cf-b1fb-8b5504be028b",
    ///                 "title": "Water Network Node",
    ///                 "description": "Water Network Node",
    ///                 "links": [
    ///                     {
    ///                         "href": "https://localhost:28007/api/v1/configurations/adc57284-9682-4c6e-a86d-ec64861f5202/groups/27f1172b-ed2b-40ae-9001-e4d7191cb065/collections/8322e835-b57b-40cf-b1fb-8b5504be028b",
    ///                         "rel": "self",
    ///                         "type": "application/json",
    ///                         "hrefLang": "",
    ///                         "title": "",
    ///                         "length": 0
    ///                     }
    ///                 ],
    ///                 "crs": [
    ///                     "http://www.opengis.net/def/crs/EPSG/0/27700"
    ///                 ],
    ///                 "itemType": "feature"
    ///             }
    ///         ]
    ///     }
    ///    
    /// </remarks>
    /// <param name="mapConfigId">The id of the map configuration.</param>
    /// <response code="200">Success - The list of available collections in the service.</response>
    /// <response code="400">Bad Request - The request is malformed or missing parameters.</response>
    /// <response code="404">Not Found - No collections found for the given map configuration and group.</response> 
    /// <response code="500">Server Error - A server error has occurred.</response>
    [HttpGet("/api/v1/{mapConfigId}/collections")]
    [EndpointName("GetCollections")]
    [EndpointSummary("Get Collections")]
    [EndpointDescription("Gets the list of collections available in the dataset. This is the root level collection and in the context of NUAR map configuration this equates to all layer groups.")]
    [ProducesResponseType(typeof(Collections), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetCollections([FromRoute][Required][Description("The unique identifier for the configuration to get collections for.")] string mapConfigId)
    {
        try
        {
            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(mapConfigId, out var configId)) return BadRequest();

            // Log the request
            Log.Information($"Fetching LayerGroups in configuration [{mapConfigId}] and returning them as collections.");

            // Get the layer groups belonging to the provided map configuration identifier
            List<LayerGroup> layerGroups = (_dataAccessLayer.LayerGroupRepository.GetLayerGroupsByMapConfigId(configId).Result ?? Enumerable.Empty<LayerGroup>()).ToList();

            // If no layer groups found then there are no collections, return a 404 response
            if (layerGroups.Count == 0)
            {
                // Create an error response
                var error = new ErrorResponse()
                {
                    ErrorTitle = "No Collections Found",
                    ErrorMessage = $"Could not find collections for map configuration: {mapConfigId}"
                };
                    
                // Log the error
                Log.Error($"[GetCollections] - {error.ErrorMessage}");

                return NotFound(error);
            }

            // Fix for IDE0305: Simplify collection initialization
            Collections collections = new Collections
            {
                Links = new List<Link>
                {
                    new()
                    {
                        Rel = "self",
                        Type = "application/json",
                        Href = $"/api/v1/{mapConfigId}/collections",
                        Title = "this document"
                    }
                }
            };

            // Transform the layer groups into a list of CollectionItem
            List<Collection> collectionItems = layerGroups.OrderBy(o => o.DisplayOrder).Select(group => new Collection()
            {
                Id = group.Id.ToString(),
                Description = group.DisplayName["en"] ?? string.Empty,
                Title = group.DisplayName["en"] ?? string.Empty,
                Links = new List<Link>()
                {
                    new()
                    {
                        Rel = "self",
                        Type = "application/json",
                        Href = $"/api/v1/{mapConfigId}/collections/{group.Id}",
                        Title = "this document"
                    }
                },
                ItemType = "collection"
            }).ToList();

            // Add the list of collection items to the collections return
            collections.CollectionItems = collectionItems;

            // Return the processed list
            return Ok(collections);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return StatusCode(500, new ErrorResponse(ex));
        }
    }

    /// <summary>
    /// Describe the feature collection with id 'collectionId'
    /// </summary>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///         "id": "2da9424a-4d09-40fe-89ff-74453fe780db",
    ///         "title": "Water Network Node Zone of Interest",
    ///         "description": "Water Network Node Zone of Interest",
    ///         "links": [
    ///             {
    ///                 "href": "https://localhost:28007/api/v1/configurations/adc57284-9682-4c6e-a86d-ec64861f5202/groups/27f1172b-ed2b-40ae-9001-e4d7191cb065/collections",
    ///                 "rel": "self",
    ///                 "type": "application/json",
    ///                 "hrefLang": "",
    ///                 "title": "",
    ///                 "length": 0
    ///             }
    ///         ],
    ///         "spatialExtent": [0, 0, 700000, 1300000],
    ///         "crs": [
    ///             "EPSG:27700"
    ///         ],
    ///         "itemType": "feature"
    ///     }
    ///    
    /// </remarks>
    /// <param name="mapConfigId">The id of the map configuration the collection belongs to.</param>
    /// <param name="groupId">The id of the group the collection belongs to.</param>
    /// <param name="collectionId">The asset type</param>
    /// <response code="200">Success - The collection item matching the given 'collectionId'.</response>
    /// <response code="401">Unauthorised - User is not authenticated.</response>
    /// <response code="403">Forbidden - User is not authorised to access this endpoint.</response>
    /// <response code="404">Not Found - Could not find collection with id matching 'collectionId'.</response>
    /// <response code="500">Server Error - A server error has occurred.</response>
    [HttpGet("/api/v1/{mapConfigId}/collections/{collectionId}")]
    [EndpointName("GetCollection")]
    [EndpointSummary("Get Collection")]
    [EndpointDescription("Describe the feature collection with id 'collectionId'.")]
    [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetCollection([FromRoute][Required][Description("The configuration identifier.")] string mapConfigId, 
        [FromRoute][Required][Description("The collection identifier.")] string collectionId)
    {
        try
        {
            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(mapConfigId, out var configId)) return BadRequest();
                
            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(collectionId, out var collectionGuid)) return BadRequest();

            // Based on the NUAR map configuration model, a collection identifier could be either another layer group or a layer. In most cases it will be a layer identifier, so
            // we'll make that assumption first and look for a layer matching the collection identifier. If that fails, we'll then look for a layer group matching the collection identifier.
            // If we find neither then we'll return a 404 response.

            // Log the request
            Log.Information($"Attempting to get collection details for {collectionId} from map configuration {mapConfigId}.");

            // First attempt to get a layer
            dynamic? collection = await _dataAccessLayer.LayerRepository.GetLayerByMapConfigIdAndLayerId(configId, collectionGuid);

            if (collection == null)
            {
                collection = await _dataAccessLayer.LayerGroupRepository.GetLayerGroupById(configId, collectionGuid);
            }

            // If we still have no collection then return a 404 response
            if (collection == null)
            {
                // Create an error response
                var error = new ErrorResponse()
                {
                    ErrorTitle = "No Collection Found",
                    ErrorMessage = $"Could not find collection with id: {collectionId} in map configuration: {mapConfigId}."
                };

                // Log the error
                Log.Error($"[GetCollection] - {error.ErrorMessage}");
                return NotFound(error);
            }

            // Transform the result into a CollectionItem, setting properties according to the type of collection found
            Collection item = new Collection()
            {
                Id = collection.Id.ToString(),
                Description = collection.DisplayName["en"] ?? string.Empty,
                Title = collection.DisplayName["en"] ?? string.Empty,
                CoordinateReferenceSystems = collection is Layer ? new List<string> { "EPSG:27700" } : null,
                SpatialExtent = collection is Layer ? new[] { 0.0, 0.0, 700000.0, 1300000.0 } : null,
                ItemType = collection is Layer ? "feature" : "collection",
                Links = new List<Link>()
                {
                    new Link()
                    {
                        Rel = "self",
                        Type = "application/json",
                        Href = $"/api/v1/{mapConfigId}/collections/{collectionId}",
                        Title = "this document"
                    }
                }
            };

            // If the collection is a layer, we can add additional links and then send the response
            if (collection is Layer layer)
            {
                item.Links.AddRange(new List<Link>()
                {
                    new Link()
                    {
                        Rel = "ogc-rel:queryables",
                        Type = "application/json",
                        Href = $"/api/v1/{mapConfigId}/collections/{collectionId}/queryables",
                        Title = "queryable properties"
                    },
                    new Link()
                    {
                        Rel = "items",
                        Type = "application/geo+json",
                        Href = $"/api/v1/{mapConfigId}/collections/{collectionId}/items",
                        Title = (layer.DisplayName["en"] ?? "collection items")
                    }
                });
                item.LinkTemplates = new List<Link>()
                {
                    new Link()
                    {
                        Rel = "item",
                        Type = "application/geo+json",
                        Href = $"/api/v1/{mapConfigId}/collections/{collectionId}/items/{{featureId}}",
                        Title = (layer.DisplayName["en"] ?? "feature item")
                    }
                };

                // Send the response
                return Ok(item);
            }

            // If the collection is a layer group, we need to query to get its children and provide them as links
            if (collection is LayerGroup layerGroup)
            {
                // A layer group could be the parent of another layer group or a collection of layers, so we need to ensure we handle both cases.
                if (layerGroup.HasChildren)
                {
                    // This is a layer group with children, so we need to get the child groups and add them as collection links
                    var childGroups = await _dataAccessLayer.LayerGroupRepository.GetChildLayerGroups(layerGroup.Id);

                    // Add links for each child layer group
                    if (childGroups != null && childGroups.Any())
                    {
                        item.Links.AddRange(childGroups.Select(childGroup => new Link()
                        {
                            Rel = "data",
                            Type = "application/json",
                            Href = $"/api/v1/{mapConfigId}/collections/{childGroup.Id}",
                            Title = childGroup.DisplayName["en"] ?? string.Empty
                        }));
                    }
                }

                // Get the layers in the layer group, a layer group could have child layer groups and also layers
                List<Layer> layers = (_dataAccessLayer.LayerRepository.GetLayersByGroupId(layerGroup.Id).Result ?? Enumerable.Empty<Layer>()).ToList();
                    
                // If no layers found, return a 404 response
                if (layers.Any())
                {
                    // Add links to each layer in the layer group
                    item.Links.AddRange(layers.Select(layer => new Link()
                    {
                        Rel = "items",
                        Type = "application/geo+json",
                        Href = $"/api/v1/{mapConfigId}/collections/{collectionId}/items/{layer.Id}",
                        Title = layer.DisplayName["en"] ?? string.Empty
                    }));
                }
            }

            // Return the found item
            return Ok(item);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to fetch with id: {collectionId}. Exception Message: {ex.Message}");
            return StatusCode(500, new ErrorResponse(ex));
        }
    }
}