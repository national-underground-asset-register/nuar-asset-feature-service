// <copyright file="QueryBase.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration
{
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
        public QueryBase(string connectionString)
        {
            // Set the connection string property
            ConnectionString = connectionString;

            // Create the new NpgsqlConnection instance
            Connection = new NpgsqlConnection(ConnectionString);
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
        }
    }
}
