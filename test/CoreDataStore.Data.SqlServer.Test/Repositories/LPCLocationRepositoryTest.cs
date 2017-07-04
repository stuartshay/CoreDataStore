using System.IO;
using System.Linq;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace CoreDataStore.Data.SqlServer.Test.Repositories
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

            var dbonnection = builder.GetConnectionString("SqlServer");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseSqlServer(dbonnection))
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





        //[Fact, Trait("Category", "Intergration")]
        //public void Can_Get_LPCReport()
        //{
        //    var lpNumber = "LP-00871";
        //    var landmark = _dbContext.LPCReports.Where(x => x.LPNumber == lpNumber).Select(x => x.LPNumber).First();

        //    Assert.Equal(lpNumber, landmark);
        //}










    }
}
