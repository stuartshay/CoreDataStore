using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDataStore.Data.Infrastructure;
using CoreDataStore.Domain.Entities;

namespace CoreDataStore.Data.Interfaces
{
    /// <summary>
    /// Landmark Repository interface
    /// </summary>
    public interface ILandmarkRepository : IEntityBaseRepository<Landmark>, IDisposable { }

    /// <summary>
    /// Lpc Report Repository interface
    /// </summary>
    public interface ILpcReportRepository : IEntityBaseRepository<LpcReport>, IDisposable { }

    /// <summary>
    /// Lpc Location Repository interface
    /// </summary>
    public interface ILpcLocationRepository : IEntityBaseRepository<LpcLocation>, IDisposable { }

    /// <summary>
    /// Lpc Lamppost Repository interface
    /// </summary>
    public interface ILpcLamppostRepository : IEntityBaseRepository<LpcLamppost>, IDisposable { }

    /// <summary>
    /// Pluto Repository interface
    /// </summary>
    public interface IPlutoRepository : IEntityBaseRepository<Pluto>, IDisposable
    {
        List<Pluto> GetPluto(string lpcNumber);

        Task<List<Pluto>> GetPlutoAsync(string lpcNumber);

        int GetPlutoCount(string lpcNumber);

        Task<int> GetPlutoCountAsync(string lpcNumber);
    }
}
