using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Navigator.Common.Helpers;

namespace CoreDataStore.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables("")
                .AddCommandLine(args)
                .Build();

            var url = config["ASPNETCORE_URLS"] ?? "http://*:5000"; 
            var env = config["ASPNETCORE_ENVIRONMENT"] ?? "Development";
            var connectionPostgreSql = config["CONNECTION_PostgreSQL"];

            using (ConsoleColorContext.Create())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("ASPNETCORE_URLS:{0}", url);
                Console.WriteLine("ASPNETCORE_ENVIRONMENT:{0}", env);
                Console.WriteLine("CONNECTION_PostgreSQL:{0}", connectionPostgreSql);
            }

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseUrls(url)
                .UseEnvironment(env)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
