using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Interfaces
{
    public interface ILandmarkRepository : IEntityBaseRepository<Landmark> { }

    public interface ILPCReportRepository : IEntityBaseRepository<LPCReport> { }

    public interface ILPCLocationRepository : IEntityBaseRepository<LPCLocation> { }

    public interface ILPCLamppostRepository : IEntityBaseRepository<LPCLamppost> { }

    public interface IPlutoRepository : IEntityBaseRepository<Pluto> { }

}
