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
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Peer> Peers { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Repository>().HasKey(r => new { r.PeerId, r.Id });
        builder.Entity<Resource>().HasKey(r => new { r.PeerId, r.RepositoryId, r.Id });
    }

    public void InitializeDatabase()
    {
        Database.EnsureDeleted();
        Database.Migrate();

        ResourceType csv = new ResourceType { Name = "EventLog", FileExtension = ".csv" };
        ResourceTypes.Add(csv);

        Peer dtuPeer = new Peer { Name = "DTU", ApiUrl = "http://dtu.dk" };
        Peer kuPeer = new Peer { Name = "KU", ApiUrl = "http://ku.dk" };
        Peer nvPeer = new Peer { Name = "Novo Nordisk", ApiUrl = "http://novonordisk.dk" };

        Peers.Add(dtuPeer);
        Peers.Add(kuPeer);
        Peers.Add(nvPeer);


        Repository dtuRepo = new Repository { Name = "DTU Repository", Peer = dtuPeer };
        Repository kuRepo = new Repository { Name = "KU Repository", Peer = kuPeer };
        Repository nvRepo = new Repository { Name = "NovoNordisk Repository", Peer = nvPeer };

        Repositories.Add(dtuRepo);
        Repositories.Add(kuRepo);
        Repositories.Add(nvRepo);

        Resource dtuResource = new Resource { Name = "EventLogDtu", Repository = dtuRepo, ResourceType = csv };
        Resource kuResource = new Resource { Name = "EventLogKu", Repository = kuRepo, ResourceType = csv };
        Resource nvResource = new Resource { Name = "EventLogNn", Repository = nvRepo, ResourceType = csv };

        Resources.Add(dtuResource);
        Resources.Add(kuResource);
        Resources.Add(nvResource);

        SaveChanges();
    }


}
