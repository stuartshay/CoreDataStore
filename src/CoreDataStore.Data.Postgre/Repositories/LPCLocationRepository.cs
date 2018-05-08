﻿using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Postgre.Repositories
{
    public class LPCLocationRepository : EntityBaseRepository<LPCLocation>, ILpcLocationRepository
    {
        public LPCLocationRepository(NYCLandmarkContext context)
            : base(context)
        { }

        public void Dispose()
        {
        }
    }
}
