using System;
using System.Collections.Generic;
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
using Navigator.Common.Helpers;

namespace CoreDataStore.Data.Postgre.Test.Repositories
{
    public class LPCReportRepositoryTest
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILPCReportRepository lpcReportRepository;
        private readonly NYCLandmarkContext dbContext;

        public LPCReportRepositoryTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<NYCLandmarkContext>(options =>
            options.UseNpgsql(@"User ID=nyclandmarks;Password=nyclandmarks;Server=192.168.99.100;Port=5432;Database=nyclandmarks;Integrated Security=true;Pooling=true;"));

            services.AddScoped<ILPCReportRepository, LPCReportRepository>();
            serviceProvider = services.BuildServiceProvider();

            dbContext = serviceProvider.GetRequiredService<NYCLandmarkContext>();
            lpcReportRepository = serviceProvider.GetRequiredService<ILPCReportRepository>();

        }

        //[Fact]
        [Fact(Skip = "ci test")]
        public void LPC_Reports_Exist()
        {
            var results = lpcReportRepository.GetAll();
            var count = results.Count();

            Assert.NotNull(results);
            Assert.NotEqual(0, count);
        }


        [Fact(Skip = "ci test")]
        public void Can_Load_LPC_Report()
        {
            var lpcReports = DataLoader.LoadLPCReports(@"./../../data/LPCReport.csv");

            dbContext.LPCReports.AddRange(lpcReports);
            dbContext.SaveChanges();
        }


        [Fact]
        public void Can_Get_Borough_List()
        {
            var results = EnumHelper.EnumToList<Borough>().Select(e => e.GetDescription()).ToList();
            Assert.NotNull(results);
        }


        [Fact(Skip = "ci test")]
        //[Fact]
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

            var results = lpcReportRepository
                .GetPage(predicate, request.PageSize * (request.Page - 1), request.PageSize, sortingList);

            Assert.NotNull(results);

        }


    }
}
