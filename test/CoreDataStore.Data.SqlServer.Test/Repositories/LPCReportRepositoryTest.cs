using System.IO;
using System.Linq;
using CoreDataStore.Data.Extensions;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using CoreDataStore.Data.Filters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using CoreDataStore.Common.Helpers;
using CoreDataStore.Data.Helpers;

namespace CoreDataStore.Data.SqlServer.Test.Repositories
{
    public class LPCReportRepositoryTest
    {
        private readonly ILPCReportRepository _lpcReportRepository;
        private readonly NYCLandmarkContext _dbContext;

        public LPCReportRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("SqlServer");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseSqlServer(dbonnection))
                .AddScoped<ILPCReportRepository, LPCReportRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();

            _dbContext = serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _lpcReportRepository = serviceProvider.GetRequiredService<ILPCReportRepository>();
        }


        [Fact, Trait("Category", "Intergration")]
        public void LPC_Reports_Exist()
        {
            var results = _lpcReportRepository.GetAll().ToList();
            Assert.NotNull(results);
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Update_LPC_Report()
        {
            var lpcReport = _lpcReportRepository.GetSingle(1);
            lpcReport.Name = "Pieter Claesen Wyckoff House X";

            var result = _dbContext.SaveChanges();
            Assert.NotNull(result);
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_LPCReport()
        {
            var lpNumber = "LP-00871";
            var landmark = _dbContext.LPCReports.Where(x => x.LPNumber == lpNumber).Select(x => x.LPNumber).First();

            Assert.Equal(lpNumber, landmark);
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Included_Fields()
        {
            var lpNumber = "LP-00871";
            var landmarkCount = 4;

            var landmark = _dbContext.LPCReports.Include(x => x.Landmarks).Where(x => x.LPNumber == lpNumber).Select(x => x).First();

            Assert.Equal(lpNumber, landmark.LPNumber);
            Assert.Equal(landmarkCount, landmark.Landmarks.Count);
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Paging_List()
        {
            //int count = 0;
            var request = new LPCReportRequest
            {
                PageSize = 20,
                Page = 1,
            };

            var sortingList = new List<SortModel>()
            {
                new SortModel
                {
                    SortColumn = "name",
                    SortOrder = "asc"
                }
            };

            var results = _lpcReportRepository.GetPage(request.Page, request.PageSize, sortingList).ToList();
            Assert.Equal(request.PageSize, results.Count);

        }


        [Fact(Skip = "ci test")]
        public void Can_Load_LPC_Report()
        {
            var lpcReports = DataLoader.LoadLPCReports(@"./../../data/LPCReport.csv");

            _dbContext.LPCReports.AddRange(lpcReports);
            _dbContext.SaveChanges();
        }


        [Fact(Skip = "ci test")]
        public void Can_Load_Landmarks()
        {
            int batchSize = 10000;

            var landmarks = DataLoader.LoadLandmarks(@"./../../data/Landmarks.csv").ToList();
            foreach (var list in landmarks.Batch(batchSize))
            {
                foreach (var mark in list)
                {
                    var exsiting = _dbContext.LPCReports.Where(l => l.LPNumber == mark.LP_NUMBER);
                    if (exsiting.Any())
                    {
                        _dbContext.Landmarks.AddRange(mark);
                    }
                }

                if (_dbContext.Landmarks.Any())
                {
                    _dbContext.SaveChanges();
                }
            }
        }

    }
}
