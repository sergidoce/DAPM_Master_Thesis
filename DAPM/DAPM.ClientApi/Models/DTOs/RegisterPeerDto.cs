namespace DAPM.ClientApi.Models.DTOs
{
    public class RegisterPeerDto
    {
        public string PeerName { get; set; }    
        public string IntroductionPeerAddress { get; set; }
        public string LocalPeerAddress { get; set; }
    }
}
