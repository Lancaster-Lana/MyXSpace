using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AspNet.Security.OpenIdConnect.Primitives;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
//using Autofac;
//using Autofac.Extensions.DependencyInjection;
using OpenIddict.Abstractions; //Or IdentityServer4
using MyXSpace.Core;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Permissions;
using MyXSpace.Core.Permissions.Authorization;
using MyXSpace.Core.Services;
using MyXSpace.Web.Helpers;
using MyXSpace.Web.ViewModels;
using MyXSpace.EF;
using MyXSpace.AppServices.Sessions;
using MyXSpace.Core.Base;
using MyXSpace.Core.Repositories.Interfaces;
using MyXSpace.Core.Repositories;
using MyXSpace.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using MyXSpace.AppServices.MultiTenancy;

namespace MyXSpace.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private static string _currentTenantName;

        /// <summary>
        /// Return tenant (brand name from current session or config file)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTenant()
        {
            //switch(Session.TenantName) //var user = User.Identity.GetUserId()
            //_tenantId = _httpContextAccessor.HttpContext.User.FindFirst(AzureAdClaimTypes.TenantId)?.Value;
            return _currentTenantName;   //return BrandName.MyAdsearch;
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //configured tenant for which DB migrations will be applied
            _currentTenantName = Configuration["CurrentBrand"];

            //Register auto mapper for models<->entities 
           // MappingConfig.InitializeAutoMapper().CreateMapper();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMultitenancy<Tenant, CachingAppTenantResolver>();
            //services.Configure<MultitenancyOptions>(Configuration.GetSection("Multitenancy"));

            //Init Mapper profiles 
            //services.AddAutoMapper();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            //INIT Swagger for API endpoints testing
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web API - MyXSpace", Version = "v1" });
                //c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            services.AddSwaggerGen();

            //LOGGING
            //RegisterAppInsights(services);
            services.AddApplicationInsightsTelemetry(Configuration);

            // Ask Cookie Policy
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //Add SESSION support
            services.AddDistributedMemoryCache();  // Add in-memory implementation of IDistributedCache:

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Adequat.MyX.Web.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Add SPA support
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";   // In production, the Angular files will be served from this directory
            });

            #region !! TO_BE moved to AIP (based on IdentityServer 4) 

            //DB CONNECTIONS : CATALOG\TENANT
            //1.1.common CatalogDB (with tenants list)
            string catalogDBConnectionName = "MyXCatalog_DBConnection";
            string catalogDBConnectionString = Configuration.GetConnectionString(catalogDBConnectionName);

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(catalogDBConnectionString, c => c.MigrationsAssembly("MyXSpace.EF"));
            });
            //1.2 for current tenant (tenant associated with brand)
            string brandName = GetCurrentTenant(); // TODO: from session: default - "MyAdseach"
            string tenantDBConnectionName = string.Format("{0}_DBConnection", brandName);
            string tenantDBConnectionString =
                Configuration.GetConnectionString(tenantDBConnectionName) ??
                string.Format(Configuration["ConnectionStrings:TenantDefault_DBConnection"], brandName);

            //TODO: context will be detected in run-time (url or select in ser login page)
            services.AddDbContext<TenantDbContext>(options =>
            {
                options.UseSqlServer(tenantDBConnectionString, t => t.MigrationsAssembly("MyXSpace.EF"));
                //options.UseOpenIddict();
            });

            // Register the OpenIddict OIDC services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<TenantDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/connect/token");// Enable the token endpoint (required to use the password flow).
                    options.AllowPasswordFlow();// Allow client applications to use the grant_type=password flow.
                    options.AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();// Accept token requests that don't specify a client_id.
                    options.DisableHttpsRequirement(); //During development, you can disable the HTTPS requirement. Note: Comment this out in production
                    options.RegisterScopes(
                        OpenIdConnectConstants.Scopes.OpenId,
                        OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Roles);

                    // options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                    // Note: to use JWT access tokens instead of the default encrypted format, the following lines are required:
                    // options.UseJsonWebTokens();
                })
                .AddValidation(); //Only compatible with the default token format. For JWT tokens, use the Microsoft JWT bearer handler

            //User Identity\Role will be integrated in TenantDbContext
            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<TenantDbContext>()
                    .AddDefaultTokenProviders()
                    .AddUserManager<UserManager<AppUser>>() //.AddUserStore<TenantDbContext>() //this one provides data storage for user.
                     //.AddRoleManager<UserManager<AppRole>>() //.AddRoleStore<AuthDbContext>()
                    .AddSignInManager<SignInManager<AppUser>>();
            //.AddOpenIddict();

            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //// Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = false;

                //// Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            #endregion

            //Todo: ***Using DataAnnotations for validation until Swashbuckle supports FluentValidation***
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            //.AddJsonOptions(opts =>
            //{
            //    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});

            // Add CORS
            services.AddCors();

            //TODO: create AUTHORIZATION rules
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ViewAllUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers));
                options.AddPolicy(Policies.ManageAllUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers));

                options.AddPolicy(Policies.ViewAllRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles));
                options.AddPolicy(Policies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
                options.AddPolicy(Policies.ManageAllRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageRoles));
                options.AddPolicy(Policies.AssignAllowedRolesPolicy, policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
            });
            // Authorization Handlers
            services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

            //2. Add Services dependencies
            // Infrastructure module : repositories resolving
            services.AddScoped<DbContext, CatalogDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(CRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(CRepository<,>));
            services.AddScoped(typeof(IRepository<Tenant, int>), typeof(CRepository<Tenant, int>));

            services.AddScoped<IdentityDbContext<AppUser, AppRole, string>, TenantDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(TRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(TRepository<,>));

            //Domain services
            services.AddScoped<ITenantManager, TenantManager>();
            services.AddScoped<IAccountManager, AccountManager<TenantDbContext>>();     //services.AddSingleton<AccountManager>()  

            //App Services 
            services.AddScoped<ITenantAppService, TenantAppService>(); //TODO: singleton
            services.AddScoped<ISessionAppService, SessionAppService>(); //TODO: singleton
            //services.AddTransient<IProfileService, ProfileService>();

            //Creation and Seeding DB
            services.AddTransient<ITenantDatabaseInitializer, TenantDatabaseInitializer>();
            services.AddTransient<ICatalogDatabaseInitializer, CatalogDatabaseInitializer>();
            //services.AddTransient<ITenantProvider, DatabaseTenantProvider>();

            //Other services
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<SmtpConfig>(Configuration.GetSection("SmtpConfig"));
            services.AddScoped<IUnitOfWork, HttpUnitOfWork>();

            //!-- Autofac Module int --
            //var builder = new ContainerBuilder();
            //ConfigureContainer(builder);
            //builder.Populate(services);
            //ApplicationContainer = builder.Build();

            ////var strategy = new XTenantIdentificationStrategy();
            ////var mtc = new MultitenantContainer(strategy, builder.Build());
            ////ApplicationContainer = mtc;

            //return new AutofacServiceProvider(ApplicationContainer);
        }

        //public IContainer ApplicationContainer { get; private set; }

        //Autofac modules registration . 
        //ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. 
        /*public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterModule<CoreModule>();
            //builder.RegisterModule<ServiceModule>();
            //builder.RegisterModule<WebApiModule>();
        }*/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ILogger<Startup> logger
           //ITenantManager tenantManager, ITenantDatabaseInitializer databaseInitializer, 
           //ISessionAppService sessionService
           )
        {
            //app.UseMultitenancy<AppTenant>();
            //app.UseTenantContainers<AppTenant>();

            //Get current tenant name - GetCurrentTenant(); 
            //* if it signedIn - get from session; if not - from configuration file (it can be defined as "CurrentBrand". Tenant name associated with Brand name)
            //string tenantName = sessionService.GetCurrentTenant()?.Name
            //                   ?? Configuration["CurrentBrand"];

            //var catalog = app.ApplicationServices.GetService<CatalogDbContext>();
            // ITenantManager tenantManager = app.ApplicationServices.GetService<ITenantManager>();
            // IUserAppService sessionService = app.ApplicationServices.GetService<IUserAppService>();

            //Utilities.ConfigureLogger(loggerFactory);

            //Write SERILOG errors to file
            var file = Path.Combine(Directory.GetCurrentDirectory(), Configuration["Logging:LogFileName"] ?? "Log_MyXSpace.log");
            loggerFactory.AddFile(file); //loggerFactory.AddFile(Configuration.GetSection("Logging"));
            var fileLogger = loggerFactory.CreateLogger("FileLogger");
            //logger = loggerFactory.CreateLogger<LogTenantMiddleware>();

            //App Insignts
            //loggerFactory.AddAzureWebAppDiagnostics();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                fileLogger.LogDebug("Started dubug mode");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            EmailTemplates.Initialize(env);

            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseHttpsRedirection();
            //app.UseRouting();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //Inject session support (to get current user, tenant)
            app.UseSession(); // NOTE: session  MUST go before UseMvc()

            app.UseAuthentication();  //app.UseCookieAuthentication();
            logger.LogDebug("Added UseAuthentication in the MyXSpace.Web ");
            //app.UseAuthorization();

            // app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            ////Localization support : https://gunnarpeipman.com/aspnet/aspnet-core-simple-localization/
            //var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(options.Value);

            //Default MVC routing 
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "LocalizedDefault",
                //    template: "{lang:lang}/{controller=Home}/{action=Index}/{id?}"
                //);

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //init SPA startup
            app.UseSpa(spa =>
            {
                // serving an Angular SPA from ASP.NET Core,see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    spa.Options.StartupTimeout = TimeSpan.FromSeconds(120); // Increase the timeout if angular app is taking longer to startup
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Use this instead to use the angular cli server
                }
            });


            //Swagger API - enable middleware to serve generated JSON endpoints.
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "Swagger UI for MyXSpace";
                    c.RoutePrefix = "swagger";// under url '/swagger' will be swagger generated docs  
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TT V1");
                });
        }

        //private void RegisterAppInsights(IServiceCollection services)
        //{
        //    services.AddApplicationInsightsTelemetry(Configuration);
        //    var orchestratorType = Configuration.GetValue<string>("OrchestratorType");

        //    if (orchestratorType?.ToUpper() == "K8S")
        //    {
        //        // Enable K8s telemetry initializer
        //        services.EnableKubernetes();
        //    }
        //    if (orchestratorType?.ToUpper() == "SF")
        //    {
        //        // Enable SF telemetry initializer
        //        services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
        //            new FabricTelemetryInitializer());
        //    }
        //}
    }


    /*
    public class CachingAppTenantResolver : MemoryCacheTenantResolver<Tenant>
    {
        private readonly IEnumerable<Tenant> tenants;

        public CachingAppTenantResolver(IMemoryCache cache, ILoggerFactory loggerFactory, IOptions<MultitenancyOptions> options)
            : base(cache, loggerFactory)
        {
            this.tenants = options.Value.Tenants;
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            return context.Tenant.Hostnames;
        }

        protected override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            TenantContext<AppTenant> tenantContext = null;

            var tenant = tenants.FirstOrDefault(t =>
                t.Hostnames.Any(h => h.Equals(context.Request.Host.Value.ToLower())));

            if (tenant != null)
            {
                tenantContext = new TenantContext<AppTenant>(tenant);
            }

            return Task.FromResult(tenantContext);
        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return base.CreateCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        }

        
    }*/
}
