namespace DAPM.PeerApi
{
    public static class PeerApiEndpoints
    {
        public static string PeerApiPort = ":5001";

        public static string HandshakeRequestEndpoint = PeerApiPort + "/handshake/request";
        public static string HandshakeRequestResponseEndpoint = PeerApiPort + "/handshake/request-response";
        public static string RegistryUpdateAckEndpoint = PeerApiPort + "/registry/update-ack";
        public static string RegistryUpdateEndpoint = PeerApiPort + "/registry/updates";
        public static string PostResourceEndpoint = PeerApiPort + "/resources";
        public static string PostResourceResultEndpoint = PeerApiPort + "/resources/result";
    }
}
