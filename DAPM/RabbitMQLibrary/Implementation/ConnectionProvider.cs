using RabbitMQ.Client;
using RabbitMQLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RabbitMQLibrary.Implementation
{
    internal sealed class ConnectionProvider : IDisposable, IConnectionProvider
    {
        private readonly ILogger<ConnectionProvider> _logger;
        private readonly IAsyncConnectionFactory _connectionFactory;
        private IConnection _connection;

        public ConnectionProvider(ILogger<ConnectionProvider> logger, IAsyncConnectionFactory connectionFactory) 
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }
        public IConnection GetConnection()
        {
            if(_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
                _logger.LogDebug("A RabbitMQ connection has been opened");
            }

            return _connection;
        }

        public void Dispose()
        {
            try
            {
                if (_connection != null && _connection.IsOpen)
                {
                    _logger.LogDebug("Closing the connection");
                    _connection?.Close();
                    _connection?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMq channel or connection");
            }

        }
    }
}
