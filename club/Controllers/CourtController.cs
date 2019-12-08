using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Authorization;

namespace club.Controllers
{
    [Route("/api/courts")]
    [Produces("application/json")]
    [ApiController]
    public class CourtsController : Controller
    {
        private readonly ICourtService _courtService;

        public CourtsController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        /// <summary>
        /// Lists all courts.
        /// </summary>
        /// <returns>List os courts.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(CourtListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            var response = await _courtService.ListAsync(pageNum, pageSize);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }
        
        /// <summary>
        /// Returns an existing court according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourtResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _courtService.GetAsync(id);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }


        /// <summary>
        /// Saves a new court.
        /// </summary>
        /// <param name="resource">court data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CourtResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] CourtSaveResource saveResource)
        {
            var response = await _courtService.SaveAsync(saveResource);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return CreatedAtAction(nameof(GetAsync), new { id = response.Result.Id }, response.Result);
        }

        /// <summary>
        /// Updates an existing court according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="resource">Updated member data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MemberResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] CourtSaveResource saveResource)
        {
            var response = await _courtService.UpdateAsync(id, saveResource);

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
        [ProducesResponseType(typeof(CourtResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _courtService.DeleteAsync(id);

            if (!result.Success && result.Error.Status == 404)
                return NotFound(result.Error);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Result);
        }
    }
}