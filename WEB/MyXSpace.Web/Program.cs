using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Autofac.Extensions.DependencyInjection;

namespace MyXSpace.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()) //it neede for Autofac in Core 3+
                //.ConfigureServices(services => services.AddAutofac()) //NOTE: If Autofac modules will be integrated (https://stackoverflow.com/questions/56385277/configure-autofac-in-asp-net-core-3-0-preview-5-or-higher)
                //.UseAutofacMultitenantRequestServices(() => Startup.ApplicationContainer)
                //.ConfigureLogging((hostingContext, logging) =>
                // {
                //     // Requires `using Microsoft.Extensions.Logging;`
                //     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //     logging.AddConsole();
                //     logging.AddDebug();
                //     logging.AddEventSourceLogger();
                // })
                //.ConfigureLogging(logging =>
                //{
                //    // clear default logging providers
                //    logging.ClearProviders();

                //    // add built-in providers manually, as needed 
                //    logging.AddConsole();
                //    logging.AddDebug();
                //    logging.AddEventLog();
                //    logging.AddEventSourceLogger();
                //    logging.AddTraceSource(sourceSwitchName);
                //});

                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    var env = hostingContext.HostingEnvironment;

                //    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                //    if (env.IsDevelopment())
                //    {
                //        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                //        if (appAssembly != null)
                //        {
                //            config.AddUserSecrets(appAssembly, optional: true);
                //        }
                //    }
                //    config.AddEnvironmentVariables();
                //    if (args != null)
                //    {
                //        config.AddCommandLine(args);
                //    }
                //})

                .UseIISIntegration()

                .UseApplicationInsights()
                .UseUrls("http://localhost:56876/", "myadequat.fr", "","")
                .UseStartup<Startup>();
    }
}
