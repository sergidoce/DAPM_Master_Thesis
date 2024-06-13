namespace DAPM.ClientApi.Services.Interfaces
{
    public interface ISystemService
    {
        public Guid StartCollabHandshake(string introductionPeerDomain);
    }
}
