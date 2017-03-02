using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CoreDataStore.Domain.Interfaces;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Mappings;
using CoreDataStore.Service.Services;
using CoreDataStore.Web.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Swashbuckle.Swagger.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Navigator.Middleware.HttpHeaders;

namespace CoreDataStore.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        private void ConfigService(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Core DataStore API",
                    Description = "Core DataStore API",
                    TermsOfService = "None"
                });

                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));

            }); 

            // Services
            services.AddScoped<ILPCReportService, LPCReportService>();
            services.AddScoped<ILandmarkService, LandmarkService>();
            services.AddScoped<IPlutoService, PlutoService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                .AllowAnyMethod()
                                                                .AllowAnyHeader()));

            var connection = Configuration["ConnectionStrings:Sqlite"];
            services.AddDbContext<Data.Sqlite.NYCLandmarkContext>(options => options.UseSqlite(connection));

            // Repositories
            services.AddScoped<ILPCReportRepository, Data.Sqlite.Repositories.LPCReportRepository>();
            services.AddScoped<ILandmarkRepository, Data.Sqlite.Repositories.LandmarkRepository>();
            services.AddScoped<IPlutoRepository, Data.Sqlite.Repositories.PlutoRepository>();
            services.AddScoped<IReferenceRepository, Data.Sqlite.Repositories.ReferenceRepository>();

            ConfigService(services);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureStagingServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                .AllowAnyMethod()
                                                                .AllowAnyHeader()));

            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables("");
            var config = builder.Build();

            var stageConnection = Configuration["ConnectionStrings:PostgreSQL"];

            //EnvironmentVariable Exist Overide
            if (!string.IsNullOrWhiteSpace(config["CONNECTION_PostgreSQL"]))
                stageConnection = config["CONNECTION_PostgreSQL"];

            services.AddDbContext<Data.Postgre.NYCLandmarkContext>(options => options.UseNpgsql(stageConnection));
            
            // Repositories
            services.AddScoped<ILPCReportRepository, Data.Postgre.Repositories.LPCReportRepository>();
            services.AddScoped<ILandmarkRepository, Data.Postgre.Repositories.LandmarkRepository>();
            services.AddScoped<IPlutoRepository, Data.Postgre.Repositories.PlutoRepository>();
            services.AddScoped<IReferenceRepository, Data.Postgre.Repositories.ReferenceRepository>();

            ConfigService(services);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader()));

            var devConnection = Configuration["ConnectionStrings:SqlServer"];
            services.AddDbContext<Data.SqlServer.NYCLandmarkContext>(options => options.UseSqlServer(devConnection));

            // Repositories
            services.AddScoped<ILPCReportRepository, Data.SqlServer.Repositories.LPCReportRepository>();
            services.AddScoped<ILandmarkRepository, Data.SqlServer.Repositories.LandmarkRepository>();
            services.AddScoped<IPlutoRepository, Data.SqlServer.Repositories.PlutoRepository>();
            services.AddScoped<IReferenceRepository, Data.SqlServer.Repositories.ReferenceRepository>();

            ConfigService(services);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            //app.UseBrowserLink();
            //app.UseDatabaseErrorPage();

            AppConfig(app, loggerFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        public void ConfigureStaging(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //app.UseExceptionHandler("/Home/Error");
            app.UseExceptionHandler(
                          builder =>
                          {
                              builder.Run(
                                async context =>
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                                    var error = context.Features.Get<IExceptionHandlerFeature>();
                                    if (error != null)
                                    {
                                        context.Response.AddApplicationError(error.Error.Message);
                                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                                    }
                                });
                          }); 

            AppConfig(app, loggerFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        public void ConfigureProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler("/Home/Error");
            //loggerFactory.AddProvider(new SqlLoggerProvider());

            AppConfig(app, loggerFactory);

        }

        private void AppConfig(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {         
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            AutoMapperConfiguration.Configure();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseResponseHeaderMiddleware();
            app.UseMvc(ConfigureRoutes);

            app.UseSwagger();  
            app.UseSwaggerUi();

        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

        private string GetXmlCommentsPath(ApplicationEnvironment appEnvironment)
        {
            var documentationFile = appEnvironment.ApplicationName + ".xml";
            return Path.Combine(appEnvironment.ApplicationBasePath, documentationFile);
        }

    }
}
