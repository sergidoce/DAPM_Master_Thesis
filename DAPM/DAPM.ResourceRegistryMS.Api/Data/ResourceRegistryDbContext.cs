using Microsoft.EntityFrameworkCore;

public class ResourceRegistryDbContext : DbContext
{
    public ResourceRegistryDbContext(DbContextOptions<ResourceRegistryDbContext> options) 
        : base(options)
    {
    }

    public DbSet<DAPM.ResourceRegistryMS.Api.Models.Resource> Resources { get; set; }
}
