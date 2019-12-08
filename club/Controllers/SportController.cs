using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace club.Controllers
{
    [Route("/api/sports")]
    [Produces("application/json")]
    [ApiController]
    public class SportsController : Controller
    {
        private readonly ISportService _sportService;

        public SportsController(ISportService sportService)
        {
            _sportService = sportService;
        }

        /// <summary>
        /// Lists all sports.
        /// </summary>
        /// <returns>List os sports.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(SportListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            var response = await _sportService.ListAsync(pageNum, pageSize);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }


        /// <summary>
        /// Returns an existing sport according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SportResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response =  await _sportService.GetAsync(id);

            if (!response.Success && response.Error.status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }


        /// <summary>
        /// Saves a new sport.
        /// </summary>
        /// <param name="resource">sport data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SportResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SportSaveResource saveResource)
        {
            var response = await _sportService.SaveAsync(saveResource);

            if (!response.Success)
                return BadRequest(response.Error);

            return CreatedAtAction(nameof(GetAsync), new { id = response.Result.Id }, response.Result);
        }

        /// <summary>
        /// Updates an existing sport according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="resource">Updated sport data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SportResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SportSaveResource saveResource)
        {
            var response = await _sportService.UpdateAsync(id, saveResource);

            if (!response.Success && response.Error.status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Deletes a given sport according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _sportService.DeleteAsync(id);

            if (!result.Success && result.Error.status == 404)
                return NotFound(result.Error);

            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}