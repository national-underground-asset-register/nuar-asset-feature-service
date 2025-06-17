namespace Nuar.ChannelEnhancements.Core.Data.Features.Helpers;

using System.Text.Json;

/// <summary>
/// A static class that provides templated serialisation and deserialisation to and from json
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Allows a class to be serialised to a Json formatted srting
    /// </summary>
    /// <param name="obj"> the instance to be serialised</param>
    /// <param name="options">Optional json options for serialisaiton</param>
    /// <returns></returns>
    public static string ToJson<T>(this object obj, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(obj, options);
    }

    /// <summary>
    /// Allows a class to be deserialised from a Json formatted srting
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json">The json formatted string to parse into the class instance</param>
    /// <param name="options">Optional json options for deserialisaiton</param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }
}