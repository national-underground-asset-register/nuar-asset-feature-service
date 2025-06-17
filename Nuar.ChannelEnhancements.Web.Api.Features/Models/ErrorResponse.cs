using Newtonsoft.Json;

namespace Nuar.ChannelEnhancements.Web.Api.Features.Models;

public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the title of the error to return
    /// </summary>
    [JsonProperty("errorTitle")]
    public string ErrorTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message to return
    /// </summary>
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error code to return
    /// </summary>
    /// <remarks>This is optional, set to null to have the serialiser ignore it</remarks>
    [JsonProperty(PropertyName = "errorCode", NullValueHandling = NullValueHandling.Ignore)]
    public int? ErrorCode { get; set; } = null;

    /// <summary>
    /// Constructs an <see cref="ErrorResponse"/> with details from the provided <see cref="Exception"/>
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to return</param>
    public ErrorResponse(Exception exception)
    {
        ErrorTitle = $"Exception raised from: {exception.Source}";
        ErrorMessage = exception.Message;
        ErrorCode = exception.HResult;
    }

    /// <summary>
    /// Constructs an <see cref="ErrorResponse"/> with title and message.
    /// </summary>
    /// <param name="errorTitle">The title of the error</param>
    /// <param name="errorMessage">The message of the error</param>
    public ErrorResponse(string errorTitle, string errorMessage)
    {
        ErrorTitle = errorTitle;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Constructs an <see cref="ErrorResponse"/> with a title, message, and code.
    /// </summary>
    /// <param name="errorTitle">The title of the error</param>
    /// <param name="errorMessage">The message of the error</param>
    /// <param name="errorCode">The code of the error</param>
    public ErrorResponse(string errorTitle, string errorMessage, int errorCode)
    {
        ErrorTitle = errorTitle;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Constructs an <see cref="ErrorResponse"/>
    /// </summary>
    public ErrorResponse()
    {
        // Parameterless constructor
    }
}