using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Authorization;

namespace club.Controllers
{
    [Route("/api/bookings")]
    [Produces("application/json")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Lists all bookings of a given date.
        /// </summary>
        /// <returns>List os bookings.</returns>
        [Authorize]
        [HttpGet("byDate/{date}")]
        [ProducesResponseType(typeof(BookingListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> ListOfDAteAsync(string date, int pageNum = 1, int pageSize = 50)
        {
            var response = await _bookingService.ListAsync(pageNum, pageSize, date);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }
        
        /// <summary>
        /// Lists all bookings.
        /// </summary>
        /// <returns>List os bookings.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(BookingListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> ListAsync(int pageNum = 1, int pageSize = 50)
        {            
            var response = await _bookingService.ListAsync(pageNum, pageSize);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }


        /// <summary>
        /// Returns an existing booking according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookingResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _bookingService.GetAsync(id);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);



            return Ok(response.Result);
        }


        /// <summary>
        /// Saves a new booking.
        /// </summary>
        /// <param name="resource">booking data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BookingResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] BookingSaveResource saveResource)
        {
            var response = await _bookingService.SaveAsync(saveResource);

            if (!response.Success)
                return BadRequest(response.Error);

            return CreatedAtAction(nameof(GetAsync), new { id = response.Result.Id }, response.Result);
        }

        /// <summary>
        /// Updates an existing booking according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="resource">Updated booking data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BookingResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] BookingSaveResource saveResource)
        {
            var response = await _bookingService.UpdateAsync(id, saveResource);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Deletes a given court according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BookingResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _bookingService.DeleteAsync(id);

            if (!result.Success && result.Error.Status == 404)
                return NotFound(result.Error);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Result);
        }
    }
}