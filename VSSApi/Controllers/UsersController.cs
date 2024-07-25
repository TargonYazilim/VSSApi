using Business.Services.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace VSSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var response = await _userService.Login(userDto);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public IActionResult CreateUser(UserDto userDto)
        {
            var response = _userService.AddUserAsync(userDto);
            return Ok(response);
        }
    }
}
