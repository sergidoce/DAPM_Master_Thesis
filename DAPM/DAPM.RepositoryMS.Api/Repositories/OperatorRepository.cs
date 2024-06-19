using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class OperatorRepository : IOperatorRepository
    {
        private ILogger<OperatorRepository> _logger;
        private readonly RepositoryDbContext _repositoryDbContext;

        public OperatorRepository(ILogger<OperatorRepository> logger, RepositoryDbContext repositoryDbContext)
        {
            _logger = logger;
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<Operator> AddOperator(Operator op)
        {
            await _repositoryDbContext.Operators.AddAsync(op);
            _repositoryDbContext.SaveChanges();
            return op;
        }

        public Task<Operator> GetOperatorById(Guid repositoryId, Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.PostgreSQL.File> GetOperatorFiles(Guid repositoryId, Guid resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
