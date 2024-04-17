using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Implementation
{
    internal class ChannelProvider : IDisposable, IChannelProvider
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ILogger<ChannelProvider> _logger;
        private IModel _model;

        public ChannelProvider(IConnectionProvider connectionProvider, ILogger<ChannelProvider> logger)
        {
            _connectionProvider = connectionProvider;
            _logger = logger;
        }

        public IModel GetChannel()
        {
            if (_model == null || !_model.IsOpen)
            {
                _logger.LogDebug("Open RabbitMQ channel");
                _model = _connectionProvider.GetConnection().CreateModel();
                _logger.LogDebug($"Created RabbitMQ channel {_model.ChannelNumber}");
            }

            return _model;

        }

        public void Dispose()
        {
            try
            {
                if (_model != null && _model.IsOpen)
                {
                    _logger.LogDebug($"Closing RabbitMQ channel {_model.ChannelNumber}");
                    _model?.Close();
                    _model?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMq channel or connection");
            }

        }
    }
}
