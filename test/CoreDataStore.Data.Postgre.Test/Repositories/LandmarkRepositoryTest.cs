using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreDataStore.Common.Helpers;
using CoreDataStore.Data.Extensions;
using CoreDataStore.Data.Filters;
using CoreDataStore.Data.Helpers;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Repositories;
using CoreDataStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreDataStore.Data.Postgre.Test.Repositories
{
    public class LandmarkRepositoryTest
    {
        private readonly ILandmarkRepository _landmarkRepository;
        private readonly NYCLandmarkContext _dbContext;

        public LandmarkRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseNpgsql(dbonnection))
                .AddScoped<ILandmarkRepository, LandmarkRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();

            _dbContext = serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _landmarkRepository = serviceProvider.GetRequiredService<ILandmarkRepository>();
        }

        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Landmark()
        {
            var id = 100;
            var result = _landmarkRepository.GetSingle(id);
            Assert.NotNull(result);
        }

        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Filtered_Paging_List()
        {
            var predicate = PredicateBuilder.True<Landmark>();
            var request = new LandmarkRequest()
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

            var results = _landmarkRepository
                .GetPage(predicate, request.PageSize * (request.Page - 1), request.PageSize, sortingList);

            Assert.NotNull(results);

        }


        [Fact(Skip = "ci test")]
        public void Can_Load_Landmarks()
        {
            int batchSize = 1000;

            var landmarks = DataLoader.LoadLandmarks(@"./../../data/Landmarks.csv").ToList();
            foreach (var list in landmarks.Batch(batchSize))
            {
                _dbContext.Landmarks.AddRange(list);
                _dbContext.SaveChanges();
            }
        }

    }
}
