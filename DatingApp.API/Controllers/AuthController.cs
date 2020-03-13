using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private UserManager<User> _userManager;
        private IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            registerDto.Username = registerDto.Username.ToLower();
            if (await _repo.UserExists(registerDto.Username))
            {
                return BadRequest("User name already exists!");
            }
            var userToCreate = new User
            {
                Username = registerDto.Username
            };
            var createUser = await _repo.Register(userToCreate, registerDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto loginDto)
        {
        
            throw new Exception("Computer say no");
            var userFromRepo = await _repo.Login(loginDto.Username, loginDto.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim("Username",userFromRepo.Username),
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthSettings:Key"]));
            var token = new JwtSecurityToken(
                issuer: _config["AuthSettings:Issuer"],
            audience: _config["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new {
                token = tokenAsString
            });
        }
    }
}