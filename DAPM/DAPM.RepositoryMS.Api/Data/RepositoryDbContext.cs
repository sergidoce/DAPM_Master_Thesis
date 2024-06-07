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
        public DbSet<Pipeline> Pipelines { get; set; }


        public void InitializeDatabase()
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.EnsureDeleted();
                Database.Migrate();

                Repository repository = new Repository()
                {
                    Id = new Guid("8746e302-e56e-46d2-83a2-dda343689a77"),
                    Name = "DTU Repository"
                };

                Repositories.Add(repository);

                SaveChanges();
            }
        }
    }
}
