namespace DAPM.ClientApi.Services.Interfaces
{
    public interface ISystemService
    {
        public Guid RegisterPeer(string peerName, string introductionPeerAddress, string localPeerAddress);
    }
}
