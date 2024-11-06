using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories;
namespace API.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var confiugration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Development.json")
        .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
        .UseNpgsql(confiugration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("Repositories"));

        return new RepositoryContext(builder.Options);
    }
}
