using Microsoft.EntityFrameworkCore;
using DAPM.ResourceRegistryMS.Api.Models;

public class ResourceRegistryDbContext : DbContext
{
    public ResourceRegistryDbContext(DbContextOptions<ResourceRegistryDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Peer> Peers { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
}
