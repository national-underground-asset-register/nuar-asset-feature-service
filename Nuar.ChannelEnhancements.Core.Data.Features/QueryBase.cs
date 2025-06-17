namespace Nuar.ChannelEnhancements.Core.Data.Features;

using Npgsql;

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
    /// Instantiates a new <see cref="QueryBase"/> and creates a connection
    /// </summary>
    /// <param name="connectionString">The database connection string to use.</param>
    public QueryBase(string connectionString)
    {
        Connection = new NpgsqlConnection(connectionString);
    }

    /// <summary>
    /// Disposes the <see cref="QueryBase"/> by cleaning up database connection
    /// </summary>
    public void Dispose()
    {
        // Do nothing is the connection is null
        if (Connection == null) return;

        // Otherwise we need to close and dispose
        Connection.Close();
        Connection.Dispose();
        Connection = null;
    }
}