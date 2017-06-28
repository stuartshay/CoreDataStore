using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.SqlServer.Repositories
{
    public class LPCLocationRepository : EntityBaseRepository<LPCLocation>, ILPCLocationRepository
    {
        public LPCLocationRepository(NYCLandmarkContext context)
            : base(context)
        { }

    }
}
