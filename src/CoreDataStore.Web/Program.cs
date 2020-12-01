using System;
using System.Diagnostics;
using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using CoreDataStore.Web.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Graylog;

namespace CoreDataStore.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// IConfiguration
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();


        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        ///// <summary>
        ///// Main Entry Point
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static int Main(string[] args)
        //{
        //    var config = Configuration.Get<ApplicationOptions>();

        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Information()
        //        .WriteTo.Graylog(new GraylogSinkOptions
        //        {
        //            HostnameOrAddress = config.Graylog.Host,
        //            Port = 12201,
        //        })
        //        .CreateLogger();

        //    Serilog.Debugging.SelfLog.Enable(msg =>
        //    {
        //        Debug.Print(msg);
        //        Debugger.Break();
        //    });

        //    try
        //    {
        //        Log.Information("Init:CoreDataStore.Web");
        //        CreateWebHostBuilder(args).Build().Run();
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //        return 1;
        //    }
        //}

        ///// <summary>
        ///// Create WebHost Builder
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    return WebHost.CreateDefaultBuilder(args)
        //        .ConfigureMetricsWithDefaults(
        //            builder => builder.OutputMetrics.AsPrometheusPlainText())
        //        .UseMetrics(
        //            options =>
        //            {
        //                options.EndpointOptions = endpointsOptions =>
        //                {
        //                    endpointsOptions.MetricsTextEndpointOutputFormatter =
        //                        new MetricsPrometheusTextOutputFormatter();
        //                };
        //            })
        //        .UseStartup<Startup>()
        //        .CaptureStartupErrors(true)
        //        .UseConfiguration(Configuration);
        //}
    }
}
