using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace club.Controllers
{
    [Route("/api/members")]
    [Produces("application/json")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// Lists all members.
        /// </summary>
        /// <returns>List os members.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(SportListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> ListAsync(int pageNum = 1, int pageSize = 50)
        {
            var response = await _memberService.ListAsync(pageNum, pageSize);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Returns an existing member according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MemberResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _memberService.GetAsync(id);

            if (!response.Success && response.Error.status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }


        /// <summary>
        /// Saves a new member.
        /// </summary>
        /// <param name="resource">member data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(MemberResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] MemberSaveResource saveResource)
        {
            var response = await _memberService.SaveAsync(saveResource);

            if (!response.Success)
                return BadRequest(response.Error);
            
            return CreatedAtAction(nameof(GetAsync), new { id = response.Result.Id }, response.Result);
        }

        /// <summary>
        /// Updates an existing member according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="resource">Updated member data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MemberResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] MemberSaveResource saveResource)
        {
            var response = await _memberService.UpdateAsync(id, saveResource);

            if (!response.Success && response.Error.status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Deletes a given member according to an identifier.
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
            var result = await _memberService.DeleteAsync(id);

            if (!result.Success && result.Error.status == 404)
                return NotFound(result.Error);

            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}