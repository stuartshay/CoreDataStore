using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using App.Metrics;
using App.Metrics.Meter;
using CoreDataStore.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreDataStore.Web.Controllers
{
    /// <summary>
    /// Server Status API Controller
    /// </summary>
    //[EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class DiagnosticsController : Controller
    {
        private static readonly Random Rnd = new Random();

        private readonly IWebHostEnvironment _env;

        //private readonly IMetrics _metrics;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsController"/> class.
        /// </summary>
        /// <param name="env"></param>
        public DiagnosticsController(IWebHostEnvironment env)//(IWebHostingEnvironment env, IMetrics metrics)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            //_metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        /// <summary>
        ///  Heartbeat
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        public IActionResult Status() => Ok();

        /// <summary>
        ///  Get Server Diagnostics
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServerDiagnostics), 200)]
        [Produces("application/json", Type = typeof(ServerDiagnostics))]
        public IActionResult Get()
        {
            Log.Information($"Init:ServerDiagnostics:{DateTime.UtcNow}");

            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var diagnostics = new ServerDiagnostics
            {
                MachineDate = DateTime.Now,
                MachineName = Environment.MachineName,
                MachineCulture = $"{CultureInfo.CurrentCulture.DisplayName} - {CultureInfo.CurrentCulture.Name}",

                Platform = osNameAndVersion.Trim(),
                DnsHostName = Dns.GetHostName(),
                WorkingDirectory = null,
                ContentRootPath = _env.ContentRootPath,
                WebRootPath = _env.WebRootPath,
                ApplicationName = _env.ApplicationName,
                EnvironmentName = _env.EnvironmentName,
                Runtime = GetNetCoreVersion(),
            };

            diagnostics.MachineTimeZone = TimeZoneInfo.Local.IsDaylightSavingTime(diagnostics.MachineDate) ? TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName;
            diagnostics.ApplicationVersionNumber = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            var ipAddresses = Dns.GetHostAddressesAsync(diagnostics.DnsHostName).Result.Where(x => x.AddressFamily == AddressFamily.InterNetwork).AsEnumerable().Distinct();

            var enumerable = ipAddresses as IPAddress[] ?? ipAddresses.ToArray();
            var ipList = new List<string>(enumerable.Count());
            foreach (var ipAddress in enumerable)
            {
                ipList.Add(ipAddress.ToString());
            }

            diagnostics.IpAddressList = ipList;

            return Ok(diagnostics);
        }

        /// <summary>
        /// Metrics Test Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("metrics/")]
        public IActionResult GetMetrics()
        {
            var httpStatusMeter = new MeterOptions
            {
                Name = "Http Status",
                MeasurementUnit = Unit.Calls,
            };

            //_metrics.Measure.Meter.Mark(httpStatusMeter, 70, "200");
            //_metrics.Measure.Meter.Mark(httpStatusMeter, 10, "500");
            //_metrics.Measure.Meter.Mark(httpStatusMeter, 20, "401");

            //// Test Counter Sample
            //_metrics.Measure.Counter.Increment(MetricsRegistry.Counters.TestCounter);
            //_metrics.Measure.Counter.Increment(MetricsRegistry.Counters.TestCounter, 4);
            //_metrics.Measure.Counter.Decrement(MetricsRegistry.Counters.TestCounter, 2);

            //var process = Process.GetCurrentProcess();
            //var physicalMemoryGauge = new FunctionGauge(() => process.WorkingSet64);

            //_metrics.Measure.Gauge.SetValue(MetricsRegistry.Gauges.TestGauge, () => physicalMemoryGauge.Value);

            //_metrics.Measure.Gauge.SetValue(
            //    MetricsRegistry.Gauges.DerivedGauge,
            //    () => new DerivedGauge(physicalMemoryGauge, g => g / 1024.0 / 1024.0));

            //var cacheHits = _metrics.Provider.Meter.Instance(MetricsRegistry.Meters.CacheHits);
            //var calls = _metrics.Provider.Timer.Instance(MetricsRegistry.Timers.DatabaseQueryTimer);

            //var cacheHit = Rnd.Next(0, 2) == 0;
            //if (cacheHit)
            //{
            //    cacheHits.Mark();
            //}

            //using (calls.NewContext())
            //{
            //    Thread.Sleep(cacheHit ? 10 : 100);
            //}

            //using (_metrics.Measure.Apdex.Track(MetricsRegistry.ApdexScores.TestApdex))
            //{
            //    Thread.Sleep(cacheHit ? 10 : 100);
            //}

            //_metrics.Measure.Gauge.SetValue(MetricsRegistry.Gauges.CacheHitRatioGauge, () => new HitRatioGauge(cacheHits, calls, m => m.OneMinuteRate));

            //var histogram = _metrics.Provider.Histogram.Instance(MetricsRegistry.Histograms.TestHAdvancedistogram);
            //histogram.Update(Rnd.Next(1, 20));

            //_metrics.Measure.Histogram.Update(MetricsRegistry.Histograms.TestHistogram, Rnd.Next(20, 40));

            //_metrics.Measure.Timer.Time(MetricsRegistry.Timers.TestTimer, () => Thread.Sleep(20), "value1");
            //_metrics.Measure.Timer.Time(MetricsRegistry.Timers.TestTimer, () => Thread.Sleep(25), "value2");

            //using (_metrics.Measure.Timer.Time(MetricsRegistry.Timers.TestTimerTwo))
            //{
            //    Thread.Sleep(15);
            //}

            //using (_metrics.Measure.Timer.Time(MetricsRegistry.Timers.TestTimerTwo, "value1"))
            //{
            //    Thread.Sleep(20);
            //}

            //using (_metrics.Measure.Timer.Time(MetricsRegistry.Timers.TestTimerTwo, "value2"))
            //{
            //    Thread.Sleep(25);
            //}

            return Ok();
        }

        private static string GetNetCoreVersion()
        {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }
    }
}
