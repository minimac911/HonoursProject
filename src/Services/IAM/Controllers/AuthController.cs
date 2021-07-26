using IAM.Data;
using IAM.Helpers;
using IAM.Services;
using IAM.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ITokenBuilder _tokenBuilder;

        public AuthController(UserContext context, ITokenBuilder tokenBuilder)
        {
            _context = context;
            _tokenBuilder = tokenBuilder;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user)
        {
            // check for user using username or email
            var foundUser = await _context
                .User
                .SingleOrDefaultAsync(usr => usr.Username == user.Username || usr.Email == user.Username);

            // if user was not found 
            if (foundUser == null)
            {
                return NotFound($"User with username or email of \'{user.Username}\' does not exist");
            }

            // TODO: Do hash checks
            var isValid = PasswordHasher.CheckHash(user.Password, foundUser.Password);

            if (!isValid)
            {
                return BadRequest($"Username/email or password were incrorect");
            }

            var token = _tokenBuilder.BuildToken(foundUser.Username);

            return Ok(token);
        }

        [HttpGet("verify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyJwtToken()
        {
            // get username from claims
            var username = User.Claims.SingleOrDefault();
    
            if(username == null)
            {
                return Unauthorized();
            }

            // does the user exsist with that username
            var userExist = await _context
                .User
                .AnyAsync(usr => usr.Username == username.Value);

            if (!userExist)
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
