using System;
using System.Collections.Generic;
using System.Linq;
using CoreDataStore.Data.Data;
using CoreDataStore.Domain.Enum;
using CoreDataStore.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Navigator.Common.Helpers;

namespace CoreDataStore.Web.Controllers
{
    /// <summary>
    /// Reference Data Controller
    /// </summary>
    [Route("api/[controller]")]
    public class ReferenceController : Controller
    {
        private readonly IReferenceRepository _referenceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceController"/> class.
        /// </summary>
        /// <param name="referenceRepository"></param>
        public ReferenceController(IReferenceRepository referenceRepository)
        {
            this._referenceRepository = referenceRepository ?? throw new ArgumentNullException(nameof(referenceRepository));
        }

        /// <summary>
        ///  Get Boroughs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("borough")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [Produces("application/json", Type = typeof(IEnumerable<string>))]
        public IActionResult GetBoroughs()
        {
            return Ok(EnumHelper.EnumToList<Borough>().Select(e => e.GetDescription()));
        }

        /// <summary>
        ///  Get Reference Types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("objectType")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [Produces("application/json", Type = typeof(IEnumerable<string>))]
        public IActionResult GetReferenceTypes()
        {
            return Ok(EnumHelper.EnumToList<ObjectType>().Select(e => e.GetDescription()));
        }

        /// <summary>
        ///  Get Parent Styles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("parentStyle")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [Produces("application/json", Type = typeof(IEnumerable<string>))]
        public IActionResult GetParentStyles()
        {
            return Ok(StylesData.GetParentStyles());
        }
    }
}
