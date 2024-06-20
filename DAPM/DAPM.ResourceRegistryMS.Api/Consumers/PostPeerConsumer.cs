using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class PostPeerConsumer : IQueueConsumer<PostPeerMessage>
    {
        private IPeerService _peerService;

        public PostPeerConsumer(IPeerService peerService)
        {
            _peerService = peerService;
        }

        public async Task ConsumeAsync(PostPeerMessage message)
        {
            await _peerService.PostPeer(message.Organization);
            return;
        }
    }
}
