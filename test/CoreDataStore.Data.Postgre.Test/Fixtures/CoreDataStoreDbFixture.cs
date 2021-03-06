﻿using System;
using System.IO;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreDataStore.Data.Postgre.Test.Fixtures
{
    public class CoreDataStoreDbFixture : IDisposable
    {
        public CoreDataStoreDbFixture()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Console.WriteLine("ENV:" + env);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            DbConnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NycLandmarkContext>(options => options.UseNpgsql(DbConnection))
                .AddScoped<ILandmarkRepository, LandmarkRepository>()
                .AddScoped<ILpcLamppostRepository, LpcLamppostRepository>()
                .AddScoped<ILpcLocationRepository, LpcLocationRepository>()
                .AddScoped<ILpcReportRepository, LpcReportRepository>()
                .AddScoped<IPlutoRepository, PlutoRepository>()
                .BuildServiceProvider();

            DbContext = serviceProvider.GetRequiredService<NycLandmarkContext>();

            LandmarkRepository = serviceProvider.GetRequiredService<ILandmarkRepository>();
            LpcLamppostRepository = serviceProvider.GetRequiredService<ILpcLamppostRepository>();
            LpcLocationRepository = serviceProvider.GetRequiredService<ILpcLocationRepository>();
            LpcReportRepository = serviceProvider.GetRequiredService<ILpcReportRepository>();
            PlutoRepository = serviceProvider.GetRequiredService<IPlutoRepository>();
        }

        public string DbConnection { get; private set; }

        public NycLandmarkContext DbContext { get; private set; }

        public ILandmarkRepository LandmarkRepository { get; private set; }

        public ILpcLamppostRepository LpcLamppostRepository { get; private set; }

        public ILpcLocationRepository LpcLocationRepository { get; private set; }

        public ILpcReportRepository LpcReportRepository { get; private set; }

        public IPlutoRepository PlutoRepository { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LandmarkRepository.Dispose();
                LpcLamppostRepository.Dispose();
                LpcLocationRepository.Dispose();
                LpcReportRepository.Dispose();
                PlutoRepository.Dispose();
                DbContext.Dispose();
            }
        }
    }
}
