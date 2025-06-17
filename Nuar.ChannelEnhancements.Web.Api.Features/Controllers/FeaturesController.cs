using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Nuar.ChannelEnhancements.Core.Data.Features;
using Nuar.ChannelEnhancements.Core.Data.Features.Interfaces;
using NetTopologySuite.Features;
using Newtonsoft.Json.Linq;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Attributes;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Layers;
using Nuar.ChannelEnhancements.Web.Api.Features.Models;
using Serilog;
using NetTopologySuite.Index.HPRtree;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Controllers;

[ApiController]
public class FeaturesController : Controller
{
    private readonly IFeaturesRepository _featuresRepository;

    private readonly ILayerRepository _layerRepository;

    private readonly IAttributeRepository _attributeRepository;

    private readonly ILayerGroupRepository _layerGroupRepository;

    private readonly string _assetFeatureSchema = "nuardata"; //todo use config

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="featuresRepository">The <see cref="IFeaturesRepository"/> used to perform operations involving collections</param>
    public FeaturesController(IFeaturesRepository featuresRepository, IAttributeRepository attributeRepository, ILayerRepository layerRepository, ILayerGroupRepository layerGroupRepository)
    {
        _featuresRepository = featuresRepository;
        _attributeRepository = attributeRepository;
        _layerRepository = layerRepository;
        _layerGroupRepository = layerGroupRepository;
    }

