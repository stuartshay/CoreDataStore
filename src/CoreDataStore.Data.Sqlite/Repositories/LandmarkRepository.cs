﻿using System;
using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Sqlite.Repositories
{
    public class LandmarkRepository : EntityBaseRepository<Landmark>, ILandmarkRepository
    {
        private NycLandmarkContext _context;

        public LandmarkRepository(NycLandmarkContext context)
            : base(context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}
