using DotnetEfCompiledQueries.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace DotnetEfCompiledQueries.Data;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CompiledQueries;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}
