using Newtonsoft.Json;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

public class OgcApiConformance
{
    /// <summary>
    /// Identifiers of the conformance classes defined in the OGC API Features (Core) specification.
    /// </summary>
    public enum ApiConformanceClass
    {
        Core,
        Html,
        GeoJson,
        GmlSimpleFeaturesProfile0,
        GmlSimpleFeaturesProfile2,
        OpenApiSpecification3
    }

    /// <summary>
    /// Defined list of conformance class Uris according to the OGC API Features specification
    /// </summary>
    private static readonly Dictionary<ApiConformanceClass, string> _conformanceUris = new Dictionary<ApiConformanceClass, string>
        {
            {ApiConformanceClass.Core, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/core" },
            {ApiConformanceClass.GeoJson, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/geojson" },
            {ApiConformanceClass.GmlSimpleFeaturesProfile0, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/gmlsf0" },
            {ApiConformanceClass.GmlSimpleFeaturesProfile2, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/gmlsf2" },
            {ApiConformanceClass.Html, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/html"},
            {ApiConformanceClass.OpenApiSpecification3, "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/oas30"}
        };

    /// <summary>
    /// Private list of conformance Uris to be managed
    /// </summary>
    private List<string>? _conformsTo;

    public OgcApiConformance()
    {
        // Initialise the list of conformance Uris
        _conformsTo = new List<string>();
    }

    /// <summary>
    /// Sets the specified <see cref="ApiConformanceClass"/> support status
    /// </summary>
    /// <param name="conformanceClass">The class to set the status of</param>
    /// <param name="supported">The support status to set, True is supported and False is not supported</param>
    public void SetConformance(ApiConformanceClass conformanceClass, bool supported)
    {
        // Get the Uri for the given conformance class
        string conformanceUri = _conformanceUris[conformanceClass];

        // Add or remove it from the list
        if (supported)
        {
            if (_conformsTo.Contains(conformanceUri))
                return;
            _conformsTo.Add(conformanceUri);
        }
        else
        {
            if (_conformsTo.Contains(conformanceUri)) _conformsTo.Remove(conformanceUri);
        }
    }

    /// <summary>
    /// Gets the list of conformance classes supported
    /// </summary>
    ///
    [JsonProperty(PropertyName = "conformsTo")]
    public ConformsToResponse ConformsTo
    {
        get
        {
            return new ConformsToResponse(_conformsTo);
        }
    }
}

public class ConformsToResponse
{
    public ConformsToResponse(List<string> conformanceUris)
    {
        ConformsTo = conformanceUris;
    }

    public List<string> ConformsTo { get; set; }
}