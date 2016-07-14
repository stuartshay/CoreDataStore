using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace CoreDataStore.Data.SqlServer.Test.Helpers
{
    public class TemporaryDbContextFactory : IDbContextFactory<NYCLandmarkContext>
    {
        //http://benjii.me/2016/05/dotnet-ef-migrations-for-asp-net-core/
        //https://github.com/jerriep/Aspnet5DbContextTesting/blob/master/test/Aspnet5DbContextTesting.Tests/ProductsControllerTests.cs

        //#warning "Remove Hardcoded DB Connection String"

        public NYCLandmarkContext Create(DbContextFactoryOptions options)
        {
            Console.WriteLine(AppContext.BaseDirectory);
            var dbContextBuilder = new DbContextOptionsBuilder<NYCLandmarkContext>();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var connectionStringConfig = configBuilder.Build();
            var devConnection = connectionStringConfig.GetConnectionString("SqlServer");

            Console.WriteLine("DEV Connection" + devConnection);
            IConfigurationRoot Configuration = configBuilder.Build();
  

            dbContextBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=NycLandmarks;Integrated Security=True"
                , b => b.MigrationsAssembly(GetType().GetTypeInfo().Assembly.GetName().Name));

            return new NYCLandmarkContext(dbContextBuilder.Options);
        }
    }
}
