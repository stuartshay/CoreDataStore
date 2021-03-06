﻿using System;
using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.SqlServer.Repositories
{
    public class LpcLocationRepository : EntityBaseRepository<LpcLocation>, ILpcLocationRepository
    {
        public LpcLocationRepository(NycLandmarkContext context)
            : base(context)
        { }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
