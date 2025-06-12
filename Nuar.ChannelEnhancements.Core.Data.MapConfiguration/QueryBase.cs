// <copyright file="QueryBase.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration
{
    using Helpers;
    using Helpers.FluentMap.Attributes;
    using Helpers.FluentMap.Styles;
    using Helpers.FluentMap.Models;
    using Npgsql;

    /// <summary>
    /// Base query class.
    /// </summary>
    public class QueryBase : IDisposable
    {
        /// <summary>
        /// Gets or sets the database connection string
        /// </summary>
        public static string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database connection
        /// </summary>
        public NpgsqlConnection? Connection { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="QueryBase"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string to use.</param>
        /// <param name="logConnectionsToDb">Whether to log the db connections</param>
        public QueryBase(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new NpgsqlConnection(ConnectionString);

            FluentMapper.Initialize(config =>
            {
                // Setup the fluent maps
                try { config.AddMap(new AttributeGroupMap()); } catch {}
                try { config.AddMap(new LayerAttributeMap()); } catch { }
                try { config.AddMap(new LayerMap()); } catch { }
                try { config.AddMap(new LayerGroupMap()); } catch { }
                try { config.AddMap(new MapConfigurationMap()); } catch { }
                try { config.AddMap(new MapViewMap()); } catch { }
                try { config.AddMap(new StyleFillMap()); } catch { }
                try { config.AddMap(new StyleMap()); } catch { }
                try { config.AddMap(new StyleMarkerMap()); } catch { }
                try { config.AddMap(new StyleRuleMap()); } catch { }
                try { config.AddMap(new StyleStrokeMap()); } catch { }
                try { config.AddMap(new StyleRuleConditionMap()); } catch { }
            });
            
            // There is an issue with the FluentMapper that does not allow us to Add a Map multiple
            // times, however we cannot simply check for empty since we add in several versions of QueryBase
            // As a workaround I have wrapped each call in a try catch that silently catches the error thrown
            // by Dapper FluentMap because it only errors when it finds a duplicate
            // if (FluentMapper.EntityMaps.IsEmpty)
            //{
            //    FluentMapper.Initialize(config =>
            //    {
            //        // Setup the fluent maps
            //        try
            //        {
            //            config.AddMap(new AttributeGroupMap());
            //            config.AddMap(new LayerAttributeMap());
            //            config.AddMap(new LayerMap());
            //            config.AddMap(new LayerGroupMap());
            //            config.AddMap(new MapConfigurationMap());
            //            config.AddMap(new MapViewMap());
            //            config.AddMap(new StyleFillMap());
            //            config.AddMap(new StyleMap());
            //            config.AddMap(new StyleMarkerMap());
            //            config.AddMap(new StyleRuleMap());
            //            config.AddMap(new StyleStrokeMap());
            //            config.AddMap(new StyleRuleConditionMap());
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex, "An error occurred setting up the Dapper FluentMapper");
            //            throw;
            //        }
            //    });
            //}
          /* else  // Unfortunately this did not work - not sure why, possibly to do with the typeof but I tried Model.Dtos.<type> also
            {
                FluentMapper.EntityMaps.GetOrAdd(typeof(AttributeGroupMap), new AttributeGroupMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(LayerAttributeMap), new LayerAttributeMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(LayerGroupMap), new LayerGroupMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(LayerMap), new LayerMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(MapConfigurationMap), new MapConfigurationMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(MapViewMap), new MapViewMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(SourcePropertyMap), new SourcePropertyMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(StyleFillMap), new StyleFillMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(StyleMap), new StyleMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(StyleMarkerMap), new StyleMarkerMap());
                FluentMapper.EntityMaps.GetOrAdd(typeof(StyleStrokeMap), new StyleStrokeMap());
            }*/

            // Exit if logging connections is not required
            if (!_logConnectionsToDb) return;

            // Set an id on QueryBase from a random number
            Random rnd = new();
            _id = rnd.Next(0, int.MaxValue);
            string logId = string.Format(OutputFormat, GetType().Name, _id);

            DbConnectionMonitor.UpdateConnection(logId, "Open");
        }

        /// <summary>
        /// Dispose the connection.
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }

            // Exit if logging connections is not required
            if (!_logConnectionsToDb) return;

            string id = string.Format(OutputFormat, GetType().Name, _id);
            DbConnectionMonitor.UpdateConnection(id, "Closed");
        }
    }
}
