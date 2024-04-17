using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Extensions
{
    public static class QueueingStartupExtensions
    {
        public static void AddQueueing(this IServiceCollection services, QueueingConfigurationSettings settings)
        {
            services.AddSingleton<QueueingConfigurationSettings>(settings);

            services.AddSingleton<IAsyncConnectionFactory>(provider =>
            {
                var factory = new ConnectionFactory
                {
                    UserName = settings.RabbitMqUsername,
                    Password = settings.RabbitMqPassword,
                    HostName = settings.RabbitMqHostname,
                    Port = settings.RabbitMqPort.GetValueOrDefault(),

                    DispatchConsumersAsync = true,
                    AutomaticRecoveryEnabled = true,

                    // Configure the amount of concurrent consumers within one host
                    ConsumerDispatchConcurrency = settings.RabbitMqConsumerConcurrency.GetValueOrDefault(),
                };

                factory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");

                return factory;
            });

            // The RabbitMQ documentation states that Connections are meant to be long lived
            // and should be used to perform all operations. We chose to implement the connection as a Singleton to ensure that.
            // In case of high concurrent usage multiple connections could be used, but for most usage one connection per host will be sufficient
            // See https://www.rabbitmq.com/dotnet-api-guide.html#connecting and https://www.rabbitmq.com/dotnet-api-guide.html#concurrency-thread-usage
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            // The RabbitMQ documentation states that IModels (or Channels) should not be used between threads simultaniously.
            // When using transactions in the consumers, giving each scope its own Channel will insure reliability in the processing of a Queue message
            // See https://www.rabbitmq.com/dotnet-api-guide.html#concurrency-channel-sharing
            services.AddScoped<IChannelProvider, ChannelProvider>();
            services.AddScoped(typeof(IQueueChannelProvider<>), typeof(QueueChannelProvider<>));

            services.AddScoped(typeof(IQueueProducer<>), typeof(QueueProducer<>));
        }
        public static void AddQueueMessageConsumer<TMessageConsumer, TQueueMessage>(this IServiceCollection services) where TMessageConsumer : IQueueConsumer<TQueueMessage> where TQueueMessage : class, IQueueMessage
        {
            services.AddScoped(typeof(TMessageConsumer));
            services.AddScoped<IQueueConsumerHandler<TMessageConsumer, TQueueMessage>, QueueConsumerHandler<TMessageConsumer, TQueueMessage>>();
            services.AddHostedService<QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage>>();
        }
    }

}
