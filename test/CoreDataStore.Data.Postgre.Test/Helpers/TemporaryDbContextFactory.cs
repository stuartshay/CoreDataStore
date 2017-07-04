using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoreDataStore.Data.Postgre.Test.Helpers
{
    //public class TemporaryDbContextFactory : IDbContextFactory<NYCLandmarkContext>
    //{
    //    //http://benjii.me/2016/05/dotnet-ef-migrations-for-asp-net-core/
    //    //https://github.com/jerriep/Aspnet5DbContextTesting/blob/master/test/Aspnet5DbContextTesting.Tests/ProductsControllerTests.cs

    //    public NYCLandmarkContext Create(DbContextFactoryOptions options)
    //    {
    //        var builder = new DbContextOptionsBuilder<NYCLandmarkContext>();
    //        builder.UseNpgsql(
    //            "User ID=nyclandmarks;Password=nyclandmarks;Server=192.168.1.6;Port=5432;Database=nyclandmarks;Integrated Security=true;Pooling=true;"
    //            , b => b.MigrationsAssembly(GetType().GetTypeInfo().Assembly.GetName().Name));

    //        return new NYCLandmarkContext(builder.Options);
    //    }
    //}

}
