using System.IO;
using System.Linq;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreDataStore.Data.Postgre.Test.Repositories
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

            var dbonnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseNpgsql(dbonnection))
                .AddScoped<IPlutoRepository, PlutoRepository>()
                .BuildServiceProvider();

             serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _plutoRepository = serviceProvider.GetRequiredService<IPlutoRepository>();

        }

        [Fact, Trait("Category", "Intergration")]
        public void Pluto_Block_Lot_Exist()
        {
            int block = 1096;
            int lot = 54;

            var result = _plutoRepository.FindBy(x => x.Block == block && x.Lot == lot).FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(block, result.Block);
            Assert.Equal(lot, result.Lot);
        }

        [Fact, Trait("Category", "Intergration")]
        public void Pluto_BBL_Exist()
        {
            long bbl = 5080500013;
            var result = _plutoRepository.FindBy(x => x.BBL == bbl).FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal("10307", result.ZipCode);
            Assert.Equal(bbl, bbl);
        }

    }
}
