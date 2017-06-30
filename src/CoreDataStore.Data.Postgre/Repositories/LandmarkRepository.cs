using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Postgre.Repositories
{
    public class LandmarkRepository : EntityBaseRepository<Landmark>, ILandmarkRepository
    {
        public LandmarkRepository(NYCLandmarkContext context) 
            : base(context)
        {
        }
    }
}
