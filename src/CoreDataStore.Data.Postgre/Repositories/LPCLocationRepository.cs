using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Postgre.Repositories
{
    public class LPCLocationRepository : EntityBaseRepository<LPCLocation>, ILPCLocationRepository
    {
        public LPCLocationRepository(NYCLandmarkContext context)
            : base(context)
        { }

    }
}
