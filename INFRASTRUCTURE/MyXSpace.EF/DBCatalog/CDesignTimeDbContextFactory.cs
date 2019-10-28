
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyXSpace.EF
{  
    /// <summary>
    /// Will create initial tenant DB for <>
    /// Get DB connection strings in appsettings.config
    /// </summary>
    public class CDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    { 
        public CatalogDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            string catalogDBConn = string.Format("ConnectionStrings:MyXCatalog_DBConnection");

            string connectionStr = configuration[catalogDBConn];

            var builder = new DbContextOptionsBuilder<CatalogDbContext>();
            builder.UseSqlServer(connectionStr);
            //builder.UseSqlServer(connectionStr, b => b.MigrationsAssembly("MyXSpace.EF"));

            return new CatalogDbContext(builder.Options);
        }
    }
}