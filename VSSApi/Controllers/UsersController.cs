﻿using Business.Services.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSSApi.Controllers
{
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var response = await _userService.Login(userDto);
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
