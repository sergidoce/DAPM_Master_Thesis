using Microsoft.EntityFrameworkCore;
using DAPM.ResourceRegistryMS.Api.Models;

public class ResourceRegistryDbContext : DbContext
{
    ILogger<ResourceRegistryDbContext> _logger;
    public ResourceRegistryDbContext(DbContextOptions<ResourceRegistryDbContext> options, ILogger<ResourceRegistryDbContext> logger) 
        : base(options)
    {
        _logger = logger;
        InitializeDatabase();
    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Peer> Peers { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Repository>().HasKey(r => new { r.PeerId, r.Id });
        builder.Entity<Resource>().HasKey(r => new { r.PeerId, r.RepositoryId, r.Id });
        builder.Entity<Pipeline>().HasKey(r => new { r.PeerId, r.RepositoryId, r.Id });
    }

    public void InitializeDatabase()
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.EnsureDeleted();
            Database.Migrate();

            ResourceType csv = new ResourceType {Id = Guid.Empty, Name = "EventLog", FileExtension = ".csv" };
            ResourceTypes.Add(csv);

            Peer dtuPeer = new Peer { Id = new Guid("43b2c65f-f82c-4aff-b049-ccdac4e02671"), Name = "DTU", Domain = "http://dtu.dk" };
       

            Peers.Add(dtuPeer);
            Repository dtuRepo = new Repository { Id = new Guid("8746e302-e56e-46d2-83a2-dda343689a77"), Name = "DTU Repository", PeerId = dtuPeer.Id };
       

            Repositories.Add(dtuRepo);

            SaveChanges();
        }
    }
}
