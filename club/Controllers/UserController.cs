using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using club.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using club.Services;
using club.Resources;
using Microsoft.AspNetCore.Http;

namespace club.Controllers
{
    [Authorize]   
    [Route("/api/users")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,            
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(UserWithTokenResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> Authenticate([FromBody]UserAuthenticationResource userAuthenticationResource)
        {
            var response = await _userService.Authenticate(userAuthenticationResource.Email, userAuthenticationResource.Password);
            
            if (!response.Success)
                return BadRequest(response.Error);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, response.Result.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
                       
            // return basic user info (without password) and token to store client side
            UserWithTokenResource userToken = new UserWithTokenResource();

            userToken.Id = response.Result.Id;
            userToken.Email = response.Result.Email;
            userToken.FirstName = response.Result.FirstName;
            userToken.LastName = response.Result.LastName;
            userToken.Token = tokenString;

            return Ok(userToken);
        }

        /// <summary>
        /// Saves a new user.
        /// </summary>
        /// <param name="resource">User data.</param>
        /// <returns>Response for the request.</returns>
        [AllowAnonymous]
        [HttpPost]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]                
        public async Task<IActionResult> Register([FromBody]UserSaveResource userSaveResource)
        {
            var response = await _userService.Create(userSaveResource);

            if (!response.Success)
                return BadRequest(response.Error);

            return CreatedAtAction(nameof(Register), new { id = response.Result.Id }, response.Result);
        }

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>List of users.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(UserListResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAll(int pageNum = 1, int pageSize = 50)
        {
            var response = await _userService.GetAll(pageNum, pageSize);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Returns an existing user according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _userService.GetById(id);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
        }

        /// <summary>
        /// Updates an existing user according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="resource">Updated user data.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> Update(int id, [FromBody]UserSaveResource userSaveResource)
        {
            var response = await _userService.Update(id, userSaveResource);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(response.Result);
            
        }

        /// <summary>
        /// Deletes a given user according to an identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Response for the request.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResource), 404)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.Delete(id);

            if (!response.Success && response.Error.Status == 404)
                return NotFound(response.Error);

            if (!response.Success)
                return BadRequest(response.Error);

            return NoContent();
        }
    }
}