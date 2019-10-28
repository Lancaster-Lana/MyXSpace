using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyXSpace.Core.Base;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Helpers;
using MyXSpace.Core.Interfaces;
using MyXSpace.Core.Services;
using MyXSpace.EF;

namespace MyXSpace.DBMigrator
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;

            //configured tenant for which DB migrations will be applied
            _currentTenantName = Configuration["CurrentBrand"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //1.1.common CatalogDB (with tenants list)
            string catalogDBConnectionName = "MyXCatalog_DBConnection";
            string catalogDBConnectionString = Configuration.GetConnectionString(catalogDBConnectionName);
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(catalogDBConnectionString, b => b.MigrationsAssembly("MyXSpace.EF"));
            });

            //1.2 current tenant\brand for DB seeding  (NOTE: different for each brand)
            string brandName = GetCurrentTenant();
            string tenantDBConnectionName = string.Format("{0}_DBConnection", brandName);
            string tenantDBConnectionString =
                Configuration.GetConnectionString(tenantDBConnectionName) ??
                string.Format(Configuration["ConnectionStrings:TenantDefault_DBConnection"],  brandName);

            services.AddDbContext<TenantDbContext>(options =>
            {
                options.UseSqlServer(tenantDBConnectionString, b => b.MigrationsAssembly("MyXSpace.EF"));
                options.UseOpenIddict();
            });

            //User Identity\Role will be intgrated in TenantDbContext
            services.AddIdentity<AppUser, AppRole>()//.AddOpenIddict();
                .AddEntityFrameworkStores<TenantDbContext>()
                    .AddUserManager<UserManager<AppUser>>()  //.AddUserStore<TenantDbContext>() //this one provides data storage for user.
                    //.AddRoleStore<AuthDbContext>()//.AddRoleManager<UserManager<AppRole>>()
                    .AddSignInManager<SignInManager<AppUser>>()
                    .AddDefaultTokenProviders();
               
            //2. Resolve services dependencies
            //services.AddScoped<IdentityDbContext<AppUser, AppRole, int>, TenantDbContext>();
            //services.AddScoped<DbContext, CatalogDbContext>();
            services.AddScoped<IAccountManager, AccountManager<TenantDbContext>>();

            services.AddScoped(typeof(IRepository<>), typeof(CRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(CRepository<,>));
            services.AddScoped(typeof(IRepository<Tenant, int>), typeof(CRepository<Tenant, int>));
            //services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(TRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(TRepository<,>));

            services.AddScoped<ITenantManager, TenantManager>();
            //services.AddTransient<ISessionAppService, SessionAppService>();

            services.AddTransient<ICatalogDatabaseInitializer, CatalogDatabaseInitializer>();
            services.AddTransient<ITenantDatabaseInitializer, TenantDatabaseInitializer>();
        }

        public void Configure(
                IApplicationBuilder app, IHostingEnvironment env,
                ILoggerFactory loggerFactory, ILogger<Startup> logger,
                ITenantManager tenantManager,               
                ICatalogDatabaseInitializer catalogDatabaseInitializer,
                ITenantDatabaseInitializer tenantDatabaseInitializer)
        {
            //Utilities.ConfigureLogger(loggerFactory);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //TODO: Hosting DB (CatalogDB) init 
            catalogDatabaseInitializer.SeedAsync().Wait();

            //get tenant\brandName from session or config file
            string tenantName = GetCurrentTenant();
            string tenantDBConnectionName = string.Format("{0}_DBConnection", tenantName);
            string tenantDBConnectionString =
                Configuration.GetConnectionString(tenantDBConnectionName) ??
                string.Format(Configuration["ConnectionStrings: TenantDefault_DBConnection"], tenantName);

            string tenantAdminPwd = Configuration["TenantAdminPassword"] ?? "Admin#123";

            try
            {
                //Check if the Tenant exists in catalogDb
                int? tenantId = tenantManager.GetByName(tenantName)?.Id;

                if (tenantId == null)
                {
                    //create Tenant in catalogDB 
                    var newTenant = new Tenant(tenantName); //new Tenant(tenantName, host, connectionStr, adminUser); 
                    newTenant.Host = Configuration["TenantHost"] ?? "127.0.0.1";//TODO from connection string
                    newTenant.ConnectionString = tenantDBConnectionString;// tenantDatabaseInitializer.ConnectionString;
                    
                    //newTenant.Admin = TODO:create adminUser for new tenant later in migrations
                    tenantManager.Register(newTenant);
                    tenantId = newTenant.Id;
                }

                //DB seeding for tenant : create tenant DB, create 'internal' roles, admin user 
                tenantDatabaseInitializer.SeedAsync(tenantId.Value, tenantAdminPwd).Wait();
            }
            catch (Exception ex)
            {
                logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);
                throw new Exception(LoggingEvents.INIT_DATABASE.Name, ex);
            }

            //Post initialize message
            app.Run(async (context) =>
            {
                string message = string.Format("Migrations for tenant '{0}' done!", tenantName);
                await context.Response.WriteAsync(message);
            });
        }

        private static string _currentTenantName;

        /// <summary>
        /// TODO: return tenant (brand name from current session or config file)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTenant()
        {
            //switch(Session.TenantName) //var user = User.Identity.GetUserId()
            //_tenantId = _httpContextAccessor.HttpContext.User.FindFirst(AzureAdClaimTypes.TenantId)?.Value;

            //Find tenant by name (in Catalog DB):
            //If exists: tenantId = TenantManager.GetTenantByName(_currentTenantName);
            //If not - must be created: tenantId = TenantManager.Register (new Tenant(tenantName, tHost, tConnectionString))

            return _currentTenantName;   //return BrandName.MyAdsearch;
        }
    }
}
