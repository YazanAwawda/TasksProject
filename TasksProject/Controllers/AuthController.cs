using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TasksProject.DataEntity;
using TasksProject.Dto;
using TasksProject.Models;
using TasksProject.Service;

namespace TasksProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerRequest)
        {
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.PasswordHash))
            {
                return BadRequest("Invalid registration information.");
            }

            var result = await _userService.RegisterAsync(registerRequest);
            if (result)
            {
                return Ok("Registration successful.");
            }
            return BadRequest("User already exists.");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.PasswordHash))
            {
                return BadRequest("Invalid login information.");
            }

            var userResponse = await _userService.LoginAsync(loginRequest);
            if (userResponse != null)
            {
                return Ok(userResponse);
            }
            return BadRequest("Wrong account or password");
        }
    }
}