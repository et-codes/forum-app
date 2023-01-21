using API.DTOs;
using API.Services;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<UserEntity> userManager, 
            TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                return BadRequest($"Username '{registerDto.Username}' is already taken.");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest($"Email address '{registerDto.Email}' is already taken.");
            }

            var user = new UserEntity
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };
            }

            return BadRequest(result.Errors);
        }
    }
}