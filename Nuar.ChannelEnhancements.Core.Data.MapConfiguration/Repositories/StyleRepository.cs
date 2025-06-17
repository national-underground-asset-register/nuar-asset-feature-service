// <copyright file="StyleRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Repositories;

using System.Data;
using Dapper;
using Helpers;
using Interfaces;
using Models;
using Models.Styles;
using Serilog;

/// <summary>
/// Style Repository class.
/// </summary>
public class StyleRepository : QueryBase, IStyleRepository
{
    private readonly DatabaseConfigurationSettings _dbSettings;

    /// <summary>
    /// Initializes an instance of <see cref="StyleRepository"/>.
    /// </summary>
    /// <param name="dbSettings">The database connection config.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StyleRepository(DatabaseConfigurationSettings dbSettings) : base(dbSettings.ConnectionString ?? throw new ArgumentNullException(dbSettings.ConnectionString))
    {
        // Set the Npgsql switch to enable stored procedure compatibility mode
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

        // Set the database configuration settings
        _dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));

        // Register the custom mappings for the Style model
        SqlMapper.SetTypeMap(typeof(Style), new ColumnAttributeTypeMapper<Style>());
    }

    /// <inheritdoc/>
    public async Task<Style?> GetById(Guid styleId)
    {
        try
        {
            // Ensure Connection is not null before calling QueryAsync
            if (Connection == null)
            {
                Log.Error($"{nameof(GetById)} - Connection is null. Unable to query database.");
                return null;
            }

            var p = new DynamicParameters();
            p.Add("@_style_id", styleId, DbType.Guid, ParameterDirection.Input);

            var style = await Connection.QueryAsync<Style, StyleFill, StyleStroke, StyleMarker, Style>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetStyleById"]}",
                map: (s, fill, stroke, marker) =>
                {
                    s.Fill = fill;
                    s.Marker = marker;
                    s.Stroke = stroke;

                    // Dapper does not map results in a multimap if the splitOn column name returns null from the database so
                    // the stored function returns blank strings for the fill_color, stroke_color, and marker_image_source properties.
                    // This ensures that the properties are always set to a valid object, but will cause us problems later with null checks.
                    // To combat this we need to now set the properties to null if the values are empty strings.
                    if (string.IsNullOrEmpty(fill.FillColor))
                    {
                        s.Fill.FillColor = null;
                    }
                    if (string.IsNullOrEmpty(stroke.Color))
                    {
                        s.Stroke.Color = null;
                    }
                    if (string.IsNullOrEmpty(marker.ImageSrc))
                    {
                        s.Marker.ImageSrc = null;
                    }

                    // Return the processed style object
                    return s;
                },
                param: p,
                splitOn: "fill_color,stroke_color,marker_image_source",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            // Return the first style or null if no styles were found
            return style.FirstOrDefault() ?? null;
        }
        catch (Exception e)
        {
            Log.Error($"{nameof(GetById)} - Unable to get style from database with identifier {styleId}", e);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Style?>?> GetAllStyles()
    {
        try
        {
            // Ensure Connection is not null before calling QueryAsync
            if (Connection == null)
            {
                Log.Error($"{nameof(GetAllStyles)} - Connection is null. Unable to query database.");
                return null;
            }

            var styles = await Connection.QueryAsync<Style, StyleFill, StyleStroke, StyleMarker, Style>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetAllStyles"]}",
                (s, fill, stroke, marker) =>
                {
                    s.Fill = fill;
                    s.Marker = marker;
                    s.Stroke = stroke;

                    // Dapper does not map results in a multimap if the splitOn column name returns null from the database so
                    // the stored function returns blank strings for the fill_color, stroke_color, and marker_image_source properties.
                    // This ensures that the properties are always set to a valid object, but will cause us problems later with null checks.
                    // To combat this we need to now set the properties to null if the values are empty strings.
                    if (string.IsNullOrEmpty(fill.FillColor))
                    {
                        s.Fill.FillColor = null;
                    }
                    if (string.IsNullOrEmpty(stroke.Color))
                    {
                        s.Stroke.Color = null;
                    }
                    if (string.IsNullOrEmpty(marker.ImageSrc))
                    {
                        s.Marker.ImageSrc = null;
                    }

                    // Return the processed style object
                    return s;
                },
                param: null,
                splitOn: "fill_color,stroke_color,marker_image_source",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            return styles.ToList();
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetAllStyles)} - Unable to get styles from database.", ex);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Style?>?> GetAllStylesByMapConfig(Guid configId)
    {
        try
        {
            // Ensure Connection is not null before calling QueryAsync
            if (Connection == null)
            {
                Log.Error($"{nameof(GetAllStylesByMapConfig)} - Connection is null. Unable to query database.");
                return null;
            }

            DynamicParameters p = new DynamicParameters();
            p.Add("@_map_config_id", configId, DbType.Guid, ParameterDirection.Input);

            var styles = await Connection.QueryAsync<Style, StyleFill, StyleStroke, StyleMarker, Style>(
                sql: $"{_dbSettings.MapConfigurationSchemaName}.{_dbSettings.MapConfigurationFunctionMap["GetStylesByMapConfigId"]}",
                (s, fill, stroke, marker) =>
                {
                    s.Fill = fill;
                    s.Marker = marker;
                    s.Stroke = stroke;

                    // Dapper does not map results in a multimap if the splitOn column name returns null from the database so
                    // the stored function returns blank strings for the fill_color, stroke_color, and marker_image_source properties.
                    // This ensures that the properties are always set to a valid object, but will cause us problems later with null checks.
                    // To combat this we need to now set the properties to null if the values are empty strings.
                    if (string.IsNullOrEmpty(fill.FillColor))
                    {
                        s.Fill.FillColor = null;
                    }
                    if (string.IsNullOrEmpty(stroke.Color))
                    {
                        s.Stroke.Color = null;
                    }
                    if (string.IsNullOrEmpty(marker.ImageSrc))
                    {
                        s.Marker.ImageSrc = null;
                    }

                    // Return the processed style object
                    return s;
                },
                param: p,
                splitOn: "fill_color,stroke_color,marker_image_source",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 900);

            return styles.ToList();
        }
        catch (Exception ex)
        {
            Log.Error($"{nameof(GetAllStylesByMapConfig)} - Unable to get styles from database.", ex);
            return null;
        }
    }
}