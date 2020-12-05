using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDataStore.Service.Models;

namespace CoreDataStore.Service.Interfaces
{
    /// <summary>
    /// Pluto Service Interface
    /// </summary>
    public interface IPlutoService
    {
        /// <summary>
        /// Get Pluto
        /// </summary>
        /// <param name="lpcNumber">LPC Id</param>
        /// <returns></returns>
        List<PlutoModel> GetPluto(string lpcNumber);

        /// <summary>
        /// Get Pluto Async
        /// </summary>
        /// <param name="lpcNumber">LPC Id</param>
        /// <returns></returns>
        Task<List<PlutoModel>> GetPlutoAsync(string lpcNumber);


        /// <summary>
        /// Get Pluto Async
        /// </summary>
        /// <param name="bbl">BBL - UniqueId</param>
        /// <returns></returns>
        Task<PlutoModel> GetPlutoAsync(long bbl);

    }
}
