using StackExchange.Redis;
using System.Collections.Concurrent;

namespace ApiGetWay.Common
{
    /// <summary>
    /// Redis class
    /// </summary>
    public class RedisHelper : IDisposable
    {
        // Connection string
        private string _connectionString;
        // Instance name
        private string _instanceName;
        // Default database
        private int _defaultDB;

        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        /// <param name="defaultDB"></param>
        public RedisHelper(string connectionString, string instanceName, int defaultDB = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDB = defaultDB;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        /// <summary>
        /// Get ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        /// <summary>
        /// Get Redis database
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase(_defaultDB);
        }
        /// <summary>
        /// Get Redis server
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="endPointsIndex"></param>
        /// <returns></returns>
        public StackExchange.Redis.IServer GetServer(string? configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }
        /// <summary>
        /// Get Redis subscriber
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public ISubscriber GetSubscriber(string? configName = null)
        {
            return GetConnect().GetSubscriber();
        }
        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }

    }
}