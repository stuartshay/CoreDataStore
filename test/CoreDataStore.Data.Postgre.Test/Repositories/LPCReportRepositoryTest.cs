using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using CoreDataStore.Data.Helpers;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Repositories;
using CoreDataStore.Common.Helpers;
using CoreDataStore.Data.Extensions;
using CoreDataStore.Data.Filters;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Domain.Enum;
using Microsoft.Extensions.Configuration;
using Navigator.Common.Helpers;

namespace CoreDataStore.Data.Postgre.Test.Repositories
{
    public class LPCReportRepositoryTest
    {
        private readonly ILPCReportRepository _lpcReportRepository;

        public LPCReportRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseNpgsql(dbonnection))
                .AddScoped<ILPCReportRepository, LPCReportRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _lpcReportRepository = serviceProvider.GetRequiredService<ILPCReportRepository>();
        }


        [Fact]
        public void LPC_Reports_Exist()
        {
            var results = _lpcReportRepository.GetAll().ToList();
            var count = results.Count();

            Assert.NotNull(results);
            Assert.NotEqual(0, count);
        }


        [Fact]
        public void Can_Get_Borough_List()
        {
            var results = EnumHelper.EnumToList<Borough>().Select(e => e.GetDescription()).ToList();
            Assert.NotNull(results);
        }


        [Fact]
        public void Can_Get_Filtered_Paging_List()
        {
            var predicate = PredicateBuilder.True<LPCReport>();
            var request = new LPCReportRequest()
            {
                PageSize = 20,
                Page = 1,
            };

            var sortModel = new SortModel
            {
                SortColumn = !string.IsNullOrEmpty(request.SortColumn) ? request.SortColumn : null,
                SortOrder = !string.IsNullOrEmpty(request.SortOrder) ? request.SortOrder : null
            };

            var sortingList = new List<SortModel>();
            sortingList.Add(sortModel);

            var results = _lpcReportRepository
                .GetPage(predicate, request.PageSize * (request.Page - 1), request.PageSize, sortingList).ToList();

            Assert.NotNull(results);

        }



        //[Fact(Skip = "ci test")]
        //public void Can_Load_LPC_Report()
        //{
        //    var lpcReports = DataLoader.LoadLPCReports(@"./../../data/LPCReport.csv");

        //    _dbContext.LPCReports.AddRange(lpcReports);
        //    _dbContext.SaveChanges();
        //}

    }
}