    /// <summary>
    /// Gets the features from the collection matching the given 'collectionId'
    /// </summary>
    /// <param name="mapConfigId">The id of the map configuration.</param>
    /// <param name="groupId">The id of the collections group.</param>
    /// <param name="collectionId">The id of the collection to get features for.</param>
    /// <param name="bbox">A comma separated list of coordinates expressing a bounding box in the form minX,minY,maxX,maxY.</param>
    /// <remarks>
    /// Sample Response:
    /// 
    ///     {
    ///         "type": "FeatureCollection",
    ///         "features": [
    ///             {
    ///                 "type": "Feature",
    ///                 "bbox": [531670.570942462, 181124.680414347, 531670.570942462, 181124.680414347],
    ///                 "geometry": {
    ///                     "type": "Point",
    ///                     "coordinates": [531670.570942462, 181124.680414347]
    ///                 },
    ///                 "properties": {
    ///                     "depthmethod": null,
    ///                     "installationmethod": null,
    ///                     "materialsubtype": null,
    ///                     "objectownerassigneduniqueid": null,
    ///                     "componentsubtype": "Capped End",
    ///                     "verticalcrs": null,
    ///                     "originaldatedatacollected": null,
    ///                     "horizontalmeasurementmethod": null,
    ///                     "insideheight_height": null,
    ///                     "locationtype": null,
    ///                     "colour": null,
    ///                     "componenttype": "Capped End",
    ///                     "enhancedmeasures": null,
    ///                     "depth_depth": null,
    ///                     "operator": "Asset Operator",
    ///                     "objectname": "Asset Name",
    ///                     "objectowner": "Asset Owner",
    ///                     "installationmethodsubtype": null,
    ///                     "operationalstatus": "Abandoned",
    ///                     "localereferencetype": null,
    ///                     "insidewidth_unitofmeasure": "Millimetres",
    ///                     "qualitylevel": null,
    ///                     "description": "Standard Description",
    ///                     "dateoflaststatuschange": null,
    ///                     "undergroundstatus": null,
    ///                     "systemloaddate": "2023-06-20T19:44:35.318Z",
    ///                     "localereference": null,
    ///                     "datedatacollected": "2009-12-09T00:00:00",
    ///                     "horizontalcrs": "27700",
    ///                     "dateofinstallation": null,
    ///                     "insidelength_length": null,
    ///                     "material": "Other",
    ///                     "dataowner": "Data Owner",
    ///                     "insidewidth_width": 99
    ///                 }
    ///             },
    ///             {
    ///                 "type": "Feature",
    ///                 "bbox": [531877.899261178, 181633.906849548, 531877.899261178, 181633.906849548],
    ///                 "geometry": {
    ///                     "type": "Point",
    ///                     "coordinates": [531877.899261178, 181633.906849548]
    ///                 },
    ///                 "properties": {
    ///                     "depthmethod": null,
    ///                     "installationmethod": null,
    ///                     "materialsubtype": null,
    ///                     "objectownerassigneduniqueid": null,
    ///                     "componentsubtype": "Fire Hydrant",
    ///                     "verticalcrs": null,
    ///                     "originaldatedatacollected": null,
    ///                     "horizontalmeasurementmethod": null,
    ///                     "insideheight_height": null,
    ///                     "locationtype": null,
    ///                     "colour": null,
    ///                     "componenttype": "Hydrant",
    ///                     "enhancedmeasures": null,
    ///                     "depth_depth": null,
    ///                     "operator": "Asset Operator",
    ///                     "objectname": "Asset Name",
    ///                     "objectowner": "Asset Owner",
    ///                     "installationmethodsubtype": null,
    ///                     "operationalstatus": "In Service",
    ///                     "localereferencetype": null,
    ///                     "insidewidth_unitofmeasure": "Millimetres",
    ///                     "qualitylevel": null,
    ///                     "description": "Standard Description",
    ///                     "dateoflaststatuschange": null,
    ///                     "undergroundstatus": null,
    ///                     "systemloaddate": "2022-09-29T12:17:54.805Z",
    ///                     "localereference": null,
    ///                     "datedatacollected": null,
    ///                     "horizontalcrs": "27700",
    ///                     "dateofinstallation": null,
    ///                     "insidelength_length": null,
    ///                     "material": "Other",
    ///                     "dataowner": "Data Owner",
    ///                     "insidewidth_width": 99
    ///                 }
    ///            }
    ///         ],
    ///         "bbox": [531600, 180600, 531880, 1808904]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Success - The response document consisting of features in a collection.</response>
    /// <response code="400">Bad Request - The request is malformed or missing parameters.</response>
    /// <response code="401">Unauthorised - User is not authenticated.</response>
    /// <response code="403">Forbidden - User is not authorised to access this endpoint.</response>
    /// <response code="404">Not Found - No collections found for the given details.</response> 
    /// <response code="500">Server Error - A server error has occurred.</response>
    [HttpGet("/api/v1/configurations/{mapConfigId}/groups/{groupId}/collections/{ collectionId}/items")]
    [EndpointName("GetFeatures")]
    [EndpointSummary("Get FeatureCollection")]
    [EndpointDescription("Gets the features intersecting the given bounding box for the specified `collectionId`. The bounding box parameter `bbox` is mandatory.")]
    [ProducesResponseType(typeof(FeatureCollection), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public virtual IActionResult GetFeatures([FromRoute][Required] string mapConfigId, [FromRoute][Required] string groupId, [FromRoute][Required] string collectionId, [FromQuery][Required] string bbox)
    {
        try
        {
            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(mapConfigId, out var configId)) return BadRequest();

            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(groupId, out var layerGroupId)) return BadRequest();

            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(collectionId, out var collectionGuid)) return BadRequest();

            // TODO: Consider security here where one might like to check the user is authorised to access the collectionId, layerGroupId and configId using the provided bounding box
            double[] boundingBox = bbox.Split(',').Select(coord => double.Parse(coord.Trim())).ToArray();

            Log.Debug($"Attempting to get features for collection {collectionId} in group {layerGroupId} and configuration {configId} filtered by BoundingBox: {bbox}");

            // Get the layer and its attributes
            Layer? lyr = _layerRepository.GetLayerByMapConfigIdAndLayerId(configId, collectionGuid).Result;
            if (lyr == null)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorTitle = "Could Not Find Layer",
                    ErrorMessage = $"Could not find layer with id {collectionId} in group {layerGroupId} for configuration {configId}.",
                };

