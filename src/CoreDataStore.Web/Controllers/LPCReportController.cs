using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDataStore.Data.Filters;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Models;
using CoreDataStore.Service.ValidationRules;
using CoreDataStore.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDataStore.Web.Controllers
{
    /// <summary>
    /// LPC Reports API Controller.
    /// </summary>
    [Route("api/[controller]")]
    public class LpcReportController : Controller
    {
        private readonly ILpcReportService _lpcReportService;
        private readonly ILandmarkService _landmarkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LpcReportController"/> class.
        /// </summary>
        /// <param name="lpcReportService"></param>
        /// <param name="landmarkService"></param>
        public LpcReportController(ILpcReportService lpcReportService, ILandmarkService landmarkService)
        {
            _lpcReportService = lpcReportService ?? throw new ArgumentNullException(nameof(lpcReportService));
            _landmarkService = landmarkService ?? throw new ArgumentNullException(nameof(landmarkService));
        }

        /// <summary>
        /// Get LPC Report
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(LpcReportModel))]
        [ProducesResponseType(typeof(LpcReportModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
                return BadRequest();

            var result = await _lpcReportService.GetLPCReportAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        ///  Put Update LPC Report
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]LpcReportModel model)
        {
            if (id == 0)
                return BadRequest();

            var validator = new LpcReportRule();
            var validationResults = validator.Validate(model);

            if (!validationResults.IsValid)
                return Json(validationResults.Errors);

            var result = _lpcReportService.GetLPCReport(id);
            if (result == null)
                return NotFound();

            _lpcReportService.UpdateLPCReport(model);

            return new NoContentResult();
        }

        /// <summary>
        /// Get LPC Reports Filtered Results.
        /// </summary>
        /// <param name="query">Query String Parms</param>
        /// <param name="limit">Records per Page</param>
        /// <param name="page">Page Number</param>
        /// <returns></returns>
        [HttpGet("{limit:int}/{page:int}")]
        [Produces("application/json", Type = typeof(IEnumerable<LpcReportModel>))]
        [ProducesResponseType(typeof(IEnumerable<LpcReportModel>), 200)]
        public IActionResult Get([FromQuery]LPCReportRequestModel query, int limit, int page)
        {
            var request = new LpcReportRequest
            {
                PageSize = limit,
                Page = page,
                SortColumn = !string.IsNullOrEmpty(query.Sort) ? query.Sort : "LPNumber",
                SortOrder = !string.IsNullOrEmpty(query.Order) ? query.Order : "asc",
                ParentStyleList = !string.IsNullOrEmpty(query.ParentStyles) ? query.ParentStyles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : null,
                Neighborhood = !string.IsNullOrWhiteSpace(query.Neighborhood) ? query.Neighborhood.Trim() : null,
                Borough = !string.IsNullOrWhiteSpace(query.Borough) ? query.Borough.Trim() : null,
                ObjectType = !string.IsNullOrWhiteSpace(query.ObjectType) ? query.ObjectType.Trim() : null,
            };

            var records = _lpcReportService.GetLPCReports(request);
            var totalRecords = records.Total;
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-InlineCount");
            HttpContext.Response.Headers.Add("X-InlineCount", totalRecords.ToString());

            return Ok(records.Results);
        }

        /// <summary>
        /// Get Landmark Filtered Results.
        /// </summary>
        /// <param name="query">Query String Parms</param>
        /// <param name="limit">Records per Page</param>
        /// <param name="page">Page Number</param>
        /// <returns></returns>
        [Produces("application/json", Type = typeof(IEnumerable<LandmarkModel>))]
        [ProducesResponseType(typeof(IEnumerable<LandmarkModel>), 200)]
        [HttpGet("landmark/{limit:int}/{page:int}")]
        public IActionResult GetLandmarks([FromQuery]LandmarkRequestModel query, int limit, int page)
        {
            long totalRecords;
            var request = new LandmarkRequest
            {
                PageSize = limit,
                Page = page,
                SortColumn = !string.IsNullOrEmpty(query.Sort) ? query.Sort : "LP_NUMBER",
                SortOrder = !string.IsNullOrEmpty(query.Order) ? query.Order : "asc",
                LpcNumber = query.LpcNumber,
            };

            var records = _landmarkService.GetLandmarks(request);
            totalRecords = records.Total;
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-InlineCount");
            HttpContext.Response.Headers.Add("X-InlineCount", totalRecords.ToString());

            return Ok(records.Results.OrderBy(x => x.Street).ThenBy(x => x.Number));
        }

        /// <summary>
        /// Get Landmark Street List.
        /// </summary>
        /// <param name="lpcNumber"></param>
        /// <returns></returns>
        [Produces("application/json", Type = typeof(IEnumerable<string>))]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet("landmark/streets/{lpcNumber}")]
        public IActionResult GetStreets(string lpcNumber)
        {
            if (lpcNumber == null)
                return BadRequest();

            var results = _landmarkService.GetLandmarkStreets(lpcNumber.Trim());
            var totalRecords = results.Count;

            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-InlineCount");
            HttpContext.Response.Headers.Add("X-InlineCount", totalRecords.ToString());

            return Ok(results);
        }
    }
}
