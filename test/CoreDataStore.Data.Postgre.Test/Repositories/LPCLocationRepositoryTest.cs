using System.IO;
using System.Linq;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Repositories;
using CoreDataStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreDataStore.Data.Postgre.Test.Repositories
{
    public class LPCLocationRepositoryTest
    {
        private readonly ILPCLocationRepository _lpcLocationRepository;
        private readonly NYCLandmarkContext _dbContext;

        public LPCLocationRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseNpgsql(dbonnection))
                .AddScoped<ILPCLocationRepository, LPCLocationRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();

            _dbContext = serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _lpcLocationRepository = serviceProvider.GetRequiredService<ILPCLocationRepository>(); 
        }


        [Fact, Trait("Category", "Intergration")]
        public void Get_LPC_Locations_List()
        {
            var results = _lpcLocationRepository.GetAll().ToList();
            Assert.NotNull(results);
        }

        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_LPCLocation()
        {
            var lpNumber = "LP-00871";
            var lpcLocation = _dbContext.LPCLocation.Where(x => x.LPNumber == lpNumber).Select(x => x.LPNumber).First();

            Assert.Equal(lpNumber, lpcLocation);
        }


    }
}
