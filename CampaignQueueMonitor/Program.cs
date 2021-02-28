using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CampaignQueueMonitor
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var runner = serviceProvider.GetService<IRunnerService>();
            runner.Run().Wait();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole().AddDebug());

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            services.AddSingleton<IConfiguration>(configuration);

            services.AddTransient<IRunnerService, RunnerService>();
            services.AddTransient<ISendDataService, SendDataService>();
            services.AddTransient<IFetchService, FetchService>();
            services.AddTransient<IZenDesk, ZenDeskService>();
            services.AddTransient<IOtherDesk, OtherDeskService>();
        }
    }
}
