using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            registerDto.Username = registerDto.Username.ToLower();
            if(await _repo.UserExists(registerDto.Username))
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
    }
}