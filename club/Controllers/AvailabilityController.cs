using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Authorization;

namespace club.Controllers
{
    [Route("/api/availability")]
    [Produces("application/json")]
    [ApiController]
    public class AvailabilityController : Controller
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        /// <summary>
        /// Lists available courts.
        /// </summary>
        /// <returns>List of courts and its free slots.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(CourtAvailableListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> Availability(string dateTimeString, int sportId, int memberId, int pageNum = 1, int pageSize = 50)
        {
            var response = await _availabilityService.Availability(dateTimeString, sportId, memberId, pageNum, pageSize);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

     
    }
}