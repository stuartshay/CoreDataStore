using System.IO;
using System.Linq;
using CoreDataStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreDataStore.Data.SqlServer.Repositories;
using Xunit;

namespace CoreDataStore.Data.SqlServer.Test.Repositories
{
    public class PlutoRepositoryTest
    {
        private readonly IPlutoRepository _plutoRepository;

        public PlutoRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("SqlServer");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseSqlServer(dbonnection))
                .AddScoped<IPlutoRepository, PlutoRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _plutoRepository = serviceProvider.GetRequiredService<IPlutoRepository>();
        }

        [Fact, Trait("Category", "Intergration")]
        public void Pluto_Block_Lot_Exist()
        {
            var results = _plutoRepository.FindBy(x => x.Block == 1 && x.Lot == 10).ToList();
            Assert.NotNull(results);
        }

        [Fact, Trait("Category", "Intergration")]
        public void Pluto_BBL_Exist()
        {
            var results = _plutoRepository.FindBy(x => x.BBL == 5080500013).ToList();
            Assert.NotNull(results);
        }

        [Fact, Trait("Category", "Intergration")]
        public void Get_Pluto_By_LPCNumber()
        {
            var lpNumber = "LP-02039";

            var results = _plutoRepository.FindBy(x => x.Landmark.LP_NUMBER == lpNumber).ToList();
            Assert.NotNull(results);
        }
    }
}
