using Business.Services.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Login;

namespace VSSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _userService.Login(loginRequest);
            return Ok(response);
        }

  
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUserAsync(UserDto userDto)
        {
            var response = await _userService.AddUserAsync(userDto);
            return Ok(response);
        }
    }
}
