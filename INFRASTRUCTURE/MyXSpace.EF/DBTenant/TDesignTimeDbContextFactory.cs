
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
    public class TDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    { 
        public TenantDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();
     
            string brandName = configuration["CurrentBrand"];   //DB connection for current tenant (tenant associated with brand)
            //brandName = HttpUnitOfWork.CurrentTenant.Name;

            string tenantDBConn = string.Format("ConnectionStrings:{0}_DBConnection", brandName);

            string connectionStr = configuration[tenantDBConn];

            var builder = new DbContextOptionsBuilder<TenantDbContext>();
            builder.UseSqlServer(connectionStr);
            //builder.UseSqlServer(connectionStr, b => b.MigrationsAssembly("MyXSpace.EF"));

            return new TenantDbContext(builder.Options);
        }
    }
}
