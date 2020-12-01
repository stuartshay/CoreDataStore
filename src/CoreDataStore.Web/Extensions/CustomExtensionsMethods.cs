using System;
using System.IO;
using System.Reflection;
using CoreDataStore.Data.Postgre;
using CoreDataStore.Web.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CoreDataStore.Web.Extensions
{
    /// <summary>
    /// Add ConfigurationOptions.
    /// </summary>
    public static class CustomExtensionsMethods
    {
        /// <summary>
        /// Configuration Options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration
            services.AddOptions();
            services.Configure<ApplicationOptions>(configuration);
            services.AddSingleton(configuration);

            return services;
        }

        /// <summary>
        /// Swagger Configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",  new OpenApiInfo()
                {
                    Title = "CoreDataStore.Web",
                    Description = "CoreDataStore.Web",
                    Version = "v1",
                });
                options.IncludeXmlComments(GetXmlCommentsPath());
            });

            return services;
        }

        /// <summary>
        ///   Custom Health Check.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<NycLandmarkContext>("DbContextHealthCheck");

            services.AddHealthChecksUI();

            return services;
        }

        /// <summary>
        ///  Swagger Documentation.
        /// </summary>
        /// <returns></returns>
        private static string GetXmlCommentsPath()
        {
            var basePath = AppContext.BaseDirectory;
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
            var fileName = Path.GetFileName(assemblyName + ".xml");

            return Path.Combine(basePath, fileName);
        }
    }
}