                return NotFound(error);
            }

            // Get the list of attributes and generate a list of strings then set it to the features repository
            // NOTE: There is a short-term fix here to make sure the list of attributes for the table is unique, the current map configuration design has got
            // potential for duplicate attribute names in attribute groups. A HashSet is used as it does not allow duplicate items and so won't add them.
            HashSet<string> attributeNames = lyr.Attributes.Select(layerAttribute => $"\"{layerAttribute.AttributeName!}\"").ToHashSet();

            // Remove strings from the hashset that have the value RowComponent# in them
            attributeNames.RemoveWhere(attr => attr.Contains("RowComponent#"));
            attributeNames.RemoveWhere(attr => attr.Contains("NUARACTOR:"));

            _featuresRepository.ColumnNames = attributeNames.ToList();
            _featuresRepository.SchemaName = _assetFeatureSchema;

            FeatureCollection? features = _featuresRepository.GetFeatureCollection(lyr.Name, boundingBox[0], boundingBox[1], boundingBox[2], boundingBox[3]);

            if (features == null)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorTitle = "Could Not Find Features",
                    ErrorMessage = $"Could not find features for collection [{collectionId}] in group [{layerGroupId}] and configuration [{configId}].",
                };

                return NotFound(error);
            }

            Log.Debug($"Found features for collection {collectionId} in group {layerGroupId} and configuration {configId}.");
            return Ok(features);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to fetch with id: {collectionId}. Exception Message: {ex.Message}");
            return StatusCode(500, new ErrorResponse(ex));
        }
    }

    /// <summary>
    /// Gets the feature with the given 'featureId' from the collection with the given 'collectionId'
    /// </summary>
    /// <param name="mapConfigId">The id of the map configuration.</param>
    /// <param name="collectionId">The id of the collection the feature can be found in.</param>
    /// <param name="featureId">The id of the feature to return.</param>
    /// <remarks>
    /// Sample Response:
    /// 
    ///     {
    ///         TBD
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Success - The response document consisting of a feature.</response>
    /// <response code="401">Unauthorised</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Server Error</response>
    [HttpGet("/api/v1/configurations/{mapConfigId}/collections/{collectionId}/items/{featureId}")]
    [EndpointName("GetFeature")]
    [EndpointSummary("Get Feature by Id")]
    [EndpointDescription("Gets the feature with the given 'featureId' from the collection with the given 'collectionId'.")]
    [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetFeature([FromRoute][Required] string mapConfigId, [FromRoute][Required] string collectionId, [FromRoute][Required] string featureId)
    {
        try
        {
            Log.Debug($"Attempting to get feature [{featureId}] from collection [{collectionId}].");

            // Attempt to convert the passed in value to a Guid, return 400 if not
            if (!Guid.TryParse(collectionId, out var collectionGuid)) return BadRequest();

            if (!Guid.TryParse(mapConfigId, out var mapConfigGuid)) return BadRequest();

            if (!Guid.TryParse(featureId, out var featureGuid)) return BadRequest();

            Feature? feature = null;

            List<Layer>? layerList = await _layerRepository.GetLayersByGroupId(collectionGuid);
            if ((layerList == null) || (layerList.Count == 0))
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorTitle = "Could Not Find Features",
                    ErrorMessage = $"Could not find features for collection [{collectionId}].",
                };

                return NotFound(error);
            }

            // Now we need to get the feature (assetType in the layer) 
            foreach (Layer lyr in layerList)
            {
                // NOTE: There is a short-term fix here to make sure the list of attributes for the table is unique, the current map configuration design has got
                // potential for duplicate attribute names in attribute groups. A HashSet is used as it does not allow duplicate items and so wont add them.
                HashSet<string> attributeNames = lyr.Attributes.Select(layerAttribute => $"\"{layerAttribute.AttributeName!}\"").ToHashSet();

                // Remove strings from the hashset that have the value RowComponent# in them
                attributeNames.RemoveWhere(attr => attr.Contains("RowComponent#"));
                attributeNames.RemoveWhere(attr => attr.Contains("NUARACTOR:"));

                // Set the column names and schema name for the features repository
                _featuresRepository.ColumnNames = attributeNames.ToList();
                _featuresRepository.SchemaName = _assetFeatureSchema;

                // loop through the set of lyr.layerAttributes and find the attribute with an id that matches the featureId
                // and then set the featureIdColumnName to the attribute name
                foreach (LayerAttribute layerAttribute in lyr.Attributes)
                {
                    if (layerAttribute.Id == featureGuid)
                    {
                        // If the attribute is found then set the featureIdColumnName to the attribute name
                        // check that the name is not null or empty 
                        if (string.IsNullOrEmpty(layerAttribute.AttributeName))
                        {
                            Log.Debug($"Null item for feature [{featureId}] in collection [{collectionId}].");
                            StatusCode(500, "NULL item in collection");
                            break;
                        }

                        _featuresRepository.FeatureIdColumnName = layerAttribute.AttributeName;
                        break;
                    }
                }

                // If we did not find it in this layer then we try the next layer
                if (!string.IsNullOrEmpty(_featuresRepository.FeatureIdColumnName))
                {
                    // We found the featureId in the layer so now we can get the feature
                    feature = _featuresRepository.GetFeature(lyr.Name, featureId);
                    break;
                }
            }

            // Did we find ANY features
            if (feature == null)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorTitle = "Feature Not Found",
                    ErrorMessage = $"Could not find feature [{featureId}] in collection [{collectionId}].",
                };

                return NotFound(error);
            }

            Log.Debug($"Found feature [{featureId}] in collection [{collectionId}].");
            return Ok(feature);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to fetch with id: {collectionId}. Exception Message: {ex.Message}");
            return StatusCode(500, new ErrorResponse(ex));
        }
    }
}