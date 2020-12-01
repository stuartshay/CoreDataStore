using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreDataStore.Web.Controllers
{
    /// <summary>
    /// PLUTO Controller.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlutoController : Controller
    {
        private readonly IPlutoService _plutoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlutoController"/> class.
        /// </summary>
        /// <param name="plutoService"></param>
        public PlutoController(IPlutoService plutoService)
        {
            this._plutoService = plutoService ?? throw new ArgumentNullException(nameof(plutoService));
        }

        /// <summary>
        ///  Get Pluto for LPC Landmark.
        /// </summary>
        /// <param name="lpcNumber"></param>
        /// <returns></returns>
        [HttpGet("{lpcNumber}")]
        [Produces("application/json", Type = typeof(IEnumerable<PlutoModel>))]
        [ProducesResponseType(typeof(IEnumerable<PlutoModel>), 200)]
        public async Task<IActionResult> Get(string lpcNumber)
        {
            if (lpcNumber == null)
                return BadRequest();

            var result = await _plutoService.GetPlutoAsync(lpcNumber);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
