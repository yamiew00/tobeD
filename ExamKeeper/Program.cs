using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExamKeeper {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.ConfigureAppConfiguration((HostBuilderContext, config) => {
                    var env = HostBuilderContext.HostingEnvironment;
                    config.AddJsonFile(path: "appsettings.json", optional : false, reloadOnChange : true);
                    if (!string.IsNullOrWhiteSpace(env.EnvironmentName)) {
                        config.AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional : false, reloadOnChange : true);
                    }
                }).UseStartup<Startup>();
            }).ConfigureServices(services => {
                services.AddHostedService<ExamKeeper.Models.ResourceCache>(); //加入Timer
            });
    }
}