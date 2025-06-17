// <copyright file="DbConnectionMonitor.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers
{
    /// <summary>
    /// Class to monitor DB connections.
    /// </summary>
    public class DbConnectionMonitor
    {
        private static readonly ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes static members of the <see cref="DbConnectionMonitor"/> class.
        /// </summary>
        static DbConnectionMonitor()
        {
            ConnectionStates = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the Connection States dictionary.
        /// </summary>
        private static Dictionary<string, string> ConnectionStates { get; set; }

        /// <summary>
        /// Updates connection state.
        /// </summary>
        /// <param name="id">Connection Id.</param>
        /// <param name="state">Connection State.</param>
        public static void UpdateConnection(string id, string state)
        {
            try
            {
                if (state.Equals("Open"))
                {
                    // Existing connection ?
                    ConnectionStates[id] = GetTimestampedState(state);
                }
                else
                {
                    // Remove closed connections.
                    ConnectionStates.Remove(id);
                }
            }
            catch
            {
                // New connection.
                ConnectionStates.Add(id, GetTimestampedState(state));
            }

            DumpConnectionState();
        }

        /// <summary>
        /// Get timestamped state.
        /// </summary>
        /// <param name="state">The state to timestamp.</param>
        /// <returns>A timestamped state string.</returns>
        private static string GetTimestampedState(string state)
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss.fff}\t{1}", DateTime.Now, state);
        }

        /// <summary>
        /// Dump connection state to a file.
        /// </summary>
        private static void DumpConnectionState()
        {
            // Set Status to Locked
            ReadWriteLock.EnterWriteLock();

            try
            {
                using (StreamWriter wrt = new StreamWriter(".\\connectionState.txt"))
                {
                    wrt.WriteLine(string.Format("{0,40}\t{1:D10}\t{2}\t\t\t\t{3}", "Class", "Object Id", "Last Access", "State"));
                    foreach (KeyValuePair<string, string> entry in ConnectionStates)
                    {
                        wrt.WriteLine(string.Format("{0}\t{1}", entry.Key, entry.Value));
                    }
                }
            }
            finally
            {
                // Release lock
                ReadWriteLock.ExitWriteLock();
            }
        }
    }
}
