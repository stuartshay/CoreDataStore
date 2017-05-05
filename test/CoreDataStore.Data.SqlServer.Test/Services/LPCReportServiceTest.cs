using System.IO;
using CoreDataStore.Data.Filters;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.SqlServer.Repositories;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Mappings;
using CoreDataStore.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreDataStore.Data.SqlServer.Test.Services
{
    public class LPCReportServiceTest
    {
        private readonly ILPCReportService _lpcReportService;

        public LPCReportServiceTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("SqlServer");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseSqlServer(dbonnection))
                .AddScoped<ILPCReportRepository, LPCReportRepository>()
                .AddScoped<ILPCReportService, LPCReportService>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();
            serviceProvider.GetRequiredService<ILPCReportRepository>();

            _lpcReportService = serviceProvider.GetRequiredService<ILPCReportService>();

            AutoMapperConfiguration.Configure();
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Filtered_Paging_List()
        {
            var request = new LPCReportRequest
            {
                PageSize = 20,
                Page = 1,
                Borough = "Manhattan"
            };

            var results = _lpcReportService.GetLPCReports(request).Results;
            Assert.Equal(request.PageSize, results.Count);
        }

    }
}
