using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace DAPM.RepositoryMS.Api.Data
{
    public class RepositoryDbContext : DbContext
    {

        ILogger<RepositoryDbContext> _logger;

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options, ILogger<RepositoryDbContext> logger) : base (options)
        {
            _logger = logger;
            InitializeDatabase();
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Models.PostgreSQL.File>  Files { get; set; }


        public void InitializeDatabase()
        {
            Database.EnsureDeleted();
            Database.Migrate();

            Repository repository = new Repository()
            {
                Id = 1,
                Name = "DTU Repository"
            };

            Repositories.Add(repository);

            SaveChanges();

        }
    }
}
