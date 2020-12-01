using System;
using AutoMapper;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Interfaces;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Services;
using CoreDataStore.Web.Configuration;
using CoreDataStore.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navigator.Common.Helpers;

namespace CoreDataStore.Web
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using (ConsoleColorContext.Create())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("ASPNETCORE_URLS:{0}", Configuration["ASPNETCORE_URLS"]);
                Console.WriteLine("ASPNETCORE_ENVIRONMENT:{0}", Configuration["ASPNETCORE_ENVIRONMENT"]);
                Console.WriteLine("CONNECTION_PostgreSQL:{0}", Configuration["CONNECTION_PostgreSQL"]);
            }
        }

        /// <summary>
        ///  Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationOptions(Configuration);
            var config = Configuration.Get<ApplicationOptions>();


            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            services.AddControllers();
            services.AddSwaggerGen();

            // Staging 
            // EnvironmentVariable Exist Override
            var connection = !string.IsNullOrWhiteSpace(Configuration["CONNECTION_PostgreSQL"]) ? Configuration["CONNECTION_PostgreSQL"] : config.ConnectionStrings.PostgreSql;

            services.AddDbContext<Data.Postgre.NycLandmarkContext>(options => options.UseNpgsql(connection));

            // Repositories
            services.AddScoped<ILpcReportRepository, Data.Postgre.Repositories.LpcReportRepository>();
            services.AddScoped<ILandmarkRepository, Data.Postgre.Repositories.LandmarkRepository>();
            services.AddScoped<IPlutoRepository, Data.Postgre.Repositories.PlutoRepository>();
            services.AddScoped<IReferenceRepository, Data.Postgre.Repositories.ReferenceRepository>();

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddMaps("CoreDataStore.Service"); });
            IMapper mapper = new Mapper(mapperConfig);
            services.AddSingleton(mapper);

            // Services 
            services.AddScoped<ILpcReportService, LpcReportService>();
            services.AddScoped<ILandmarkService, LandmarkService>();
            services.AddScoped<IPlutoService, PlutoService>();
        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var swaggerDocument = $"/swagger/v1/swagger.json";
                options.SwaggerEndpoint(swaggerDocument, "CoreDataStore.Web");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }










        ///// <summary>
        ///// ConfigureServices Development - Sqlite.
        ///// </summary>
        ///// <param name="services"></param>
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    services.AddConfigurationOptions(Configuration);
        //    var config = Configuration.Get<ApplicationOptions>();

        //    var connection = config.ConnectionStrings.Sqlite;
        //    var connectionString = $"Filename={Path.GetFullPath(connection)}";

        //    services.AddDbContext<Data.Sqlite.NycLandmarkContext>(options => options.UseSqlite(connectionString));

        //    // Repositories
        //    services.AddScoped<ILpcReportRepository, Data.Sqlite.Repositories.LpcReportRepository>();
        //    services.AddScoped<ILandmarkRepository, Data.Sqlite.Repositories.LandmarkRepository>();
        //    services.AddScoped<IPlutoRepository, Data.Sqlite.Repositories.PlutoRepository>();
        //    services.AddScoped<IReferenceRepository, Data.Sqlite.Repositories.ReferenceRepository>();

        //    ConfigureServices(services);
        //}

        ///// <summary>
        ///// ConfigureServices Staging - PostgreSQL.
        ///// </summary>
        ///// <param name="services"></param>
        //public void ConfigureStagingServices(IServiceCollection services)
        //{
        //    services.AddConfigurationOptions(Configuration);
        //    var config = Configuration.Get<ApplicationOptions>();

        //    // EnvironmentVariable Exist Override
        //    var connection = !string.IsNullOrWhiteSpace(Configuration["CONNECTION_PostgreSQL"]) ? Configuration["CONNECTION_PostgreSQL"] : config.ConnectionStrings.PostgreSql;

        //    services.AddDbContext<Data.Postgre.NycLandmarkContext>(options => options.UseNpgsql(connection));

        //    // Repositories
        //    services.AddScoped<ILpcReportRepository, Data.Postgre.Repositories.LpcReportRepository>();
        //    services.AddScoped<ILandmarkRepository, Data.Postgre.Repositories.LandmarkRepository>();
        //    services.AddScoped<IPlutoRepository, Data.Postgre.Repositories.PlutoRepository>();
        //    services.AddScoped<IReferenceRepository, Data.Postgre.Repositories.ReferenceRepository>();

        //    ConfigureServices(services);
        //}

        ///// <summary>
        ///// ConfigureServices Staging - SqlServer.
        ///// </summary>
        ///// <param name="services"></param>
        //public void ConfigureProductionServices(IServiceCollection services)
        //{
        //    services.AddConfigurationOptions(Configuration);
        //    var config = Configuration.Get<ApplicationOptions>();

        //    services.AddDbContext<Data.SqlServer.NycLandmarkContext>(options => options.UseSqlServer(config.ConnectionStrings.SqlServer));

        //    // Repositories
        //    services.AddScoped<ILpcReportRepository, Data.SqlServer.Repositories.LpcReportRepository>();
        //    services.AddScoped<ILandmarkRepository, Data.SqlServer.Repositories.LandmarkRepository>();
        //    services.AddScoped<IPlutoRepository, Data.SqlServer.Repositories.PlutoRepository>();
        //    services.AddScoped<IReferenceRepository, Data.SqlServer.Repositories.ReferenceRepository>();

        //    ConfigureServices(services);
        //}

        ///// <summary>
        ///// ConfigureServices - All Configurations
        ///// </summary>
        ///// <param name="services"></param>
        //private void ConfigureServices(IServiceCollection services)
        //{
        //    var config = Configuration.Get<ApplicationOptions>();

        //    var mapperConfig = new MapperConfiguration(cfg => { cfg.AddMaps("CoreDataStore.Service"); });
        //    IMapper mapper = new Mapper(mapperConfig);


        //    services.AddLogging(builder =>
        //    {
        //        builder.AddDebug();
        //        builder.AddConsole();
        //        builder.AddGelf(options =>
        //        {
        //            options.CompressUdp = true;
        //            options.Host = config.Graylog.Host;
        //            options.LogSource = config.Graylog.LogSource;
        //        });
        //    });

        //    services.AddSingleton(mapper);
        //    services.AddScoped<ILpcReportService, LpcReportService>();
        //    services.AddScoped<ILandmarkService, LandmarkService>();
        //    services.AddScoped<IPlutoService, PlutoService>();

        //    services.AddCustomHealthCheck(Configuration);
        //    services.AddCustomSwagger(Configuration);
        //    services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        //        .AllowAnyMethod()
        //        .AllowAnyHeader()));

        //    services.AddMvc();
        //}

        ///// <summary>
        ///// Configure Development
        ///// </summary>
        ///// <param name="app"></param>
        ///// <param name="loggerFactory"></param>
        //public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    app.UseDeveloperExceptionPage();
        //    AppConfig(app);
        //}

        ///// <summary>
        ///// Configure Staging
        ///// </summary>
        ///// <param name="app"></param>
        ///// <param name="loggerFactory"></param>
        //public void ConfigureStaging(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    app.UseExceptionHandler(
        //                  builder =>
        //                  {
        //                      builder.Run(
        //                        async context =>
        //                        {
        //                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

        //                            var error = context.Features.Get<IExceptionHandlerFeature>();
        //                            if (error != null)
        //                            {
        //                                context.Response.AddApplicationError(error.Error.Message);
        //                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
        //                            }
        //                        });
        //                  });

        //    AppConfig(app);
        //}

        ///// <summary>
        ///// Configure Production
        ///// </summary>
        ///// <param name="app"></param>
        ///// <param name="loggerFactory"></param>
        //public void ConfigureProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    app.UseExceptionHandler($"/Home/Error");
        //    AppConfig(app);
        //}

        //private void AppConfig(IApplicationBuilder app)
        //{
        //    AutoMapperConfiguration.Configure();

        //    app.UseDefaultFiles();
        //    app.UseStaticFiles();

        //    app.UseResponseHeaderMiddleware();

        //    app.UseHealthChecks($"/health");
        //    app.UseHealthChecksUI();

        //    app.UseSwagger();
        //    app.UseSwaggerUI(options =>
        //    {
        //        var swaggerDocument = $"/swagger/v1/swagger.json";
        //        options.SwaggerEndpoint(swaggerDocument, "CoreDataStore.Web");
        //    });

        //    app.UseMvc(ConfigureRoutes);
        //}

        //private void ConfigureRoutes(IRouteBuilder routeBuilder)
        //{
        //    routeBuilder.MapRoute(
        //        name: "default",
        //        template: "{controller=Home}/{action=Index}/{id?}");
        //}
    }
}
