// <copyright file="GeometryTypes.cs" company="AtkinsRealis">
// Copyright (c) AtkinsRealis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.Features.Helpers;

/// <summary>
/// 1:'Point', 2: 'LineString', 3: 'Polygon', 4: 'MultiPoint', 5: 'MultiLineString',
/// 6: 'MultiPolygon', 7: 'GeometryCollection' 
/// Other types not accepted
/// </summary>
public class GeometryTypes
{
    public const string Point = "Point";
    public const string LineString = "LineString";
    public const string Polygon = "Polygon";
    public const string MultiPoint = "MultiPoint";
    public const string MultiLineString = "MultiLineString";
    public const string MultiPolygon = "MultiPolygon";
    public const string GeometryCollection = "GeometryCollection";
}