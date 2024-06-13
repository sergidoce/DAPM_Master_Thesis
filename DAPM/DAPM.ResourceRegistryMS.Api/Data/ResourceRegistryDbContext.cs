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

     
            SaveChanges();
        }
    }
}
