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
    public class LPCLamppostRepositoryTest
    {
        private readonly ILPCLamppostRepository _lamppostRepository;
        private readonly NYCLandmarkContext _dbContext;

        public LPCLamppostRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbonnection = builder.GetConnectionString("PostgreSQL");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<NYCLandmarkContext>(options => options.UseNpgsql(dbonnection))
                .AddScoped<ILPCLamppostRepository, LPCLamppostRepository>()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<NYCLandmarkContext>();

            _dbContext = serviceProvider.GetRequiredService<NYCLandmarkContext>();
            _lamppostRepository = serviceProvider.GetRequiredService<ILPCLamppostRepository>(); 
        }


        [Fact, Trait("Category", "Intergration")]
        public void Get_LPC_Lamppost_List()
        {
            var results = _lamppostRepository.GetAll().ToList();
            var count = results.Count();

            Assert.NotNull(results);
            Assert.NotEqual(0, count);
        }


        [Fact, Trait("Category", "Intergration")]
        public void Can_Get_Lamppost()
        {
            var postId = 10;
            var result = _lamppostRepository.GetSingle(x => x.PostId == postId).PostId;  

            Assert.Equal(postId, result);
        }


    }
}
