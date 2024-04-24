namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IOperatorService
    {
        public Guid ExecuteOperator(string minerId, string resourceId);
    }
}
