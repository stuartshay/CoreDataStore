using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CoreDataStore.Web.Controllers
{
    /// <summary>
    /// Location Controller
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class LocationController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        public LocationController()
        {

        }

        /// <summary>
        /// Validates User Postion is in Range
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Location/validate/")]
        public IActionResult Validate([FromBody] Position position)
        {
            return Ok(true);
        }


        /// <summary>
        ///  Get User Location
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Position), 200)]
        [Produces("application/json")]
        public IActionResult Post([FromBody]Position position)
        {
            var location = position;
            return Ok(location);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeStamp { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class Coords
    {
        /// <summary>
        /// 
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Accuracy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AltitudeAccuracy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Heading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Speed { get; set; }


    }


}
